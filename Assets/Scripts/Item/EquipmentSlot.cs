using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] private Image slot;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite placeholderIcon;
    [SerializeField] private Sprite placeholderSwordIcon;
    [SerializeField] private Sprite placeholderShieldIcon;
    [SerializeField] private Sprite placeholderBigSwordIcon;
    [SerializeField] private Sprite placeholderWandIcon;
    [SerializeField] private Sprite placeholderBowIcon;
    [SerializeField] private Sprite placeholderBlockedIcon;
    [SerializeField] private Image scrollSlot1;
    [SerializeField] private Image scrollSlot2;

    public Equipment item;
    public EquipmentType equipmentType;

    public void FillSlot(Equipment _item)
    {
        item = _item;
        icon.enabled = true;   
        icon.raycastTarget = true;
        icon.sprite = Database.data.items[item.ID].icon;
        slot.color = Utils.SetColor(item.rarity);
        scrollSlot1.transform.parent.gameObject.SetActive(false);
        scrollSlot2.transform.parent.gameObject.SetActive(false);

        if (item.scrollsStat.Length > 0)
        {
            scrollSlot1.transform.parent.gameObject.SetActive(true);
            if (item.scrollsStat[0])
                scrollSlot1.sprite = item.scrollsStat[0].statIcon;

            if (item.scrollsStat.Length > 1)
            {
                scrollSlot2.transform.parent.gameObject.SetActive(true);
                if (item.scrollsStat[1])
                    scrollSlot2.sprite = item.scrollsStat[1].statIcon;
            }
        }  
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = placeholderIcon;
        icon.raycastTarget = false;
        slot.color = Color.white;
        scrollSlot1.transform.parent.gameObject.SetActive(false);
        scrollSlot2.transform.parent.gameObject.SetActive(false);
    }

    public void SetRightPlaceholder()
    {
        if (equipmentType == EquipmentType.MainHand)
        {
            switch (Player.Instance.Data.playerClass)
            {
                case PlayerClass.Warrior:
                    placeholderIcon = placeholderBigSwordIcon;
                    break;
                case PlayerClass.Blader:
                    placeholderIcon = placeholderSwordIcon;
                    break;
                case PlayerClass.Archer:
                    placeholderIcon = placeholderBowIcon;
                    break;
                case PlayerClass.Wizard:
                    placeholderIcon = placeholderWandIcon;
                    break;
                case PlayerClass.Shielder:
                    placeholderIcon = placeholderSwordIcon;
                    break;
            }
        }
        else if (equipmentType == EquipmentType.OffHand)
        {
            switch (Player.Instance.Data.playerClass)
            {
                case PlayerClass.Warrior:
                    placeholderIcon = placeholderBlockedIcon;
                    break;
                case PlayerClass.Blader:
                    placeholderIcon = placeholderSwordIcon;
                    equipmentType = EquipmentType.MainHand;
                    break;
                case PlayerClass.Archer:
                    placeholderIcon = placeholderBlockedIcon;
                    break;
                case PlayerClass.Wizard:
                    placeholderIcon = placeholderBlockedIcon;
                    break;
                case PlayerClass.Shielder:
                    placeholderIcon = placeholderShieldIcon;
                    break;
            }
        }
        icon.sprite = placeholderIcon;
    }
}
