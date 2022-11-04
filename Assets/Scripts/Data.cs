using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public static class Data
{
    private static string playerPath = Application.dataPath + "/player.txt";
    private static string inventoryPath = Application.dataPath + "/inventory.txt";
    private static string eqPath = Application.dataPath + "/eq.txt";

    public static void Save(PlayerData player, PlayerInventory inventory, EquipmentSlot[] eq)
    {
        string data = "";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        //Save player
        file = File.Create(playerPath);
        data = JsonUtility.ToJson(player);
        bf.Serialize(file, data);
        file.Close();

        //Save inventory data
        file = File.Create(inventoryPath);
        data = JsonUtility.ToJson(inventory.data);
        bf.Serialize(file, data);

        //Save inventory slots
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            SaveItem saveItem;
            if (inventory.slots[i].item == null)
                saveItem = new SaveItem();
            else
                saveItem = new SaveItem(inventory.slots[i].item.ID, inventory.slots[i].amount);
            data = JsonUtility.ToJson(saveItem);
            bf.Serialize(file, data);
        }
        file.Close();

        //Save equipment
        file = File.Create(eqPath);
        for (int i = 0; i < eq.Length; i++)
        {
            SaveItem saveItem;
            if (eq[i].item == null)
                saveItem = new SaveItem();
            else
                saveItem = new SaveItem(eq[i].item.ID, 0);

            data = JsonUtility.ToJson(saveItem);
            bf.Serialize(file, data);
        }
        file.Close();
    }

    public static void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(playerPath))
        {
            file = File.Open(inventoryPath, FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), Player.Instance.data);
            file.Close();
        }

        if (File.Exists(inventoryPath))
        {
            file = File.Open(inventoryPath, FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), PlayerInventory.Instance.data);
            InventorySlot[] slots = PlayerInventory.Instance.slots;
            for (int i = 0; i < slots.Length; i++)
            {
                InventorySlot slot = slots[i];
                SaveItem saveItem = new SaveItem();
                JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), saveItem);
                if (saveItem.ID == -1)
                {
                    slot.item = null;
                    slot.amount = 0;
                }
                else
                {
                    slot.item = Database.data.GetItemById(saveItem.ID).GetCopy();
                    slot.amount = saveItem.amount;
                }
            }
            file.Close();
        }

        if (File.Exists(eqPath))
        {
            file = File.Open(eqPath, FileMode.Open);
            EquipmentSlot[] equipment = PlayerEquipment.Instance.slots;
            for (int i = 0; i < equipment.Length; i++)
            {
                EquipmentSlot slot = equipment[i];
                SaveItem saveItem = new SaveItem();
                JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), saveItem);
                if (saveItem.ID == -1)
                {
                    slot.item = null;
                }
                else
                {
                    slot.item = (Equipment)Database.data.GetItemById(saveItem.ID).GetCopy();
                }
            }
            file.Close();
        }
    }
}