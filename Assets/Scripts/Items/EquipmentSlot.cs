using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Sprite placeholderIcon;
    [SerializeField] private Sprite placeholderSwordIcon;
    [SerializeField] private Sprite placeholderShieldIcon;
    [SerializeField] private Sprite placeholderBigSwordIcon;
    [SerializeField] private Sprite placeholderWandIcon;
    [SerializeField] private Sprite placeholderBowIcon;
    [SerializeField] private Sprite placeholderBlockedIcon;

    public Equipment item;
    public EquipmentType equipmentType;  

    public void FillSlot(Equipment _item)
    {
        item = _item;
        icon.enabled = true;   
        icon.raycastTarget = true;
        icon.sprite = Database.data.items[_item.ID].icon;                 
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = placeholderIcon;
        icon.raycastTarget = false;
    }

    public void SetRightPlaceholder()
    {
        if (equipmentType == EquipmentType.MainHand)
        {
            switch (Player.Instance.data.playerClass)
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
            switch (Player.Instance.data.playerClass)
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
