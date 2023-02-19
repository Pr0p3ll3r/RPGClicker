using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image slot;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantity;
    [SerializeField] private Image scrollSlot1;
    [SerializeField] private Image scrollSlot2;

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
            scrollSlot1.transform.parent.gameObject.SetActive(false);
            scrollSlot2.transform.parent.gameObject.SetActive(false);

            if (eq.scrollsStat.Length > 0)
            {
                scrollSlot1.transform.parent.gameObject.SetActive(true);
                if (eq.scrollsStat[0])
                    scrollSlot1.sprite = eq.scrollsStat[0].statIcon;

                if (eq.scrollsStat.Length > 1)
                {
                    scrollSlot2.transform.parent.gameObject.SetActive(true);
                    if (eq.scrollsStat[1])
                        scrollSlot2.sprite = eq.scrollsStat[1].statIcon;
                }
            }
        }
        else
        {
            slot.color = Color.white;
            scrollSlot1.transform.parent.gameObject.SetActive(false);
            scrollSlot2.transform.parent.gameObject.SetActive(false);
        }
                     
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
        scrollSlot1.transform.parent.gameObject.SetActive(false);
        scrollSlot2.transform.parent.gameObject.SetActive(false);
    }
}
