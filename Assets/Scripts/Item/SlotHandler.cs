using UnityEngine;
using UnityEngine.EventSystems;

public class SlotHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool inventory;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(inventory)
        {
            if(GetComponentInParent<InventorySlot>())
                ItemInfoManager.Instance.DisplayItemInfo(GetComponentInParent<InventorySlot>().item, inventory);
            else //is Enemy Item
                ItemInfoManager.Instance.DisplayItemInfo(GetComponentInParent<EquipmentSlot>().item, inventory, true);
        }         
        else
            ItemInfoManager.Instance.DisplayItemInfo(GetComponentInParent<EquipmentSlot>().item, inventory);

        SoundManager.Instance.PlayOneShot("Click");
    }
}

