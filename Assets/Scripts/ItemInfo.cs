using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemIcon;

    [SerializeField] private GameObject equipment;
    [SerializeField] private TextMeshProUGUI costEquipment;
    [SerializeField] private TextMeshProUGUI reqLvl;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI defense;
    [SerializeField] private TextMeshProUGUI strength;
    [SerializeField] private TextMeshProUGUI vitality;
    [SerializeField] private TextMeshProUGUI criticalDamage;
    [SerializeField] private TextMeshProUGUI criticalChance;

    [SerializeField] private GameObject resource;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI costItem;

    [SerializeField] private Animation textAnimation;

    private Item item;

    public void SetUp(Equipment item)
    {
        Refresh();
        if (item == null) return;
        this.item = item;
        itemName.text = item.itemName;
        itemIcon.sprite = item.icon;
        if (item.level > 0)
            itemName.text = $"{item.itemName} +{item.level}";
        switch (item.rarity)
        {
            case EquipmentRarity.Rare:
                itemName.color = Color.yellow;
                break;
            case EquipmentRarity.Epic:
                itemName.color = Color.blue;
                break;
            case EquipmentRarity.Legendary:
                itemName.color = new Color32(255, 165, 0, 255);
                break;
            default:
                itemName.color = Color.white;
                break;
        }
        reqLvl.text = $"{item.lvlRequired}";
        if (Player.Instance.info.level < item.lvlRequired)
        {            
            reqLvl.color = Color.red;
        }      
        else
        {
            reqLvl.color = Color.green;
        }        
        damage.text = $"{item.damage}";
        defense.text = $"{item.defense}";
        strength.text = $"{item.strength}";
        vitality.text = $"{item.vitality}";
        criticalDamage.text = $"{item.criticalDamage}%";
        criticalChance.text = $"{item.criticalChance}%";
        costEquipment.text = $"{item.cost}";

        PlayerInfo info = Player.Instance.info;

        if (info.equipment[(int)item.equipSlot] != null)
        {
            switch (item.equipSlot)
            {
                case EquipmentSlot.Weapon:
                    if (item.damage > info.equipment[(int)item.equipSlot].damage)
                        damage.color = Color.green;
                    else if (item.damage < info.equipment[(int)item.equipSlot].damage)
                        damage.color = Color.red;
                    break;
                case EquipmentSlot.Helmet:
                case EquipmentSlot.Chestplate:
                case EquipmentSlot.Hands:
                case EquipmentSlot.Leggins:
                case EquipmentSlot.Feet:
                case EquipmentSlot.Shield:
                    if (item.defense > info.equipment[(int)item.equipSlot].defense)
                        defense.color = Color.green;
                    else if (item.defense < info.equipment[(int)item.equipSlot].defense)
                        defense.color = Color.red;
                    break;
                case EquipmentSlot.Necklace:
                case EquipmentSlot.Ring0:
                case EquipmentSlot.Ring1:
                case EquipmentSlot.Ring2:
                case EquipmentSlot.Ring3:
                    if (item.strength > info.equipment[(int)item.equipSlot].strength)
                        strength.color = Color.green;
                    else if (item.strength < info.equipment[(int)item.equipSlot].strength)
                        strength.color = Color.red;
                    if (item.criticalDamage > info.equipment[(int)item.equipSlot].criticalDamage)
                        criticalDamage.color = Color.green;
                    else if (item.criticalDamage < info.equipment[(int)item.equipSlot].criticalDamage)
                        criticalDamage.color = Color.red;
                    if (item.vitality > info.equipment[(int)item.equipSlot].vitality)
                        vitality.color = Color.green;
                    else if (item.vitality < info.equipment[(int)item.equipSlot].vitality)
                        vitality.color = Color.red;
                    if (item.criticalChance > info.equipment[(int)item.equipSlot].criticalChance)
                        criticalChance.color = Color.green;
                    else if (item.criticalChance < info.equipment[(int)item.equipSlot].criticalChance)
                        criticalChance.color = Color.red;
                    break;
            }
        }
        else if(info.equipment[(int)item.equipSlot] == null)
        {
            damage.color = Color.green;
            defense.color = Color.green;
            strength.color = Color.green;
            criticalDamage.color = Color.green;
            vitality.color = Color.green;
            criticalChance.color = Color.green;
        }
       
        if (item.damage == 0) damage.transform.parent.gameObject.SetActive(false);
        if (item.defense == 0) defense.transform.parent.gameObject.SetActive(false);
        if (item.strength == 0) strength.transform.parent.gameObject.SetActive(false);
        if (item.criticalDamage == 0) criticalDamage.transform.parent.gameObject.SetActive(false);
        if (item.vitality == 0) vitality.transform.parent.gameObject.SetActive(false);
        if (item.criticalChance == 0) criticalChance.transform.parent.gameObject.SetActive(false);

        equipment.SetActive(true);
        gameObject.SetActive(true);
    }

    public void SetUp(Item item)
    {
        Refresh();
        this.item = item;
        itemName.text = item.itemName;
        itemIcon.sprite = item.icon;
        costItem.text = $"{item.cost}";

        resource.SetActive(true);
        gameObject.SetActive(true);
    }

    public void Equip()
    {
        if(!Player.Instance.info.Equip((Equipment)item))
            ShowText($"Not enough level", Color.red);
        transform.parent.gameObject.SetActive(false);
    }

    public void Unequip()
    {
        Equipment eq = (Equipment)item;
        if (Player.Instance.info.Unequip(eq, (int)eq.equipSlot))
            transform.parent.gameObject.SetActive(false);
    }

    void Refresh()
    {
        equipment.SetActive(false);
        resource.SetActive(false);

        damage.transform.parent.gameObject.SetActive(true);
        defense.transform.parent.gameObject.SetActive(true);
        strength.transform.parent.gameObject.SetActive(true);
        criticalDamage.transform.parent.gameObject.SetActive(true);
        vitality.transform.parent.gameObject.SetActive(true);
        criticalChance.transform.parent.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }

    public void Sell()
    {
        int gold = item.cost * item.quantity;
        Inventory.Instance.info.gold += gold;
        Inventory.Instance.RemoveItem(item);
        ShowText($"You gain {gold} gold", Color.yellow);
        SoundManager.Instance.PlayOneShot("Shop");
        transform.parent.gameObject.SetActive(false);
    }

    public void ShowText(string text, Color color)
    {
        textAnimation.GetComponent<TextMeshProUGUI>().text = text;
        textAnimation.GetComponent<TextMeshProUGUI>().color = color;
        textAnimation.Stop();
        textAnimation.Play();
    }
}
