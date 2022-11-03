using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
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

        if (item.itemType == ItemType.Equipment)
        {
            Equipment eq = (Equipment)item;
            ItemStatInfo rarity = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
            rarity.SetUp("Rarity:", $"{eq.rarity}");
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

            foreach(ItemStat itemStat in eq.CurrentNormalStats())
            {
                ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                int statValue = itemStat.currentValue;
                statInfo.SetUp($"{itemStat.name}:", $"{statValue}");
               
                Equipment currentEq = PlayerEquipment.Instance.slots[(int)eq.equipmentTypeSlot].item;
                if (currentEq != eq && currentEq != null)
                {
                    int currentStat = Utils.GetStat(itemStat.stat, currentEq.CurrentNormalStats());
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
        itemName.text = "";
        price.SetUp("Price:", $"0");
        description.text = "";
    }

    private void SellItem(Item item)
    {
        PlayerInventory.Instance.RemoveItem(item);
        CloseItemInfo();
    }

    private void EquipItem(Equipment item)
    {
        Player.Instance.Equip(item, null);
        CloseItemInfo();
    }

    private void UnequipItem(Equipment item)
    {
        Player.Instance.Unequip(item, null);
        CloseItemInfo();
    }

    private void CloseItemInfo()
    {
        gameObject.SetActive(false);
    }
}
