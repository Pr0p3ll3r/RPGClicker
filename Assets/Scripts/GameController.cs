using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using UnityEngine.UI;
using System.IO;
using System;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [SerializeField] private TextMeshProUGUI rightText;
    [SerializeField] private TextMeshProUGUI centerText;
    [SerializeField] private Animation textAnimation;

    [Header("Player")]
    [SerializeField] private PlayerData player;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button setNameButton;

    [Header("Panels")]
    [SerializeField] private GameObject newGamePanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject battlePanel;
    [SerializeField] private GameObject playerPanel;
    [SerializeField] private GameObject townPanel;
    [SerializeField] private GameObject tavernPanel;
    [SerializeField] private GameObject expeditionPanel;
    [SerializeField] private GameObject adventurePanel;
    [SerializeField] private GameObject instancePanel;
    [SerializeField] private GameObject dungeonPanel;
    [SerializeField] private GameObject workersPanel;
    [SerializeField] private GameObject armoryPanel;
    [SerializeField] private GameObject towerPanel;
    
    [Header("Adventure")]
    [SerializeField] private GameObject enemyListPrefab;
    [SerializeField] private Transform monstersList;
    [SerializeField] private Transform bossesList;
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject dropInfoPrefab;
    private GameObject currentDropInfo;

    [Header("Expedition")]
    [SerializeField] private GameObject expeditionListPrefab;
    [SerializeField] private GameObject expeditionInfo;
    [SerializeField] private Transform expeditionList;
    [SerializeField] private GameObject expeditionLoot;

    public int enemies = 0;

    private Transform canvas;

    public ObjectPooler bulletPooler;
    public ObjectPooler footstepPooler;

    void Start()
    {
        //canvas = GameObject.Find("Canvas").transform;
        //newGamePanel.SetActive(false);
        //mainPanel.SetActive(false);
        //playerPanel.SetActive(false);
        //townPanel.SetActive(false);
        //CloseCenterPanels();

        //Data.Load();

        //if (PlayerPrefs.GetInt("NewGame", 1) == 1 || !Data.Load())
        //{
        //    newGamePanel.SetActive(true);
        //    setNameButton.onClick.AddListener(delegate { StartNewGame(); });
        //}
        //else
        //{   
        //    Data.Load();
        //    LoadPlayer();
        //}           
    }

    void StartNewGame()
    {
        if (string.IsNullOrEmpty(nameInput.text)) return;

        setNameButton.interactable = false;
        nameInput.interactable = false;

        PlayerData newPlayer = new PlayerData();
        newPlayer.nickname = nameInput.text;
        Player.Instance.data = newPlayer;

        //LoadPlayer();

        PlayerPrefs.SetInt("NewGame", 0);
        PlayerPrefs.SetInt("Tower", 0);
    }

    public void EnemyKilled()
    {
        enemies--;
        if(enemies == 0)
        {

        }
    }

    void CloseCenterPanels()
    {
        battlePanel.SetActive(false);
        inventoryPanel.SetActive(false);
        statsPanel.SetActive(false);
        expeditionPanel.SetActive(false);
        adventurePanel.SetActive(false);
        dungeonPanel.SetActive(false);
        instancePanel.SetActive(false);
        workersPanel.SetActive(false);
        tavernPanel.SetActive(false);
        expeditionLoot.SetActive(false);
        armoryPanel.SetActive(false);
        towerPanel.SetActive(false);
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        bool active = inventoryPanel.activeSelf; 
        CloseCenterPanels();
        inventoryPanel.SetActive(active);
    }

    public void ShowStats()
    {    
        statsPanel.SetActive(!statsPanel.activeSelf);
        bool active = statsPanel.activeSelf;
        RefreshStats();
        CloseCenterPanels();
        statsPanel.SetActive(active);
    }

    public void RefreshStats()
    {
        PlayerInfo info = statsPanel.GetComponent<PlayerInfo>();
        info.RefreshStats();
    }

    public void ShowAdventure()
    {
        adventurePanel.SetActive(!adventurePanel.activeSelf);
        bool active = adventurePanel.activeSelf;
        buttons.SetActive(true);
        CloseCenterPanels();
        adventurePanel.SetActive(active);
    }

    public void ShowInstances()
    {
        buttons.SetActive(false);
        instancePanel.SetActive(true);
    }

    public void ShowDungeons()
    {
        buttons.SetActive(false);
        dungeonPanel.SetActive(true);
    }

    public void Return()
    {
        buttons.SetActive(true);
        dungeonPanel.SetActive(false);
        instancePanel.SetActive(false);
    }

    public void ShowArmory()
    {
        armoryPanel.SetActive(!armoryPanel.activeSelf);
        bool active = armoryPanel.activeSelf;
        CloseCenterPanels();
        armoryPanel.SetActive(active);
    }

    public void ShowTower()
    {
        towerPanel.SetActive(!towerPanel.activeSelf);
        bool active = towerPanel.activeSelf;
        CloseCenterPanels();
        towerPanel.SetActive(active);

        GameObject towerEnemy = towerPanel.transform.GetChild(0).gameObject;
        //EnemyData enemy = Database.Instance.TowerEnemies[Player.Instance.data.currentTowerLevel];
        //towerEnemy.GetComponent<EnemyInfo>().SetUp(enemy);
        //towerEnemy.transform.Find("FightButton").GetComponent<Button>().onClick.RemoveAllListeners();
        //towerEnemy.transform.Find("FightButton").GetComponent<Button>().onClick.AddListener(delegate { StartBattle(enemy); });
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        Data.Save(Player.Instance.data, PlayerInventory.Instance, PlayerEquipment.Instance.slots);
    }

    public void ShowText(string text, Color color)
    {
        textAnimation.GetComponent<TextMeshProUGUI>().text = text;
        textAnimation.GetComponent<TextMeshProUGUI>().color = color;
        textAnimation.Stop();
        textAnimation.Play();
    }
}
