using UnityEngine;

public class ItemInfoManager : MonoBehaviour
{
    public static ItemInfoManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    [SerializeField] private GameObject equippedItemInfo;
    [SerializeField] private GameObject selectedItemInfo;

    private void Start()
    {
        equippedItemInfo.SetActive(false);
        selectedItemInfo.SetActive(false);
    }

    public void DisplayItemInfo(Item item, bool inventory, bool isEnemyItem = false)
    {
        PlayerEquipment equipment = PlayerEquipment.Instance;
        selectedItemInfo.SetActive(false);
        equippedItemInfo.SetActive(false);
        if (inventory)
        {
            if (item.itemType == ItemType.Equipment)
            {
                Equipment selectedEq = (Equipment)item;
                int index = Utils.GetRightSlot(selectedEq);
                Equipment equippedEq = null;
                if (index > -1)
                    equippedEq = equipment.Slots[index].item;
                selectedItemInfo.GetComponent<ItemInfo>().SetUp(selectedEq, false, equippedEq, isEnemyItem);
                selectedItemInfo.SetActive(true);
                //compare with first empty or first equipped by index
                if (equippedEq != null)
                {
                    equippedItemInfo.GetComponent<ItemInfo>().SetUp(equippedEq, true, selectedEq, isEnemyItem);
                    equippedItemInfo.SetActive(true);
                }
            }
            else
            {
                selectedItemInfo.GetComponent<ItemInfo>().SetUp(item, false, null);
                selectedItemInfo.SetActive(true);
            }
        }
        else
        {
            Equipment equippedEq = (Equipment)item;
            equippedItemInfo.GetComponent<ItemInfo>().SetUp(equippedEq, true, null);
            equippedItemInfo.SetActive(true);
        }
    }

    public void CloseItemsInfo()
    {
        equippedItemInfo.SetActive(false);
        selectedItemInfo.SetActive(false);
    }
}
