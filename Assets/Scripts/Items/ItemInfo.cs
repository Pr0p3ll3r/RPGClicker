using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.Progress;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private ItemStatInfo price;

    [SerializeField] private Transform statParent;
    [SerializeField] private GameObject statPrefab;

    [SerializeField] private Button useButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button enhanceButton;
    [SerializeField] private Button sellButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private Armory armory;

    private Equipment itemToCompare;
    private PlayerInventory Inventory => PlayerInventory.Instance;
    private Player Player => Player.Instance;

    public void SetUp(Item item, bool equipped, Equipment toCompare)
    {
        if (toCompare != null)
            itemToCompare = toCompare;

        Clear();
        itemName.text = item.itemName;
        itemName.color = Color.white;
        itemIcon.sprite = item.icon;
        price.SetUp("Price:", $"{item.price}");
        description.text = item.description;
        if (item.description != "")
            description.gameObject.SetActive(true);

        if(!equipped)
        {
            sellButton.onClick.AddListener(delegate { SellItem(item); });
            sellButton.gameObject.SetActive(true);
        }

        closeButton.onClick.AddListener(delegate { CloseItemInfo(); });

        if(item.itemType == ItemType.Pet)
        {
            Pet pet = (Pet)item;
            ItemStatInfo lvl = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
            lvl.SetUp("Level:", $"{pet.level}");
            ItemStatInfo exp = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
            if(pet.LevelMaxed())
                exp.SetUp("Exp:", $"MAXED");
            else
                exp.SetUp("Exp:", $"{pet.exp}/{Pet.BASE_REQUIRE_EXP * pet.level}");
            foreach (StatBonus stat in pet.scrollsStat)
            {
                if (stat == null) break;

                ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                statInfo.SetUp($"{Utils.GetNiceName(stat.stat)}:", $"{stat.values[2]}");
            }

            equipButton.onClick.AddListener(delegate { EquipPet(pet); });
            equipButton.gameObject.SetActive(true);

            if(pet.CanAddScroll())
            {
                enhanceButton.onClick.AddListener(delegate { armory.OpenEnhancePanel(pet); });
                enhanceButton.gameObject.SetActive(true);
            }
        }
        else if (item.itemType == ItemType.Equipment)
        {
            Equipment eq = (Equipment)item;
            price.SetUp("Price:", $"{eq.GetSellPrice()}");
            itemName.text = $"{eq.itemName} +{eq.normalGrade.level}";
            ItemStatInfo rarity = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
            rarity.SetUp("Rarity Bonus:", $"{eq.rarity}");
            switch (eq.rarity)
            {
                case EquipmentRarity.Common:
                    rarity.SetColor(Color.white);
                    break;
                case EquipmentRarity.Uncommon:
                    rarity.SetColor(Color.green);
                    itemName.color = Color.green;
                    break;
                case EquipmentRarity.Epic:
                    rarity.SetColor(new Color(30, 115, 232, 255));
                    itemName.color = new Color(30, 115, 232, 255);
                    break;
                case EquipmentRarity.Legendary:
                    rarity.SetColor(new Color32(255, 165, 0, 255));
                    itemName.color = new Color32(255, 165, 0, 255);
                    break;
            }
            ItemStatInfo reqLvl = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
            reqLvl.SetUp("Lvl Required:", $"{eq.lvlRequired}");
            if (Player.Instance.data.level < eq.lvlRequired)
            {
                reqLvl.SetColor(Color.red);
            }

            ItemStatInfo normalStats = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
            normalStats.SetUp($"Normal Stats {eq.normalGrade.level}/{eq.normalGrade.maxLevel}", "");
            foreach (ItemStat itemStat in eq.normalGrade.stats)
            {
                SetStat(itemStat, eq.normalGrade, 0);
            }

            if (eq.scrollsCanBeAdded)
            {
                ItemStatInfo scrollsStats = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                scrollsStats.SetUp($"Scroll Slots {eq.UsedScrollsSlot()}/{eq.scrollsStat.Length}", "");
                foreach (StatBonus stat in eq.scrollsStat)
                {
                    if (stat == null) break;

                    int value;
                    if (eq.is2HWeapon)
                        value = stat.values[1];
                    else
                        value = stat.values[0];
                    ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                    statInfo.SetUp($"{Utils.GetNiceName(stat.stat)}:", $"{value}");
                }
            }

            if (eq.canBeExtremeUpgraded)
            {
                ItemStatInfo extremeStats = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                extremeStats.SetUp($"Extreme Stats {eq.extremeGrade.level}/{eq.extremeGrade.maxLevel}", "");
                foreach (ItemStat itemStat in eq.extremeGrade.stats)
                {
                    SetStat(itemStat, eq.extremeGrade, 1);
                }
            }

            if (eq.canBeDivineUpgraded)
            {
                ItemStatInfo divineStats = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                divineStats.SetUp($"Divine Stats {eq.divineGrade.level}/{eq.divineGrade.maxLevel}", "");
                foreach (ItemStat itemStat in eq.divineGrade.stats)
                {
                    SetStat(itemStat, eq.divineGrade, 2);
                }
            }

            if (eq.canBeChaosUpgraded)
            {
                ItemStatInfo chaosStats = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                chaosStats.SetUp($"Chaos Stats {eq.chaosGrade.level}/{eq.chaosGrade.maxLevel}", "");
                foreach (ItemStat itemStat in eq.chaosGrade.stats)
                {
                    SetStat(itemStat, eq.chaosGrade, 3);
                }
            }

            if (!equipped)
            {
                equipButton.onClick.AddListener(delegate { EquipItem(eq); });
                equipButton.gameObject.SetActive(true);
                equipButton.interactable = Player.Instance.CanUseIt(eq);

                upgradeButton.onClick.AddListener(delegate { armory.OpenUpgradePanel(eq); });
                upgradeButton.gameObject.SetActive(true);
                upgradeButton.interactable = eq.CanStillBeUpgraded() || eq.CanStillBeExtremeUpgraded() || eq.CanStillBeDivineUpgraded() ||eq.CanStillBeChaosUpgraded();

                enhanceButton.onClick.AddListener(delegate { armory.OpenEnhancePanel(eq); });
                enhanceButton.gameObject.SetActive(true);
                enhanceButton.interactable = eq.ScrollsCanStillBeAdded();          
            }
            else
            {
                unequipButton.onClick.AddListener(delegate { UnequipItem(eq); });
                unequipButton.gameObject.SetActive(true);
            }
        }
        else if (item.itemType == ItemType.Blueprint)
        {
            Blueprint blueprint = (Blueprint)item;
            ItemStatInfo requiredItems = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
            requiredItems.SetUp("Required Items", "");
            foreach (RequiredItem requiredItem in blueprint.requiredItems)
            {
                int howManyYouHave = PlayerInventory.Instance.HaveMany(requiredItem.item);
                ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                statInfo.SetUp($"{requiredItem.item.name}:", $"{howManyYouHave}/{requiredItem.amount}", requiredItem.item);

                if (howManyYouHave >= requiredItem.amount)
                    statInfo.SetColor(Color.green);
                else
                    statInfo.SetColor(Color.red);

                useButton.onClick.AddListener(delegate { armory.OpenCraftingPanel(blueprint); });
                useButton.gameObject.SetActive(true);
            }
        }
    }

    public void SetUpCraft(Item item)
    {
        Equipment eq = (Equipment)item;
        itemIcon.sprite = item.icon;
        price.SetUp("Price:", $"{eq.GetSellPrice()}");
        itemName.text = $"{eq.itemName} +{eq.normalGrade.level}";
        ItemStatInfo rarity = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
        rarity.SetUp("Rarity Bonus:", $"RANDOM");
        ItemStatInfo reqLvl = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
        reqLvl.SetUp("Lvl Required:", $"{eq.lvlRequired}");
        if (Player.Instance.data.level < eq.lvlRequired)
        {
            reqLvl.SetColor(Color.red);
        }

        ItemStatInfo normalStats = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
        normalStats.SetUp($"Normal Stats {eq.normalGrade.level}/{eq.normalGrade.maxLevel}", "");
        foreach (ItemStat itemStat in eq.normalGrade.stats)
        {
            SetStat(itemStat, eq.normalGrade, 0);
        }

        if (eq.scrollsCanBeAdded)
        {
            ItemStatInfo scrollsStats = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
            scrollsStats.SetUp($"Scroll Slots 0-3", "");
        }

        if (eq.canBeExtremeUpgraded)
        {
            ItemStatInfo extremeStats = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
            extremeStats.SetUp($"Extreme Stats {eq.extremeGrade.level}/{eq.extremeGrade.maxLevel}", "");
        }

        if (eq.canBeDivineUpgraded)
        {
            ItemStatInfo divineStats = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
            divineStats.SetUp($"Divine Stats {eq.divineGrade.level}/{eq.divineGrade.maxLevel}", "");
        }     
    }

    private void Clear()
    {
        useButton.onClick.RemoveAllListeners();
        equipButton.onClick.RemoveAllListeners();
        unequipButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
        enhanceButton.onClick.RemoveAllListeners();
        useButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
        description.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(false);
        enhanceButton.gameObject.SetActive(false);
        equipButton.interactable = true;
        enhanceButton.interactable = true;
        upgradeButton.interactable = true;
        foreach (Transform stat in statParent)
            Destroy(stat.gameObject);
        itemIcon.sprite = null;
        itemName.text = "";
        price.SetUp("Price:", "0");
        description.text = "";
    }

    private void SellItem(Item item)
    {
        Inventory.SellItem(item);
        SoundManager.Instance.PlayOneShot("Sell");
        CloseItemInfo();
    }

    private void EquipItem(Equipment item)
    {
        Player.Equip(item);
        CloseItemInfo();
    }

    private void EquipPet(Pet pet)
    {
        Player.EquipPet(pet);
        CloseItemInfo();
    }

    private void UnequipItem(Equipment item)
    {
        Player.Unequip(item);
        CloseItemInfo();
    }

    private void CloseItemInfo()
    {
        SoundManager.Instance.PlayOneShot("Click");
        Inventory.CloseItemsInfo();
    }

    /// <summary>
    /// GradeType: 0-normal, 1-extreme, 2-divine, 3-chaos
    /// </summary>
    /// <param name="itemStat"></param>
    /// <param name="grade"></param>
    /// <param name="gradeType"></param>
    private void SetStat(ItemStat itemStat, Grade grade, int gradeType)
    {
        ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
        int statValue = itemStat.values[grade.level];
        statInfo.SetUp($"{Utils.GetNiceName(itemStat.stat)}:", $"{statValue}");

        if (itemToCompare != null)
        {
            Grade currentEqGrade = null;
            switch (gradeType)
            {
                case 0:
                    currentEqGrade = itemToCompare.normalGrade;
                    break;
                case 1:
                    currentEqGrade = itemToCompare.extremeGrade;
                    break;
                case 2:
                    currentEqGrade = itemToCompare.divineGrade;
                    break;
                case 3:
                    currentEqGrade = itemToCompare.chaosGrade;
                    break;
            }

            int currentStat = Utils.GetStat(itemStat.stat, currentEqGrade.stats, currentEqGrade.level);
            Debug.Log(currentStat + " " + statValue);
            if (currentStat > statValue)
                statInfo.SetColor(Color.red);
            else if (currentStat < statValue)
                statInfo.SetColor(Color.green);
        }
    }
}
