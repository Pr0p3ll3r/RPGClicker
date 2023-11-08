using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Database.data = database;
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    [Header("Main")]
    [SerializeField] private GameObject exitPanel;
    [SerializeField] private Transform lootList;
    [SerializeField] private Animation popupText;
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
    [SerializeField] private GameObject rebirthPanel;
    [SerializeField] private GameObject gatePanel;

    [Header("Adventure")]
    [SerializeField] private GameObject locationPrefab;
    [SerializeField] private GameObject locationPanel;
    [SerializeField] private Transform locationList;
    [SerializeField] private Button locationButton;
    [SerializeField] private GameObject dungeonPanel;
    [SerializeField] private Transform dungeonsList;
    [SerializeField] private Button dungeonButton;
    [SerializeField] private TextMeshProUGUI dungeonEnemy;

    [Header("Tower")]
    [SerializeField] private TowerInfo towerInfo;
    [SerializeField] private Button fightButton;
    [SerializeField] private Location tower;

    [Header("Poolers")]
    public ObjectPooler damagePopupPooler;

    private Player Player => Player.Instance;
    private PlayerInventory Inventory => PlayerInventory.Instance;
    private Enemy Enemy => Enemy.Instance;
    private QuestManager questManager;
    private AchievementManager achievementManager;

    private int lastlootSlot;
    private readonly int lootSlots = 6;

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
        rebirthPanel.SetActive(false);
        gatePanel.SetActive(false);
        exitPanel.SetActive(false);

        CreateLocationList();
        CloseAdventurePanels();
        locationPanel.SetActive(true);
        locationButton.onClick.AddListener(delegate { CloseAdventurePanels(); locationPanel.SetActive(true); });
        dungeonButton.onClick.AddListener(delegate { CloseAdventurePanels(); dungeonPanel.SetActive(true); });
        dungeonEnemy.text = "";
        fightButton.onClick.AddListener(delegate { towerPanel.SetActive(false); ChangeLocation(tower); });

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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            exitPanel.SetActive(true);
        }
    }

    private void OnEnable()
    {
        RebirthSystem.OnRebirth += Reborn;
    }

    private void OnDisable()
    {
        RebirthSystem.OnRebirth -= Reborn;
    }

    private void StartNewGame()
    {
        if (string.IsNullOrEmpty(nameInput.text))
        {
            setNameButton.Select();
            return;
        }

        Player.Data = new PlayerData(nameInput.text, (PlayerClass)classDropdown.value);
        PlayerPrefs.SetInt("NewGame", 0);
        SetGame();
    }

    private void SetGame()
    {
        Player.GetComponent<PlayerUI>().UpdateHealthBar();
        Player.GetComponent<PlayerInfo>().RefreshStats();
        Player.GetComponent<PlayerInfo>().SetStatsDescription();
        mainPanel.SetActive(true);
        Database.data.locations[0].BossDefeated = true;
        UnlockLocation(Database.data.locations[0]);
        ChangeLocation(Database.data.locations[PlayerPrefs.GetInt("LastLocation", 0)]);
        foreach (EquipmentSlot slot in PlayerEquipment.Instance.Slots)
            slot.SetRightPlaceholder();
        if (Player.Data.completedQuests < Database.data.quests.Length)
            questManager.SetQuest(Database.data.quests[Player.Data.completedQuests]);
        else
            questManager.SetQuest(null);
        SetTower();
    }

    public void ClickHandler()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Player.Instance.Attack();
        }
    }

    private void Reborn()
    {
        foreach (Location location in Database.data.locations)
        {
            location.OnAfterDeserialize();
        }
        Database.data.locations[0].BossDefeated = true;
        UnlockLocation(Database.data.locations[0]);
        ChangeLocation(Database.data.locations[0]);
        PlayerPrefs.SetInt("QuestProgress", 0);
        questManager.SetQuest(Database.data.quests[0]);
        ClearLootList();
    }

    public void Reward(EnemyData enemy)
    {
        if (!Inventory.IsFull())
        {
            Item drop = LootTable.Drop(enemy.loot);
            if (drop != null)
            {
                if (lastlootSlot == lootSlots)
                    ClearLootList();
#if UNITY_EDITOR
                Debug.Log("Dropped: " + drop.itemName);
                if (drop.itemType == ItemType.Equipment) 
                {
                    Equipment dropEq = (Equipment)drop;
                    Debug.Log("Rarity: " + dropEq.rarity);
                    Debug.Log("Scroll Slots: " + dropEq.scrollsStat.Length);
                }
#endif    
                Inventory.AddItem(drop, 1);
                lootList.GetChild(lastlootSlot).GetComponent<InventorySlot>().FillSlot(drop);
                lastlootSlot++;
            }
        }

        if (enemy.isBoss)
        {
            currentLocation.BossDefeated = true;
            if (currentLocation.isDungeon)
                Player.Data.completedDungeons++;
        }

        if (enemy.isTowerMaster)
        {
            Player.Data.currentTowerLevel++;
            SetTower();
        }

        Player.Reward(enemy.exp, enemy.gold);
        questManager.QuestProgress(enemy);
        achievementManager.AchievementProgress(enemy);
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
            if (Player.Data.level < Database.data.locations[i].lvlMin)
            {
                locationList.GetChild(i).GetComponent<Button>().interactable = false;
                locationList.GetChild(i).transform.Find("Unlock").gameObject.SetActive(true);
                locationList.GetChild(i).transform.Find("Unlock/ButtonUnlock").GetComponent<Button>().interactable = false;
            }
            else if (Database.data.locations[i].Unlocked)
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
            if (Player.Data.level < Database.data.dungeons[i].lvlMin)
                dungeonsList.GetChild(i).GetComponent<Button>().interactable = false;
            else
                dungeonsList.GetChild(i).GetComponent<Button>().interactable = true;

            if (currentLocation == Database.data.dungeons[i])
                dungeonsList.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }

    private void UnlockLocation(Location location)
    {
        if (!PlayerInventory.Instance.HaveEnoughGold(location.price))
        {
            ShowText("Not enough gold!", Color.red);
        }
        else
        {
            Inventory.ChangeGoldAmount(-location.price);
            location.Unlocked = true;
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
            Inventory.ChangeGoldAmount(-dungeon.price);
            ChangeLocation(dungeon);
        }
    }

    public void ChangeLocation(Location newLocation)
    {
        if(currentLocation != null && !currentLocation.isDungeon && !currentLocation.isGate && !currentLocation.enemies[0].isTowerMaster)
            previousLocation = currentLocation;
        currentLocation = newLocation;
        locationName.text = currentLocation.locationName;
        adventurePanel.SetActive(false);
        dungeonEnemy.text = "";

        if (newLocation.isDungeon)
        {
            dungeonEnemyCount = 0;
            newLocation.BossDefeated = false;
        }

        if(!newLocation.isGate)
        {
            GateManager.inGate = false;
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
                Enemy.SetUp(currentLocation.enemies[Random.Range(0, currentLocation.enemies.Length)]);
                dungeonEnemy.text = $"{dungeonEnemyCount}/10";
            }
            else if (!currentLocation.BossDefeated)
            {
                Enemy.SetUp(currentLocation.boss);
                dungeonEnemy.text = $"BOSS";
            }
            else
                ChangeLocation(previousLocation);
        }
        else if (currentLocation.enemies[0].isTowerMaster)
        {
            if (Enemy.Data.isTowerMaster)
            {
                ChangeLocation(previousLocation);
            }
            else
            {
                Enemy.SetUp(currentLocation.enemies[Player.Data.currentTowerLevel]);
                dungeonEnemy.text = $"BOSS";
            }
        }
        else if (currentLocation.isGate)
        {
            Enemy.SetUp(currentLocation.enemies[0]);
        }
        else
        {
            if (currentLocation.BossDefeated)
            {
                Enemy.SetUp(currentLocation.enemies[Random.Range(0, currentLocation.enemies.Length)]);
                dungeonEnemy.text = "";
            }
            else
            {
                Enemy.SetUp(currentLocation.boss);
                dungeonEnemy.text = $"BOSS";
            }
        }
    }

    public void PlayerDeath()
    {
        if (currentLocation.isDungeon || Enemy.Data.isTowerMaster)
            ChangeLocation(previousLocation);
        else
            NextEnemy();
    }

    private void SetTower()
    {
        if (Player.Data.currentTowerLevel != Database.data.towerEnemies.Length - 1)
            towerInfo.SetUp(Database.data.towerEnemies[Player.Data.currentTowerLevel]);
        else
        {
            fightButton.interactable = false;
            towerInfo.SetUp(null);
        }
    }

    public void StopGate()
    {
        ChangeLocation(previousLocation);
    }

    private void OnApplicationQuit()
    {
        Data.Save(Player.Instance, PlayerInventory.Instance, PlayerEquipment.Instance.Slots);
        if (!currentLocation.isDungeon && !currentLocation.enemies[0].isTowerMaster)
            PlayerPrefs.SetInt("LastLocation", currentLocation.ID);
        else
            PlayerPrefs.SetInt("LastLocation", previousLocation.ID);
    }
}
