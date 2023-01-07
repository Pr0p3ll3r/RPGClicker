using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        Database.data = database;
        database.ResetLocations();
    }

    [Header("Main")]
    [SerializeField] private Transform lootList;
    [SerializeField] private Animation popupText;
    [SerializeField] private GameObject exitBox;
    [SerializeField] private DatabaseSO database;
    [SerializeField] private TextMeshProUGUI locationName;

    [Header("Player")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button setNameButton;
    [SerializeField] private TMP_Dropdown classDropdown;

    [Header("Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject playerPanel;
    [SerializeField] private GameObject adventurePanel;
    [SerializeField] private GameObject towerPanel;
    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject achievementsPanel;

    [Header("Adventure")]
    [SerializeField] private GameObject locationPrefab;
    [SerializeField] private GameObject locationPanel;
    [SerializeField] private Transform locationList;
    [SerializeField] private Button locationButton;
    [SerializeField] private GameObject dungeonPanel;
    [SerializeField] private Transform dungeonsList;
    [SerializeField] private Button dungeonButton;
    [SerializeField] private TextMeshProUGUI dungeonEnemy;

    [Header("Poolers")]
    public ObjectPooler damagePopupPooler;

    private Player Player => Player.Instance;
    private PlayerInventory Inventory => PlayerInventory.Instance;
    private Enemy Enemy => Enemy.Instance;
    private QuestManager questManager;
    private AchievementManager achievementManager;

    int fingerCount;
    bool screenPressed = false;

    private int lastlootSlot;
    private int lootSlots = 6;

    private Location currentLocation;
    private Location previousLocation;

    private int dungeonEnemyCount;

    private void Start()
    {
        questManager = GetComponent<QuestManager>();
        achievementManager = GetComponent<AchievementManager>();

        ClearLootList();

        startPanel.SetActive(false);
        mainPanel.SetActive(false);
        playerPanel.SetActive(false);
        adventurePanel.SetActive(false);
        towerPanel.SetActive(false);
        questPanel.SetActive(false);
        achievementsPanel.SetActive(false);

        CreateLocationList();
        CloseAdventurePanels();
        locationPanel.SetActive(true);
        locationButton.onClick.AddListener(delegate { CloseAdventurePanels(); locationPanel.SetActive(true); });
        dungeonButton.onClick.AddListener(delegate { CloseAdventurePanels(); dungeonPanel.SetActive(true); });
        dungeonEnemy.text = "";

        if (PlayerPrefs.GetInt("NewGame", 1) == 1)
        {
            startPanel.SetActive(true);
            setNameButton.onClick.AddListener(delegate { StartNewGame(); });
        }
        else
        {
            Data.Load();
            SetGame();
        }
    }

    private void Update()
    {
        //if (screenPressed)
        //{
        //    fingerCount = 0;
        //    foreach (Touch touch in Input.touches)
        //    {
        //        if (touch.phase == TouchPhase.Began && fingerCount < 1)
        //        {
        //            Attack();
        //        }
        //        fingerCount++;
        //    }
        //    screenPressed = false;
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitBox.SetActive(true);
        }
    }

    private void StartNewGame()
    {
        if (string.IsNullOrEmpty(nameInput.text))
        {
            setNameButton.Select();
            return;
        }
        
        PlayerData newPlayer = new PlayerData(nameInput.text, (PlayerClass)classDropdown.value);
        Player.data = newPlayer;
        PlayerPrefs.SetInt("NewGame", 0);
        SetGame();
    }

    private void SetGame()
    {
        Player.GetComponent<PlayerUI>().UpdateHealthBar();
        Player.GetComponent<PlayerInfo>().RefreshStats();
        Player.GetComponent<PlayerInfo>().SetStatsDescription();
        mainPanel.SetActive(true);
        ChangeLocation(Database.data.locations[0]);
        foreach (EquipmentSlot slot in PlayerEquipment.Instance.Slots)
            slot.SetRightPlaceholder();
        if(Player.data.completedQuests < Database.data.quests.Length)
            questManager.SetQuest(Database.data.quests[Player.data.completedQuests]);
        else
            questManager.SetQuest(null);
    }

    public void Reward(EnemyData enemy)
    {  
        if(!Inventory.IsFull())
        {
            Item drop = LootTable.Drop(enemy.loot);
            if (drop != null)
            {
                if (lastlootSlot == lootSlots)
                    ClearLootList();
                Debug.Log("Dropped: " + drop.itemName);
                Inventory.AddItem(drop, 1);
                lootList.GetChild(lastlootSlot).GetComponent<InventorySlot>().FillSlot(drop);
                lastlootSlot++;
            }
        }

        if (enemy.isBoss)
            currentLocation.bossDefeated = true;

        Player.Reward(enemy.exp, enemy.gold);
        questManager.QuestProgress(enemy);
        achievementManager.AchievementProgress(enemy.achievements);
    }

    private void ClearLootList()
    {
        foreach (Transform lootSlot in lootList.transform)
        {
            lootSlot.GetComponent<InventorySlot>().ClearSlot();
        }
        lastlootSlot = 0;
    }

    public void ShowText(string text, Color color)
    {
        popupText.GetComponentInChildren<TextMeshProUGUI>().text = text;
        popupText.GetComponentInChildren<TextMeshProUGUI>().color = color;
        popupText.Stop();
        popupText.Play();
    }

    public void CheckFingers()
    {
        screenPressed = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenAdventurePanel()
    {       
        RefreshLocationList();
        adventurePanel.SetActive(true);
    }

    private void CloseAdventurePanels()
    {
        locationPanel.SetActive(false);
        dungeonPanel.SetActive(false);
    }

    private void CreateLocationList()
    {
        foreach (Location location in Database.data.locations)
        {
            GameObject locationGO = Instantiate(locationPrefab, locationList);
            locationGO.GetComponent<LocationInfo>().SetUp(location);
            locationGO.GetComponent<Button>().onClick.AddListener(delegate { ChangeLocation(location); });
            locationGO.transform.Find("Unlock").gameObject.SetActive(false);
            locationGO.transform.Find("Unlock/ButtonUnlock").GetComponent<Button>().onClick.AddListener(delegate { UnlockLocation(location); });
            locationGO.GetComponent<Button>().interactable = false;
        }

        foreach (Location dungeon in Database.data.dungeons)
        {
            GameObject bossGO = Instantiate(locationPrefab, dungeonsList);
            bossGO.GetComponent<LocationInfo>().SetUp(dungeon);
            bossGO.GetComponent<Button>().onClick.AddListener(delegate { EnterDungeon(dungeon); });
            bossGO.transform.Find("Unlock").gameObject.SetActive(true);
            bossGO.transform.Find("Unlock/ButtonUnlock").gameObject.SetActive(false);
            bossGO.GetComponent<Button>().interactable = false;
        }
    }

    private void RefreshLocationList()
    {
        for (int i = 0; i < locationList.childCount; i++)
        {
            if (Player.data.level < Database.data.locations[i].lvlMin)
            {
                locationList.GetChild(i).GetComponent<Button>().interactable = false;
                locationList.GetChild(i).transform.Find("Unlock").gameObject.SetActive(true);
                locationList.GetChild(i).transform.Find("Unlock/ButtonUnlock").GetComponent<Button>().interactable = false;
            }
            else if (Database.data.locations[i].unlocked)
            {
                locationList.GetChild(i).GetComponent<Button>().interactable = true;
                locationList.GetChild(i).transform.Find("Unlock").gameObject.SetActive(false);
            }
            else
            {
                locationList.GetChild(i).transform.Find("Unlock").gameObject.SetActive(true);
                locationList.GetChild(i).transform.Find("Unlock/ButtonUnlock").GetComponent<Button>().interactable = true;
            }

            if (currentLocation == Database.data.locations[i])
                locationList.GetChild(i).GetComponent<Button>().interactable = false;
        }

        for (int i = 0; i < dungeonsList.childCount; i++)
        {
            if (Player.data.level < Database.data.dungeons[i].lvlMin)
                dungeonsList.GetChild(i).GetComponent<Button>().interactable = false;
            else
                dungeonsList.GetChild(i).GetComponent<Button>().interactable = true;
            
            if (currentLocation == Database.data.dungeons[i])
                dungeonsList.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }

    private void UnlockLocation(Location location)
    {
        if(!PlayerInventory.Instance.HaveEnoughGold(location.price))
        {
            ShowText("Not enough gold!", Color.red);
        }
        else
        {
            Inventory.ChangeGoldAmount(location.price);
            location.unlocked = true;
            RefreshLocationList();
        }
    }

    private void EnterDungeon(Location dungeon)
    {
        if (!PlayerInventory.Instance.HaveEnoughGold(dungeon.price))
        {
            ShowText("Not enough gold!", Color.red);
        }
        else
        {
            Inventory.ChangeGoldAmount(dungeon.price);
            ChangeLocation(dungeon);
        }
    }

    private void ChangeLocation(Location newLocation)
    {
        previousLocation = currentLocation;
        currentLocation = newLocation;
        locationName.text = currentLocation.locationName;
        adventurePanel.SetActive(false);
        dungeonEnemy.text = "";

        if (newLocation.isDungeon)
        {
            dungeonEnemyCount = 0;
            newLocation.bossDefeated = false;
        }
            
        NextEnemy();
    }

    public void NextEnemy()
    {
        if (currentLocation.isDungeon)
        {
            dungeonEnemyCount++;

            if (dungeonEnemyCount <= 10)
            {
                Enemy.SetUp(currentLocation.enemies[UnityEngine.Random.Range(0, currentLocation.enemies.Length)]);
                dungeonEnemy.text = $"{dungeonEnemyCount}/10";
            }
            else if (!currentLocation.bossDefeated)
            {
                Enemy.SetUp(currentLocation.boss);
                dungeonEnemy.text = $"BOSS";
            }           
            else
                ChangeLocation(previousLocation);
        }
        else
        {   
            if(currentLocation.bossDefeated)
                Enemy.SetUp(currentLocation.enemies[UnityEngine.Random.Range(0, currentLocation.enemies.Length)]);
            else
                Enemy.SetUp(currentLocation.boss);
        }
    }

    public void PlayerDeath()
    {
        if (currentLocation.isDungeon)
            ChangeLocation(previousLocation);
        else
            NextEnemy();
    }

    public void ShowTower()
    {
        towerPanel.SetActive(!towerPanel.activeSelf);
        bool active = towerPanel.activeSelf;
        towerPanel.SetActive(active);

        GameObject towerEnemy = towerPanel.transform.GetChild(0).gameObject;
        //EnemyData enemy = Database.Instance.TowerEnemies[Player.Instance.data.currentTowerLevel];
        //towerEnemy.GetComponent<EnemyInfo>().SetUp(enemy);
        //towerEnemy.transform.Find("FightButton").GetComponent<Button>().onClick.RemoveAllListeners();
        //towerEnemy.transform.Find("FightButton").GetComponent<Button>().onClick.AddListener(delegate { StartBattle(enemy); });
    }

    private void OnApplicationQuit()
    {
        Data.Save(Player.Instance, PlayerInventory.Instance, PlayerEquipment.Instance.Slots);
    }
}
