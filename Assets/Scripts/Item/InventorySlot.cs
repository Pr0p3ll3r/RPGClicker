using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image slot;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantity;

    public Item item;
    public int amount;

    public void FillSlot(Item _item, int amount = 1)
    {
        item = _item;
        quantity.text = amount.ToString();
        icon.sprite = Database.data.items[item.ID].icon;
        icon.enabled = true;
        if(item.itemType == ItemType.Equipment)
        {
            Equipment eq = (Equipment)item;
            slot.color = Utils.SetColor(eq.rarity);
        }
        else
            slot.color = Color.white;

        if (amount <= 1) quantity.gameObject.SetActive(false);
        else quantity.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        item = null;
        quantity.text = "";
        icon.sprite = null;
        icon.enabled = false;
        slot.color = Color.white;
    }
}
