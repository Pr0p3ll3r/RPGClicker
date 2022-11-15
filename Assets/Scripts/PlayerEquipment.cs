using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment Instance { get; private set; }

    public EquipmentSlot[] slots;
    [SerializeField] private Transform equipmentParent;
    [SerializeField] private PlayerUI hud;

    void Awake()
    {
        Instance = this;
        slots = equipmentParent.GetComponentsInChildren<EquipmentSlot>();
    }

    private void Start()
    {       
        RefreshUI();   
    }

    public void EquipItem(Equipment item, out Equipment previousItem)
    {
        //if there is an empty slot
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].equipmentType == item.equipmentTypeSlot && slots[i].item == null)
            {
                previousItem = null;
                slots[i].item = item;
                Debug.Log("Equipped Item: " + item.itemName);
                return;
            }
        }
        //if not swap with first one
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].equipmentType == item.equipmentTypeSlot)
            {
                previousItem = slots[i].item;
                slots[i].item = item;
                Debug.Log("Equipped Item: " + item.itemName);
                return;
            }
        }
        previousItem = null;
    }

    public void UnequipItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                slots[i].item = null;
                Debug.Log("Unequipped Item: " + item.itemName);
            }
        }
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                slots[i].ClearSlot();
            else
                slots[i].FillSlot(slots[i].item);
        }
    }
}
