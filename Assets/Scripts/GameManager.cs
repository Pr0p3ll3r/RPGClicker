using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    private Player Player => Player.Instance;
    private Enemy Enemy => Enemy.Instance;
    [SerializeField] private Transform lootList;
    [SerializeField] private Animation popupText;
    [SerializeField] private GameObject exitBox;
    [SerializeField] private DatabaseSO database;
    [SerializeField] private TextMeshProUGUI locationName;

    int fingerCount;
    bool screenPressed = false;

    private int lastlootSlot;
    private int lootSlots = 6;

    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject playerPanel;
    [SerializeField] private GameObject townPanel;
    [SerializeField] private GameObject adventurePanel;
    [SerializeField] private GameObject petsPanel;
    [SerializeField] private GameObject armoryPanel;
    [SerializeField] private GameObject towerPanel;

    [Header("Adventure")]
    [SerializeField] private GameObject locationPrefab;
    [SerializeField] private Transform locationList;
    [SerializeField] private Transform dungeonsList;
    [SerializeField] private GameObject dropInfo;

    [SerializeField] private GameObject locationPanel;
    [SerializeField] private GameObject dungeonPanel;
    [SerializeField] private Button locationButton;
    [SerializeField] private Button dungeonButton;

    [Header("Pet")]
    [SerializeField] private Pet pet;
    [SerializeField] private GameObject petPrefab;

    [Header("Poolers")]
    public ObjectPooler damagePopupPooler;

    private int maxPets = 2;
    private int nextPetIndex = 0;
    private Location currentLocation;
    private Location previousLocation;

    private void Start()
    {
        Player.data = new PlayerData();
        Database.data = database;

        ClearLootList();

        mainPanel.SetActive(true);
        playerPanel.SetActive(false);
        townPanel.SetActive(false);
        adventurePanel.SetActive(false);
        petsPanel.SetActive(false);
        armoryPanel.SetActive(false);
        towerPanel.SetActive(false);

        ChangeLocation(Database.data.locations[0]);
        CreateLocationList();
        CloseAdventurePanels();
        locationPanel.SetActive(true);
        locationButton.onClick.AddListener(delegate { CloseAdventurePanels(); locationPanel.SetActive(true); });
        dungeonButton.onClick.AddListener(delegate { CloseAdventurePanels(); dungeonPanel.SetActive(true); });

        Data.Load();
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

    public void Reward(EnemyData enemy)
    {  
        if(!PlayerInventory.Instance.IsFull())
        {
            Item drop = LootTable.Drop(enemy.loot);
            if (drop != null)
            {
                if (lastlootSlot == lootSlots)
                    ClearLootList();
                Debug.Log("Dropped: " + drop.itemName);
                PlayerInventory.Instance.AddItem(drop, 1);
                lootList.GetChild(lastlootSlot).GetComponent<InventorySlot>().FillSlot(drop);
                lastlootSlot++;
            }
        }

        Player.Reward(enemy.exp, enemy.gold);
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

    public void ShowArmory()
    {
        armoryPanel.SetActive(true);
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
            locationGO.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = location.name;
            locationGO.transform.Find("Unlock/Price").GetComponent<TextMeshProUGUI>().text = $"{location.price}";
            locationGO.GetComponent<Button>().onClick.AddListener(delegate { ChangeLocation(location); });
            locationGO.transform.Find("Unlock").gameObject.SetActive(false);
            locationGO.transform.Find("Unlock/ButtonUnlock").GetComponent<Button>().onClick.AddListener(delegate { UnlockLocation(location); });
            locationGO.GetComponent<Button>().interactable = false;
        }

        foreach (Location dungeon in Database.data.dungeons)
        {
            GameObject bossGO = Instantiate(locationPrefab, dungeonsList);
            bossGO.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = dungeon.name;
            bossGO.transform.Find("Unlock/Price").GetComponent<TextMeshProUGUI>().text = $"{dungeon.price}";
            bossGO.GetComponent<Button>().onClick.AddListener(delegate { EnterDungeon(dungeon); });
            bossGO.transform.Find("Unlock").gameObject.SetActive(true);
            bossGO.transform.Find("Unlock/ButtonUnlock").gameObject.SetActive(false);
        }

        RefreshLocationList();
    }

    private void RefreshLocationList()
    {
        for (int i = 0; i < locationList.childCount; i++)
        {
            if (Database.data.locations[i].unlocked)
                locationList.GetChild(i).GetComponent<Button>().interactable = true;
            else
                locationList.GetChild(i).transform.Find("Unlock").gameObject.SetActive(true);

            if(currentLocation == Database.data.locations[i])
                locationList.GetChild(i).GetComponent<Button>().interactable = false;
        }

        for (int i = 0; i < dungeonsList.childCount; i++)
        {
            if (currentLocation == Database.data.dungeons[i])
                dungeonsList.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }

    private void UnlockLocation(Location location)
    {
        if(!PlayerInventory.Instance.CheckGold(location.price))
        {
            ShowText("Not enough gold!", Color.red);
        }
        else
        {
            location.unlocked = true;
            RefreshLocationList();
        }
    }

    private void EnterDungeon(Location dungeon)
    {
        if (!PlayerInventory.Instance.CheckGold(dungeon.price))
        {
            ShowText("Not enough gold!", Color.red);
        }
        else
        {
            ChangeLocation(dungeon);
        }
    }

    private void ChangeLocation(Location newLocation)
    {
        previousLocation = currentLocation;
        currentLocation = newLocation;
        locationName.text = currentLocation.name;
        adventurePanel.SetActive(false);

        if (newLocation.bossDefeated)
            Enemy.SetUp(NextEnemy());
        else
            Enemy.SetUp(newLocation.boss);
    }

    public EnemyData NextEnemy()
    {
        int randomEnemy = Random.Range(0, currentLocation.enemies.Length);
        return currentLocation.enemies[randomEnemy];
    }

    public void PlayerDeath()
    {
        if(currentLocation.isDungeon)
            ChangeLocation(Database.data.locations[0]);
        else
            Enemy.SetUp(NextEnemy());
    }

    public void DisplayDropInfo(EnemyData enemy)
    {
        dropInfo.GetComponent<DropInfo>().SetUpInfo(enemy);
    }

    public void AddPet()
    {
        Pet newPet = ScriptableObject.CreateInstance<Pet>();
        //newPet.NewAsset(pet);
        newPet.index = nextPetIndex;
        nextPetIndex++;

        Player.Instance.myPets.Add(newPet);

        GameObject petUI = Instantiate(petPrefab, petsPanel.transform);
        petUI.GetComponent<PetInfo>().SetUp(newPet);
        petUI.GetComponent<Button>().onClick.AddListener(delegate { ShowPetInfo(newPet); });
        RefreshPetList();
    }

    public void NewPet()
    {
        int petsCount = Player.Instance.myPets.Count;
        if (petsCount == maxPets)
        {
            popupText.GetComponent<TextMeshProUGUI>().text = "You can't have more pets";
            popupText.Play();
            return;
        }
        PlayerInventory.Instance.RefreshUI();
        AddPet();
    }

    public void ShowPetList()
    {
        if (Player.Instance.myPets.Count == 0)
        {
            //ShowText("You don't have any workers", new Color32(255, 65, 52, 255));
            return;
        }

        RefreshPetList();
        petsPanel.SetActive(!petsPanel.activeSelf);
        bool active = petsPanel.activeSelf;

        petsPanel.SetActive(active);
    }

    public void RefreshPetList()
    {
        List<Pet> pets = Player.Instance.myPets;
        for (int i = 0; i < pets.Count; i++)
        {
            petsPanel.transform.GetChild(i).GetComponent<PetInfo>().SetUp(pets[i]);
        }
    }

    private void LoadPets()
    {
        foreach (Pet pet in Player.Instance.myPets)
        {
            GameObject petUI = Instantiate(petPrefab, petsPanel.transform);
            petUI.GetComponent<PetInfo>().SetUp(pet);
            petUI.GetComponent<Button>().onClick.AddListener(delegate { ShowPetInfo(pet); });
            nextPetIndex++;
        }
    }

    private void ShowPetInfo(Pet pet)
    {

    }

    private void OnApplicationQuit()
    {
        //Data.Save(Player.Instance.data, PlayerInventory.Instance, PlayerEquipment.Instance.slots);
    }
}
