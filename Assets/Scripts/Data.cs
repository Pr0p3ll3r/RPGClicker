using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

public static class Data
{
    private static readonly string savePath = Application.persistentDataPath + "/save.txt";

    public static void Save(Player player, PlayerInventory inventory, EquipmentSlot[] eq)
    {
        string data = "";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        //Save player
        file = File.Create(savePath);
        data = JsonUtility.ToJson(player.Data);
        bf.Serialize(file, data);

        //Save pets
        for (int i = 0; i < player.MyPets.Length; i++)
        {
            SavePet savePet;
            Pet pet = player.MyPets[i];
            if (pet == null)
            {
                savePet = new SavePet();
                data = JsonUtility.ToJson(savePet);
            }
            else
            {
                savePet = new SavePet(pet.ID, pet.level, pet.exp, pet.scrollsStat, pet.statsUnlocked);
                data = JsonUtility.ToJson(savePet);
            }
            bf.Serialize(file, data);
        }

        //Save inventory data
        data = JsonUtility.ToJson(inventory.data);
        bf.Serialize(file, data);

        //Save inventory slots
        for (int i = 0; i < inventory.slots.Length; i++)
        {                         
            Item item = inventory.slots[i].item;
            if (item == null)
            {
                SaveItem saveItem;
                saveItem = new SaveItem();
                data = JsonUtility.ToJson(saveItem);             
            }
            else
            {
                switch(inventory.slots[i].item.itemType)
                {
                    case ItemType.Equipment:
                        SaveEquipment saveEq;
                        Equipment equipment = (Equipment)item;
                        saveEq = new SaveEquipment(equipment.ID, equipment.rarity, equipment.rarityBonus, equipment.normalGrade.level, equipment.extremeGrade.level, equipment.divineGrade.level, equipment.chaosGrade.level, equipment.scrollsStat);
                        data = JsonUtility.ToJson(saveEq);
                        break;
                    case ItemType.Pet:
                        SavePet savePet;
                        Pet pet = (Pet)item;
                        savePet = new SavePet(pet.ID, pet.level, pet.exp, pet.scrollsStat, pet.statsUnlocked);
                        data = JsonUtility.ToJson(savePet);
                        break;
                   default:
                        SaveItem saveItem;
                        saveItem = new SaveItem(inventory.slots[i].item.ID, inventory.slots[i].amount);
                        data = JsonUtility.ToJson(saveItem);
                        break;
                }
            }                       
            bf.Serialize(file, data);
        }

        //Save equipment
        for (int i = 0; i < eq.Length; i++)
        {
            SaveEquipment saveEq;
            if (eq[i].item == null)
                saveEq = new SaveEquipment();
            else
                saveEq = new SaveEquipment(eq[i].item.ID, eq[i].item.rarity, eq[i].item.rarityBonus, eq[i].item.normalGrade.level, eq[i].item.extremeGrade.level, eq[i].item.divineGrade.level, eq[i].item.chaosGrade.level, eq[i].item.scrollsStat);
            data = JsonUtility.ToJson(saveEq);
            bf.Serialize(file, data);
        }
        
        //Save locations
        for (int i = 0; i < Database.data.locations.Length; i++)
        {
            SaveLocation saveLoc;
            Location loc = Database.data.locations[i];
            saveLoc = new SaveLocation(loc.Unlocked, loc.BossDefeated);
            data = JsonUtility.ToJson(saveLoc);
            bf.Serialize(file, data);
        }

        //Save achievements
        for (int i = 0; i < Database.data.achievements.Length; i++)
        {
            SaveAchievement saveAch;
            Achievement ach = Database.data.achievements[i];
            saveAch = new SaveAchievement(ach.CurrentAmount, ach.Earned, ach.Tier);
            data = JsonUtility.ToJson(saveAch);
            bf.Serialize(file, data);
        }

        //Save rebirth system
        for (int i = 0; i < Database.data.rebirthBonuses.Length; i++)
        {
            SaveRebirthBonus saveBonus;
            RebirthBonus bonus = Database.data.rebirthBonuses[i];
            saveBonus = new SaveRebirthBonus(bonus.CurrentLevel, bonus.CurrentValue);
            data = JsonUtility.ToJson(saveBonus);
            bf.Serialize(file, data);
        }

        file.Close();
    }

