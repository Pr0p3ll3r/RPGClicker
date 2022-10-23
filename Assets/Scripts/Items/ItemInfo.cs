using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private ItemStatInfo price;

    [SerializeField] private Transform statParent;
    [SerializeField] private GameObject statPrefab;

    public void SetUp(Item item)
    {
        itemName.text = item.itemName;
        price.SetUp("Price:", $"{item.price}");
        description.text = item.description;
        if (item.description == "")
            description.gameObject.SetActive(false);
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

            if(eq.equipmentType == EquipmentType.Weapon)
            {
                Weapon weapon = (Weapon)eq;
                ItemStatInfo type = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                type.SetUp("Type:", $"{weapon.equipmentTypeSlot}");
                ItemStatInfo damage = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                int weaponDamage = Utils.GetStat(Stats.Damage, weapon.CurrentNormalStats());
                damage.SetUp("Damage:", $"{weaponDamage}");
                ItemStatInfo range = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                range.SetUp("Range:", $"{weapon.range}");
                ItemStatInfo attackRate = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
                attackRate.SetUp("Attack Rate:", $"{60 / weapon.attackRate}");

                Weapon currentWeapon = (Weapon)PlayerEquipment.Instance.slots[(int)weapon.equipmentTypeSlot].item;
                if (currentWeapon != weapon && currentWeapon != null)
                {
                    int currentDamage = Utils.GetStat(Stats.Damage, currentWeapon.CurrentNormalStats());
                    if (currentDamage > weaponDamage)
                        damage.SetColor(Color.green);
                    else
                        damage.SetColor(Color.red);

                    if (currentWeapon.range > weapon.range)
                        range.SetColor(Color.green);
                    else
                        range.SetColor(Color.red);

                    if (currentWeapon.attackRate > weapon.attackRate)
                        attackRate.SetColor(Color.green);
                    else
                        attackRate.SetColor(Color.red);
                }
                else if(currentWeapon != weapon)
                {
                    damage.SetColor(Color.green);
                    attackRate.SetColor(Color.green);
                    range.SetColor(Color.green);
                }
            }
        }
        gameObject.SetActive(true);
    }
}
