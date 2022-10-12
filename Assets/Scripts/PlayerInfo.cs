using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Player", menuName = "RPG/Character/Player")]
public class PlayerInfo : Character
{
    public int killedMonsters = 0;

    public void AddStrength()
    {
        strength.AddPoint(1);
        remainPoints--;
        damage.AddPoint(1);
    }

    public void AddVitality()
    {
        vitality.AddPoint(1);
        remainPoints--;
        maxHealth += 10;
    }

    public void AddLuck()
    {
        luck.AddPoint(1);
        remainPoints--;
        criticalChance.AddPoint(1);
    }

    public void AddAgility()
    {
        agility.AddPoint(1);
        remainPoints--;
        criticalDamage.AddPoint(1);
    }

    public void AddStamina()
    {
        stamina.AddPoint(1);
        remainPoints--;
        defense.AddPoint(1);
    }

    public bool Equip(Equipment item)
    {
        //Debug.Log("Equip");

        if (level < item.lvlRequired) return false;

        int numSlot = (int)item.equipSlot;
        Equipment prevEq = null;
        
        if (numSlot == (int)EquipmentSlot.Ring0)
        {
            //8-11 ring slots
            int emptyRingSlot = 0;
            for (int i = 8; i < 12; i++)
            {
                if (equipment[i] == null)
                {
                    emptyRingSlot = i;
                    item.equipSlot = (EquipmentSlot)i;
                    equipment[emptyRingSlot] = item;
                    break;
                }
            }
            
            //if there is not empty ring slot, shift all rings by 1 and equip selected item at slot Ring0
            if(emptyRingSlot == 0)
            {
                prevEq = equipment[11];
                for(int j = 11; j > 8; j--)
                {
                    equipment[j] = equipment[j - 1];
                    equipment[j].equipSlot++;
                }
                equipment[numSlot] = item;
            }
        }
        else
        {
            equipment[numSlot] = item;
        }

        damage.AddModifier(item.damage);
        defense.AddModifier(item.defense);
        strength.AddModifier(item.strength);
        criticalDamage.AddModifier(item.criticalDamage);
        vitality.AddModifier(item.vitality);
        criticalChance.AddModifier(item.criticalChance);
        damage.AddModifier(item.strength);
        maxHealth += item.vitality * 10;
        Initialize();

        Inventory.Instance.RemoveItem(item);

        if (prevEq != null)
        {
            Unequip(prevEq, -1);
        }
        return true;
    }

    public bool Unequip(Equipment item, int numSlot)
    {
        //Debug.Log("Unequip");

        if(!Inventory.Instance.CheckSpace()) return false;

        if (numSlot != -1) equipment[numSlot] = null;

        if ((int)item.equipSlot > (int)EquipmentSlot.Ring0)
        {           
            item.equipSlot = EquipmentSlot.Ring0;
        }

        damage.RemoveModifier(item.damage);
        defense.RemoveModifier(item.defense);
        strength.RemoveModifier(item.strength);
        criticalDamage.RemoveModifier(item.criticalDamage);
        vitality.RemoveModifier(item.vitality);
        criticalChance.RemoveModifier(item.criticalChance);
        damage.RemoveModifier(item.strength);
        maxHealth -= item.vitality * 10;
        Initialize();
        Inventory.Instance.AddItem(item);
        Player.Instance.RefreshPlayer();
        return true;
    }

    public void LoadEquipment(Equipment item, int numSlot)
    {
        equipment[numSlot] = item;

        damage.AddModifier(item.damage);
        defense.AddModifier(item.defense);
        strength.AddModifier(item.strength);
        criticalDamage.AddModifier(item.criticalDamage);
        vitality.AddModifier(item.vitality);
        criticalChance.AddModifier(item.criticalChance);
        damage.AddModifier(item.strength);
        Initialize();
    }

    public bool CheckLvlUp()
    {
        if (exp >= expToLvl)
        {
            exp -= expToLvl;
            expToLvl = level * 10 + 60;
            level++;
            remainPoints++;
            Debug.Log(characterName + ": Level Up! Current level: " + level);
            GameManager.Instance.ShowText("Level UP!", Color.green);
            return true;
        }
        return false;
    }

    public void AddExp(int amount)
    {
        //if (level == 100) return;
        exp += amount;
        CheckLvlUp();
    }
}
