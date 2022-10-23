using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    private Player player;
    private Enemy enemy;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform lootList;
    [SerializeField] private Animation textAnimation;
    [SerializeField] private GameObject exitBox;

    int fingerCount;
    bool screenPressed = false;

    private int lastlootSlot;
    private int lootSlots = 6;

    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject playerPanel;
    [SerializeField] private GameObject townPanel;
    [SerializeField] private GameObject locationPanel;
    [SerializeField] private GameObject petsPanel;
    [SerializeField] private GameObject armoryPanel;
    [SerializeField] private GameObject towerPanel;

    [Header("Location")]
    [SerializeField] private GameObject locationPrefab;
    [SerializeField] private Transform locationList;
    [SerializeField] private Transform bossList;
    [SerializeField] private GameObject dropInfo;

    [Header("Pet")]
    [SerializeField] private Pet pet;
    [SerializeField] private GameObject petPrefab;

    [Header("Poolers")]
    public ObjectPooler damagePopupPooler;

    private int maxPets = 8;
    private int nextPetIndex = 0;
    private Location currentLocation;

    void Start()
    {
        player = Player.Instance;
        player.data = new PlayerData();
        enemy = Enemy.Instance;

        ClearLootList();

        mainPanel.SetActive(true);
        playerPanel.SetActive(false);
        townPanel.SetActive(false);
        statsPanel.SetActive(false);
        locationPanel.SetActive(false);
        petsPanel.SetActive(false);
        armoryPanel.SetActive(false);
        towerPanel.SetActive(false);

        CreateLocationList();
        currentLocation = Database.Locations[0];
        enemy.SetUp(currentLocation.enemies[0].GetCopy());

        Data.Load();
    }

    void Update()
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

        player.Reward(enemy.exp, enemy.gold);
    }

    void ClearLootList()
    {
        foreach (Transform lootSlot in lootList.transform)
        {
            lootSlot.GetComponent<InventorySlot>().ClearSlot();
        }
        lastlootSlot = 0;
    }

    public void ShowText(string text, Color color)
    {
        textAnimation.GetComponent<TextMeshProUGUI>().text = text;
        textAnimation.GetComponent<TextMeshProUGUI>().color = color;
        textAnimation.Stop();
        textAnimation.Play();
    }

    public void CheckFingers()
    {
        screenPressed = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
    }

    public void ShowStats()
    {
        statsPanel.SetActive(true);
    }

    public void ShowLocations()
    {
        locationPanel.SetActive(true);
        locationPanel.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void ShowArmory()
    {
        armoryPanel.SetActive(true);
    }

    void CreateLocationList()
    {
        foreach (Location location in Database.Locations)
        {
            GameObject locationGO = Instantiate(locationPrefab, locationList);
            locationGO.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = location.name;
            locationGO.transform.Find("Unlock/Price").GetComponent<TextMeshProUGUI>().text = $"{location.price}";
            locationGO.GetComponent<Button>().onClick.AddListener(delegate { ChangeLocation(location); });
            locationGO.transform.Find("Unlock").gameObject.SetActive(false);
            locationGO.transform.Find("Unlock/ButtonUnlock").GetComponent<Button>().onClick.AddListener(delegate { UnlockLocation(location); });
            locationGO.GetComponent<Button>().interactable = false;
            RefreshLocationList();
        }

        foreach (Location boss in Database.Bosses)
        {
            if (!boss.discovered) continue;

            GameObject bossGO = Instantiate(locationPrefab, bossList);
            bossGO.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = boss.name;
            bossGO.transform.Find("Unlock/Price").GetComponent<TextMeshProUGUI>().text = $"{boss.price}";
            bossGO.GetComponent<Button>().onClick.AddListener(delegate { ChangeLocation(boss); });
            bossGO.transform.Find("Unlock").gameObject.SetActive(false);
            bossGO.transform.Find("Unlock/ButtonUnlock").GetComponent<Button>().onClick.AddListener(delegate { UnlockLocation(boss); });
            bossGO.GetComponent<Button>().interactable = false;
            RefreshLocationList();
        }
    }

    void RefreshLocationList()
    {
        for (int i = 0; i < locationList.childCount; i++)
        {
            if (Database.Locations[i].unlocked)
                locationList.GetChild(i).GetComponent<Button>().interactable = true;
            else if (Database.Locations[i].discovered)
                locationList.GetChild(i).transform.Find("Unlock").gameObject.SetActive(true);

            if(currentLocation == Database.Locations[i])
                locationList.GetChild(i).GetComponent<Button>().interactable = false;
        }

        for (int i = 0; i < bossList.childCount; i++)
        {
            if (Database.Locations[i].unlocked)
                bossList.GetChild(i).GetComponent<Button>().interactable = true;
            else if (Database.Locations[i].discovered)
                bossList.GetChild(i).transform.Find("Unlock").gameObject.SetActive(true);

            if (currentLocation == Database.Locations[i])
                bossList.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }
    
    void UnlockLocation(Location location)
    {
        if(!PlayerInventory.Instance.CheckGold(location.price))
        {
            //show some notification, Not enough gold!
        }
        else
        {
            location.unlocked = true;
            RefreshLocationList();
        }
    }

    void ChangeLocation(Location newLocation)
    {
        currentLocation = newLocation;

        if (newLocation.bossDefeated)
            enemy.SetUp(NextEnemy());
        else
            enemy.SetUp(newLocation.boss);
    }

    public EnemyData NextEnemy()
    {
        int randomEnemy = Random.Range(0, currentLocation.enemies.Length);
        return currentLocation.enemies[randomEnemy];
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
            textAnimation.GetComponent<TextMeshProUGUI>().text = "You can't have more pets";
            textAnimation.Play();
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

    void LoadPets()
    {
        foreach (Pet pet in Player.Instance.myPets)
        {
            GameObject petUI = Instantiate(petPrefab, petsPanel.transform);
            petUI.GetComponent<PetInfo>().SetUp(pet);
            petUI.GetComponent<Button>().onClick.AddListener(delegate { ShowPetInfo(pet); });
            nextPetIndex++;
        }
    }

    void ShowPetInfo(Pet pet)
    {

    }

    private void OnApplicationQuit()
    {
        //Data.Save(Player.Instance.data, PlayerInventory.Instance, PlayerEquipment.Instance.slots);
    }
}
