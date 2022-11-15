using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Armory : MonoBehaviour
{
    [Header("Upgrading")]
    [SerializeField] private GameObject upgradingPanel;
    [SerializeField] private GameObject currentItemInfo;
    [SerializeField] private GameObject afterUpgradeItemInfo;
    [SerializeField] private GameObject upgradeButtons;
    [SerializeField] private Button normalUpgradeButton;
    [SerializeField] private Button extremeUpgradeButton;
    [SerializeField] private Button divineUpgradeButton;
    [SerializeField] private Button chaosUpgradeButton;
    [SerializeField] private GameObject upgradeOptions;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeChance;
    [SerializeField] private int[] normalUpgradeChances = new int[20];
    [SerializeField] private int[] extremeUpgradeChances = new int[6];
    [SerializeField] private int[] divineUpgradeChances = new int[15];
    [SerializeField] private int[] chaosUpgradeChances = new int[15];

    [Header("Enhancing")]
    [SerializeField] private GameObject enhancingPanel;
    [SerializeField] private GameObject chooseScrollPanel;
    [SerializeField] private Transform listOfScrolls;
    [SerializeField] private GameObject scrollPrefab;
    [SerializeField] private GameObject noScrollsText;
    [SerializeField] private GameObject enhanceOptions;
    [SerializeField] private Button enhanceButton;
    [SerializeField] private TextMeshProUGUI enhanceChance;

    [Header("Crafting")]
    [SerializeField] private GameObject craftingPanel;
    [SerializeField] private Button craftButton;
    [SerializeField] private TextMeshProUGUI craftChanceText;
    [SerializeField] private int craftChance;

    private PlayerInventory Inventory => PlayerInventory.Instance;
    private GameManager GameManager => GameManager.Instance;

    private void Start()
    {
        currentItemInfo.SetActive(false);
        afterUpgradeItemInfo.SetActive(false);
        upgradingPanel.SetActive(false);
        enhancingPanel.SetActive(false);
        craftingPanel.SetActive(false);
    }

    public void OpenCraftingPanel(Blueprint b)
    {
        SoundManager.Instance.PlayOneShot("Click");
        craftingPanel.SetActive(true);
        currentItemInfo.GetComponent<ItemInfo>().SetUp(b, false, null);
        afterUpgradeItemInfo.GetComponent<ItemInfo>().SetUpCraft(b.resultItem);
        currentItemInfo.SetActive(true);
        afterUpgradeItemInfo.SetActive(true);
        craftChanceText.text = $"{craftChance}%";
        craftButton.interactable = Inventory.HaveAllMaterials(b) && !Inventory.IsFull();
        craftButton.onClick.RemoveAllListeners();
        craftButton.onClick.AddListener(delegate { Craft(b, craftChance); });
    }

    public void Craft(Blueprint b, int chance)
    {     
        Inventory.RemoveMaterials(b);
        Inventory.RemoveItem(b, 1);
        if (Chance(chance))
        {
            Item resultItem = b.resultItem.GetCopy();
            GameManager.ShowText("Success", Color.green);
            Inventory.AddItem(resultItem, 1);
            SoundManager.Instance.PlayOneShot("Success");
        }
        else
        {
            GameManager.ShowText("Failed", Color.red);
            SoundManager.Instance.PlayOneShot("Failed");
        }
        craftingPanel.SetActive(false);
        currentItemInfo.SetActive(false);
        afterUpgradeItemInfo.SetActive(false);
        Inventory.CloseItemsInfo();
    }

    public void OpenUpgradePanel(Equipment eq)
    {
        SoundManager.Instance.PlayOneShot("Click");
        normalUpgradeButton.interactable = eq.CanStillBeUpgraded();
        extremeUpgradeButton.interactable = eq.CanStillBeExtremeUpgraded();
        divineUpgradeButton.interactable = eq.CanStillBeDivineUpgraded();
        chaosUpgradeButton.interactable = eq.CanStillBeChaosUpgraded();
        upgradingPanel.SetActive(true);
        currentItemInfo.SetActive(true);
        upgradeButtons.SetActive(true);
        upgradeOptions.SetActive(false);
        currentItemInfo.GetComponent<ItemInfo>().SetUp(eq, true, null);

        normalUpgradeButton.onClick.RemoveAllListeners();
        extremeUpgradeButton.onClick.RemoveAllListeners();
        divineUpgradeButton.onClick.RemoveAllListeners();
        chaosUpgradeButton.onClick.RemoveAllListeners();
        enhanceButton.onClick.RemoveAllListeners();

        normalUpgradeButton.onClick.AddListener(delegate { OpenUpgrade(eq, 0); });
        extremeUpgradeButton.onClick.AddListener(delegate { OpenUpgrade(eq, 1); });
        divineUpgradeButton.onClick.AddListener(delegate { OpenUpgrade(eq, 2); });
        chaosUpgradeButton.onClick.AddListener(delegate { OpenUpgrade(eq, 3); });
    }

    /// <summary>
    /// UpgradeMode: 0-normal, 1-extreme, 2-divine, 3-chaos
    /// </summary>
    /// <param name="eq"></param>
    /// <param name="grade"></param>
    /// <param name="upgradeMode"></param>
    private void OpenUpgrade(Equipment eq, int upgradeMode)
    {
        SoundManager.Instance.PlayOneShot("Click");
        upgradeButtons.SetActive(false);
        afterUpgradeItemInfo.SetActive(true);
        upgradeOptions.SetActive(true);
        Equipment eqAfter = (Equipment)eq.GetCopy();
        upgradeButton.onClick.RemoveAllListeners();
        int chance = 0;
        switch (upgradeMode)
        {
            case 0:
                chance = normalUpgradeChances[eq.normalGrade.level];
                eqAfter.normalGrade.level++;
                break;
            case 1:
                chance = extremeUpgradeChances[eq.extremeGrade.level];
                eqAfter.extremeGrade.level++;
                break;
            case 2:
                chance = divineUpgradeChances[eq.divineGrade.level];
                eqAfter.divineGrade.level++;
                break;
            case 3:
                chance = chaosUpgradeChances[eq.chaosGrade.level];
                eqAfter.chaosGrade.level++;
                break;
        }
        afterUpgradeItemInfo.GetComponent<ItemInfo>().SetUp(eqAfter, true, eq);
        Destroy(eqAfter);
        upgradeChance.text = $"{chance}%";
        upgradeButton.onClick.AddListener(delegate {Upgrade(eq, chance, upgradeMode); });
    }

    private void Upgrade(Equipment eq, int chance, int upgradeMode)
    {
        if (Chance(chance))
        {
            GameManager.ShowText("Success", Color.green);
            switch (upgradeMode)
            {
                case 0:
                    eq.normalGrade.level++;
                    break;
                case 1:
                    eq.extremeGrade.level++;
                    break;
                case 2:
                    eq.divineGrade.level++;
                    break;
                case 3:
                    eq.chaosGrade.level++;
                    break;
            }
            Inventory.DisplayItemInfo(eq, true);
            SoundManager.Instance.PlayOneShot("Success");
        }
        else
        {
            GameManager.ShowText("Failed", Color.red);
            SoundManager.Instance.PlayOneShot("Failed");
        }
        OpenUpgradePanel(eq);
    }

    public void OpenEnhancePanel(Item item)
    {
        SoundManager.Instance.PlayOneShot("Click");

        foreach (Transform child in listOfScrolls.transform)
            Destroy(child.gameObject);

        int i = 0;
        foreach (InventorySlot slot in Inventory.slots)
        {
            if (slot.item == null) continue;

            if (slot.item.itemType == ItemType.Scroll)
            {
                Scroll scroll = (Scroll)slot.item;

                if (item.itemType == ItemType.Equipment)
                {
                    Equipment eq = (Equipment)item;
                    switch (eq.equipmentTypeSlot)
                    { 
                        case EquipmentType.MainHand:
                            if (!scroll.inWeapon)
                                continue;
                            break;
                        case EquipmentType.OffHand:
                            if (!scroll.inWeapon)
                                continue;
                            break;
                        case EquipmentType.Helmet:
                            if (!scroll.inHelmet)
                                continue;
                            break;
                        case EquipmentType.Chest:
                            if (!scroll.inChestplate)
                                continue;
                            break;
                        case EquipmentType.Boots:
                            if (!scroll.inBoots)
                                continue;
                            break;
                        case EquipmentType.Gloves:
                            if (!scroll.inGloves)
                                continue;
                            break;               
                    }
                }

                GameObject scrollObject = Instantiate(scrollPrefab, listOfScrolls);
                scrollObject.GetComponent<InventorySlot>().FillSlot(slot.item);
                scrollObject.GetComponent<Button>().onClick.AddListener(delegate { OpenEnhance(item, scroll); });
                i++;
            }
        }

        if (i == 0)
            noScrollsText.SetActive(true);
        else
            noScrollsText.SetActive(false);
     
        currentItemInfo.GetComponent<ItemInfo>().SetUp(item, true, null);
        currentItemInfo.SetActive(true);
        enhanceOptions.SetActive(false);
        chooseScrollPanel.SetActive(true);
        enhancingPanel.SetActive(true);
    }

    private void OpenEnhance(Item item, Scroll scroll)
    {
        SoundManager.Instance.PlayOneShot("Click");
        enhanceOptions.SetActive(true);
        chooseScrollPanel.SetActive(false);
        afterUpgradeItemInfo.SetActive(true);    
        enhanceButton.onClick.RemoveAllListeners();
        int chance = 100;
        switch (item.itemType)
        {
            case ItemType.Equipment:
                Equipment eqAfter = (Equipment)item.GetCopy();
                eqAfter.AddScroll(scroll.scrollStat);
                afterUpgradeItemInfo.GetComponent<ItemInfo>().SetUp(eqAfter, true, (Equipment)item);
                Destroy(eqAfter);
                break;
            case ItemType.Pet:
                Pet petAfter = (Pet)item.GetCopy();
                petAfter.AddScroll(scroll.scrollStat);
                afterUpgradeItemInfo.GetComponent<ItemInfo>().SetUp(petAfter, true, null);
                Destroy(petAfter);
                break;
        }
        enhanceChance.text = $"{chance}%";
        enhanceButton.onClick.AddListener(delegate { Enchance(item, scroll, chance); });
    }

    private void Enchance(Item item, Scroll scroll, int chance)
    {
        Inventory.RemoveItem(scroll, 1);
        if (Chance(chance))
        {
            GameManager.ShowText("Success", Color.green);
            switch (item.itemType)
            {
                case ItemType.Equipment:
                    Equipment eq = (Equipment)item;
                    eq.AddScroll(scroll.scrollStat);
                    break;
                case ItemType.Pet:
                    Pet pet = (Pet)item;
                    pet.AddScroll(scroll.scrollStat);
                    break;
            }
            Inventory.DisplayItemInfo(item, true);
            SoundManager.Instance.PlayOneShot("Success");
        }
        else
        {
            GameManager.ShowText("Failed", Color.red);
            SoundManager.Instance.PlayOneShot("Failed");
        }
        OpenEnhancePanel(item);
    }

    //private void ChangeNumberOfCores()
    //{
    //    int chance = 100;
    //    int amount = 0;
    //    enhanceChance.text = $"{chance}%";

    //}

    bool Chance(int chance)
    {
        int random = Random.Range(1, 101);
        if (random <= chance)
        {
            return true;
        }
        else return false;
    }
}
