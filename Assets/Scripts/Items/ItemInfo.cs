using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private ItemStatInfo price;

    [SerializeField] private Transform statParent;
    [SerializeField] private GameObject statPrefab;

    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;
    [SerializeField] private Button sellButton;
    [SerializeField] private Button closeButton;

    public void SetUp(Item item, bool equipped)
    {
        Clear();
        itemName.text = item.itemName;
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
            exp.SetUp("Exp:", $"{pet.exp}/{pet.expToLvlUp}");
            foreach (StatBonus stat in pet.stats)
            {
                if (stat == null) continue;

                ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                statInfo.SetUp($"{Utils.GetNiceName(stat.stat)}:", $"{stat.values[0]}");
            }

            equipButton.onClick.AddListener(delegate { EquipPet(pet); });
            equipButton.gameObject.SetActive(true);
        }
        else if (item.itemType == ItemType.Equipment)
        {
            Equipment eq = (Equipment)item;
            ItemStatInfo rarity = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
            rarity.SetUp("Rarity Bonus:", $"{eq.rarity}");
            switch (eq.rarity)
            {
                case EquipmentRarity.Common:
                    rarity.SetColor(Color.white);
                    break;
                case EquipmentRarity.Uncommon:
                    rarity.SetColor(Color.green);
                    break;
                case EquipmentRarity.Epic:
                    rarity.SetColor(new Color(30, 115, 232, 255));
                    break;
                case EquipmentRarity.Legendary:
                    rarity.SetColor(new Color32(255, 165, 0, 255));
                    break;
            }
            ItemStatInfo reqLvl = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
            reqLvl.SetUp("Lvl Required:", $"{eq.lvlRequired}");
            if (Player.Instance.data.level < eq.lvlRequired)
            {
                reqLvl.SetColor(Color.red);
            }

            ItemStatInfo normalStats = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
            normalStats.SetUp("Normal Stats:", "");
            foreach (ItemStat itemStat in eq.normalGrade.stats)
            {
                ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                int statValue = itemStat.values[eq.normalGrade.level];
                statInfo.SetUp($"{Utils.GetNiceName(itemStat.stat)}:", $"{statValue}");
               
                Equipment currentEq = PlayerEquipment.Instance.slots[(int)eq.equipmentTypeSlot].item;
                if (currentEq != eq && currentEq != null)
                {
                    int currentStat = Utils.GetStat(itemStat.stat, currentEq.normalGrade.stats, currentEq.normalGrade.level);
                    if (currentStat > statValue)
                        statInfo.SetColor(Color.green);
                    else
                        statInfo.SetColor(Color.red);
                }
                else if (currentEq != eq)
                {
                    statInfo.SetColor(Color.green);
                }
            }

            if (eq.scrollsCanBeAdded)
            {
                ItemStatInfo scrollsStats = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                scrollsStats.SetUp("Scroll Slots:", "");
                foreach (Scroll scroll in eq.scrolls)
                {
                    if (scroll == null) continue;

                    ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                    int statValue = scroll.scrollStat.values[0];
                    statInfo.SetUp($"{Utils.GetNiceName(scroll.scrollStat.stat)}:", $"{statValue}");
                }
            }
            else
            {
                ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                statInfo.SetUp($"Cannot add scrolls", "");
            }

            if (eq.canBeExtremeUpgraded)
            {
                ItemStatInfo extremeStats = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                extremeStats.SetUp("Extreme Stats:", "");
                foreach (ItemStat itemStat in eq.extremeGrade.stats)
                {
                    ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                    int statValue = itemStat.values[eq.extremeGrade.level];
                    statInfo.SetUp($"{Utils.GetNiceName(itemStat.stat)}:", $"{statValue}");

                    Equipment currentEq = PlayerEquipment.Instance.slots[(int)eq.equipmentTypeSlot].item;
                    if (currentEq != eq && currentEq != null)
                    {
                        int currentStat = Utils.GetStat(itemStat.stat, currentEq.extremeGrade.stats, currentEq.extremeGrade.level);
                        if (currentStat > statValue)
                            statInfo.SetColor(Color.green);
                        else
                            statInfo.SetColor(Color.red);
                    }
                    else if (currentEq != eq)
                    {
                        statInfo.SetColor(Color.green);
                    }
                }
            }
            else
            {
                ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                statInfo.SetUp($"Cannot extreme upgraded", "");
            }

            if (eq.canBeDivineUpgraded)
            {
                ItemStatInfo divineStats = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                divineStats.SetUp("Divine Stats:", "");
                foreach (ItemStat itemStat in eq.divineGrade.stats)
                {
                    ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                    int statValue = itemStat.values[eq.divineGrade.level];
                    statInfo.SetUp($"{Utils.GetNiceName(itemStat.stat)}:", $"{statValue}");

                    Equipment currentEq = PlayerEquipment.Instance.slots[(int)eq.equipmentTypeSlot].item;
                    if (currentEq != eq && currentEq != null)
                    {
                        int currentStat = Utils.GetStat(itemStat.stat, currentEq.divineGrade.stats, currentEq.divineGrade.level);
                        if (currentStat > statValue)
                            statInfo.SetColor(Color.green);
                        else
                            statInfo.SetColor(Color.red);
                    }
                    else if (currentEq != eq)
                    {
                        statInfo.SetColor(Color.green);
                    }
                }
            }
            else
            {
                ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                statInfo.SetUp($"Cannot divine upgraded", "");
            }

            if (eq.canBeChaosUpgraded)
            {
                ItemStatInfo chaosStats = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                chaosStats.SetUp("Chaos Stats:", "");
                foreach (ItemStat itemStat in eq.chaosGrade.stats)
                {
                    ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                    int statValue = itemStat.values[eq.chaosGrade.level];
                    statInfo.SetUp($"{Utils.GetNiceName(itemStat.stat)}:", $"{statValue}");

                    Equipment currentEq = PlayerEquipment.Instance.slots[(int)eq.equipmentTypeSlot].item;
                    if (currentEq != eq && currentEq != null)
                    {
                        int currentStat = Utils.GetStat(itemStat.stat, currentEq.chaosGrade.stats, currentEq.chaosGrade.level);
                        if (currentStat > statValue)
                            statInfo.SetColor(Color.green);
                        else
                            statInfo.SetColor(Color.red);
                    }
                    else if (currentEq != eq)
                    {
                        statInfo.SetColor(Color.green);
                    }
                }
            }
            else
            {
                ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                statInfo.SetUp($"Cannot chaos upgraded", "");
            }

            if (!equipped)
            {
                equipButton.onClick.AddListener(delegate { EquipItem(eq); });
                equipButton.gameObject.SetActive(true);
            }
            else
            {
                unequipButton.onClick.AddListener(delegate { UnequipItem(eq); });
                unequipButton.gameObject.SetActive(true);
            }
        }
    }

    private void Clear()
    {
        equipButton.onClick.RemoveAllListeners();
        unequipButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(false);
        description.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(false);
        foreach (Transform stat in statParent)
            Destroy(stat.gameObject);
        itemIcon.sprite = null;
        itemName.text = "";
        price.SetUp("Price:", "0");
        description.text = "";
    }

    private void SellItem(Item item)
    {
        PlayerInventory.Instance.RemoveItem(item);
        SoundManager.Instance.PlayOneShot("Sell");
        PlayerInventory.Instance.AddGold(item.price);
        CloseItemInfo();
    }

    private void EquipItem(Equipment item)
    {
        Player.Instance.Equip(item);
        CloseItemInfo();
    }

    private void EquipPet(Pet pet)
    {
        Player.Instance.EquipPet(pet);
        CloseItemInfo();
    }

    private void UnequipItem(Equipment item)
    {
        Player.Instance.Unequip(item);
        CloseItemInfo();
    }

    private void CloseItemInfo()
    {
        gameObject.SetActive(false);
    }
}
