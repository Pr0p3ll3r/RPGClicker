using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static string[] Values = new string[]
    {
        "K","M",
    };

    private void Awake()
    {
        if (Instance == null) Instance = this;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    private Player player;
    private Enemy enemy;
    [SerializeField] private PlayerInfo playerBase;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TextMeshProUGUI goldText;
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

    private int maxPets = 8;
    private int nextPetIndex = 0;
    private Location currentLocation;

    void Start()
    {
        player = Player.Instance;
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
        currentLocation = Library.Locations[0];

        if (Data.Check())
        {
            Data.Load();
            LoadPets();
            //Player.Instance.info.Initialize();
        }
        else
        {
            PlayerInfo newPlayer = ScriptableObject.CreateInstance<PlayerInfo>();
            newPlayer.NewAsset(playerBase);
            player.info = newPlayer;
        }
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

        //ConvertNumber(plusValue, plusText);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitBox.SetActive(true);
        }
    }

    public void Reward(EnemyInfo enemy)
    {
        //add gold
        Inventory.Instance.AddGold(enemy.rewardGold);
        goldText.text = Inventory.Instance.info.gold.ToString();
       
        //add loot
        if(Inventory.Instance.CheckSpace())
        {
            int random = Random.Range(0, enemy.loot.Count);
            LootItem probableItem = enemy.loot[random];
            Item drop = Loot.Drop(probableItem, probableItem.chanceToDrop, enemy.possibleRarity);
            if (drop != null)
            {
                if (lastlootSlot == lootSlots)
                    ClearLootList();
                Debug.Log(enemy.characterName + " : " + drop.itemName);
                Inventory.Instance.AddItem(drop);
                lootList.GetChild(lastlootSlot).GetComponent<InventorySlot>().AddItem(drop);
                lastlootSlot++;
            }
        }

        //add exp
        player.AddExp(enemy.rewardExp);

        //add stats
        player.AddMonsterKill();

        if(enemy.boss)
        {
            currentLocation.bossDefeated = true;
            enemy.bossLocation.discovered = true;
        }
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

    public static void ConvertNumber(float number, TextMeshProUGUI text)
    {
        if (number < 1000)
        {
            text.text = number.ToString();
        }

        if (number >= 1000)
        {
            number /= 1000;
            text.text = number.ToString(new CultureInfo("en-US")) + Values[0];
        }

        if (number >= 10000)
        {
            number /= 10000;
            text.text = number.ToString(new CultureInfo("en-US")) + Values[0];
        }

        if (number >= 100000)
        {
            number /= 100000;
            text.text = number.ToString(new CultureInfo("en-US")) + Values[0];
        }

        if (number >= 1000000)
        {
            number /= 1000000;
            text.text = number.ToString(new CultureInfo("en-US")) + Values[1];
        }

        if (number >= 10000000)
        {
            number /= 10000000;
            text.text = number.ToString(new CultureInfo("en-US")) + Values[1];
        }

        if (number >= 100000000)
        {
            number /= 100000000;
            text.text = number.ToString(new CultureInfo("en-US")) + Values[1];
        }
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
        foreach (Location location in Library.Locations)
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

        foreach (Location boss in Library.Bosses)
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
            if (Library.Locations[i].unlocked)
                locationList.GetChild(i).GetComponent<Button>().interactable = true;
            else if (Library.Locations[i].discovered)
                locationList.GetChild(i).transform.Find("Unlock").gameObject.SetActive(true);

            if(currentLocation == Library.Locations[i])
                locationList.GetChild(i).GetComponent<Button>().interactable = false;
        }

        for (int i = 0; i < bossList.childCount; i++)
        {
            if (Library.Locations[i].unlocked)
                bossList.GetChild(i).GetComponent<Button>().interactable = true;
            else if (Library.Locations[i].discovered)
                bossList.GetChild(i).transform.Find("Unlock").gameObject.SetActive(true);

            if (currentLocation == Library.Locations[i])
                bossList.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }
    
    void UnlockLocation(Location location)
    {
        if(!Inventory.Instance.CheckGold(location.price))
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

    public EnemyInfo NextEnemy()
    {
        int randomEnemy = Random.Range(0, currentLocation.enemies.Length);
        return currentLocation.enemies[randomEnemy];
    }

    public void DisplayDropInfo(EnemyInfo enemy)
    {
        dropInfo.GetComponent<DropInfo>().SetUpInfo(enemy);
    }

    public void AddPet()
    {
        Pet newPet = ScriptableObject.CreateInstance<Pet>();
        newPet.NewAsset(pet);
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
        Inventory.Instance.UpdateUI();
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
        Data.Save(Player.Instance.info, Inventory.Instance.info, Player.Instance.myPets);
    }
}