    public static void Load()
    {
        if (!File.Exists(savePath)) return;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        //Debug.Log("Loading player data...");
        file = File.Open(savePath, FileMode.Open);
        JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), Player.Instance.Data);

        //Debug.Log("Loading pets data...");
        for (int i = 0; i < Player.Instance.MyPets.Length; i++)
        {
            SavePet savePet = new SavePet();
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), savePet);
            if (savePet.ID == -1)
            {
                Player.Instance.MyPets[i] = null;
            }
            else
            {
                Pet pet = (Pet)Database.data.items[savePet.ID].GetCopy();
                pet.level = savePet.level;
                pet.exp = savePet.exp;
                pet.statsUnlocked = savePet.statsUnlocked;
                for (int j = 0; j < savePet.statsIds.Length; j++)
                {
                    if (savePet.statsIds[j] == -1)
                        break;
                    pet.scrollsStat[j] = Database.data.scrollBonuses[savePet.statsIds[j]];
                }
                Player.Instance.MyPets[i] = pet;
            }
        }

        //Debug.Log("Loading inventory data...");
        JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), PlayerInventory.Instance.data);
        InventorySlot[] slots = PlayerInventory.Instance.slots;
        for (int i = 0; i < slots.Length; i++)
        {
            InventorySlot slot = slots[i];
            SaveItem saveItem = new SaveItem();
            string data = bf.Deserialize(file).ToString();
            JsonUtility.FromJsonOverwrite(data, saveItem);

            if (saveItem.ID == -1)
            {
                slot.item = null;
                slot.amount = 0;
            }
            else
            {
                Item item = Database.data.items[saveItem.ID].GetCopy();
                switch (item.itemType)
                {
                    case ItemType.Equipment:
                        SaveEquipment saveEquipment = new SaveEquipment();
                        Equipment eq = (Equipment)Database.data.items[saveItem.ID].GetCopy();
                        slot.amount = 1;
                        JsonUtility.FromJsonOverwrite(data, saveEquipment);
                        eq.rarity = saveEquipment.rarity;
                        if (saveEquipment.rarityBonusID != -1)
                            eq.rarityBonus = Database.data.rarityBonuses[saveEquipment.rarityBonusID];
                        eq.normalGrade.level = saveEquipment.normalGradeLevel;
                        eq.extremeGrade.level = saveEquipment.extremeGradeLevel;
                        eq.divineGrade.level = saveEquipment.divineGradeLevel;
                        eq.chaosGrade.level = saveEquipment.chaosGradeLevel;
                        eq.scrollsStat = new StatBonus[saveEquipment.statsIds.Length];
                        for (int j = 0; j < saveEquipment.statsIds.Length; j++)
                        {
                            if (saveEquipment.statsIds[j] == -1)
                                break;
                            eq.scrollsStat[j] = Database.data.scrollBonuses[saveEquipment.statsIds[j]];
                        }
                        slot.item = eq;
                        break;
                    case ItemType.Pet:
                        SavePet savePet = new SavePet();
                        Pet pet = (Pet)Database.data.items[saveItem.ID].GetCopy();
                        slot.amount = 1;
                        JsonUtility.FromJsonOverwrite(data, savePet);
                        pet.level = savePet.level;
                        pet.exp = savePet.exp;
                        pet.statsUnlocked = savePet.statsUnlocked;
                        for (int j = 0; j < savePet.statsIds.Length; j++)
                        {
                            if (savePet.statsIds[j] == -1)
                                break;
                            pet.scrollsStat[j] = Database.data.scrollBonuses[savePet.statsIds[j]];
                        }
                        slot.item = pet;
                        break;
                    default:
                        slot.item = item;
                        slot.amount = saveItem.amount;
                        break;
                }
            }
        }

        //Debug.Log("Loading equipment data...");
        EquipmentSlot[] equipment = PlayerEquipment.Instance.Slots;
        for (int i = 0; i < equipment.Length; i++)
        {
            EquipmentSlot slot = equipment[i];
            SaveEquipment saveEquipment = new SaveEquipment();
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), saveEquipment);
            if (saveEquipment.ID == -1)
            {
                slot.item = null;
            }
            else
            {
                Equipment eq = (Equipment)Database.data.items[saveEquipment.ID].GetCopy();
                eq.rarity = saveEquipment.rarity;
                if (saveEquipment.rarityBonusID != -1)
                    eq.rarityBonus = Database.data.rarityBonuses[saveEquipment.rarityBonusID];
                eq.normalGrade.level = saveEquipment.normalGradeLevel;
                eq.extremeGrade.level = saveEquipment.extremeGradeLevel;
                eq.divineGrade.level = saveEquipment.divineGradeLevel;
                eq.chaosGrade.level = saveEquipment.chaosGradeLevel;
                eq.scrollsStat = new StatBonus[saveEquipment.statsIds.Length];
                for (int j = 0; j < saveEquipment.statsIds.Length; j++)
                {
                    if (saveEquipment.statsIds[j] == -1)
                        break;
                    eq.scrollsStat[j] = Database.data.scrollBonuses[saveEquipment.statsIds[j]];
                }
                slot.item = eq;
            }
        }

        //Debug.Log("Loading locations...");
        for (int i = 0; i < Database.data.locations.Length; i++)
        {
            SaveLocation saveLoc = new SaveLocation();
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), saveLoc);
            Database.data.locations[i].Unlocked = saveLoc.unlocked;
            Database.data.locations[i].BossDefeated = saveLoc.bossDefeated;
        }

        //Debug.Log("Loading achievements...");
        for (int i = 0; i < Database.data.achievements.Length; i++)
        {
            SaveAchievement saveAch = new SaveAchievement();
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), saveAch);
            Database.data.achievements[i].Load(saveAch.amount, saveAch.earned, saveAch.tier);
        }

        //Debug.Log("Loading rebirth system...");
        for (int i = 0; i < Database.data.rebirthBonuses.Length; i++)
        {
            SaveRebirthBonus saveBonus = new SaveRebirthBonus();
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), saveBonus);
            Database.data.rebirthBonuses[i].CurrentLevel = saveBonus.level;
            Database.data.rebirthBonuses[i].CurrentValue = saveBonus.value;
        }

        file.Close();
    }
}