using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment Instance { get; private set; }

    public EquipmentSlot[] Slots { get; private set; }
    [SerializeField] private Transform equipmentParent;
    [SerializeField] private PlayerUI hud;

    void Awake()
    {
        Instance = this;
        Slots = equipmentParent.GetComponentsInChildren<EquipmentSlot>();
    }

    private void Start()
    {       
        RefreshUI();   
    }

    public void EquipItem(Equipment item, out Equipment previousItem)
    {
        //if there is an empty slot
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].equipmentType == item.equipmentTypeSlot && Slots[i].item == null)
            {
                previousItem = null;
                Slots[i].item = item;
                Debug.Log("Equipped Item: " + item.itemName);
                return;
            }
        }
        //if not swap with first one
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].equipmentType == item.equipmentTypeSlot)
            {
                previousItem = Slots[i].item;
                Slots[i].item = item;
                Debug.Log("Equipped Item: " + item.itemName);
                return;
            }
        }
        previousItem = null;
    }

    public void UnequipItem(Item item)
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].item == item)
            {
                Slots[i].item = null;
                Debug.Log("Unequipped Item: " + item.itemName);
            }
        }
    }

    public void RefreshUI()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].item == null)
                Slots[i].ClearSlot();
            else
                Slots[i].FillSlot(Slots[i].item);
        }
    }
}
