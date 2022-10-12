using UnityEngine;
using System.IO;
using System;
using System.Globalization;
using System.Net;
using System.Collections.Generic;

public static class Data
{
    private static string playerPath = Application.dataPath + "/player.txt";
    private static string inventoryPath = Application.dataPath + "/inventory.txt";
    private static string petsPath = Application.dataPath + "/pets.txt";

    public static void Save(PlayerInfo player, InventoryInfo inventory, List<Pet> pets)
    {
        if (File.Exists(playerPath))
        {
            File.WriteAllText(playerPath, "");
        }

        if (File.Exists(petsPath))
        {
            File.WriteAllText(petsPath, "");
        }

        if (File.Exists(inventoryPath))
        {
            File.WriteAllText(inventoryPath, "");
        }

        //Save player
        string json = "";
        json = JsonUtility.ToJson(player, true);
        File.AppendAllText(playerPath, json + '¬');
        foreach(Equipment eq in player.equipment)
        {
            if(eq != null)
            {
                json = JsonUtility.ToJson(eq, true);
                File.AppendAllText(playerPath, json + '¬');
            }         
        }

        //Save inventory
        string jsonInv = JsonUtility.ToJson(inventory, true);
        File.AppendAllText(inventoryPath, jsonInv + '¬');
        foreach (Item item in inventory.items)
        {
            jsonInv = JsonUtility.ToJson(item, true);
            File.AppendAllText(inventoryPath, jsonInv + '¬');
        }

        //Save pets
        foreach (Pet w in pets)
        {
            json = JsonUtility.ToJson(w, true);
            File.AppendAllText(petsPath, json + '¬');
        }
    }

    public static void Load()
    {  
        string fileContent = File.ReadAllText(playerPath);
        char[] separator = { '¬' };
        string[] content = fileContent.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        string contentJson;

        //Load player
        contentJson = content[0];
        PlayerInfo newPlayer = ScriptableObject.CreateInstance<PlayerInfo>();
        JsonUtility.FromJsonOverwrite(contentJson, newPlayer);
        Player.Instance.info = newPlayer;
        for (int i = 1; i < content.Length; i++)
        {
            contentJson = content[i];
            Equipment eq = ScriptableObject.CreateInstance<Equipment>();
            JsonUtility.FromJsonOverwrite(contentJson, eq);
            eq.icon = Library.LoadImage(eq.itemName);
            Player.Instance.info.LoadEquipment(eq, (int)eq.equipSlot);
        }

        //Load inventory
        fileContent = File.ReadAllText(inventoryPath);
        content = fileContent.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        contentJson = content[0];
        InventoryInfo newInventory = ScriptableObject.CreateInstance<InventoryInfo>();
        JsonUtility.FromJsonOverwrite(contentJson, newInventory);
        newInventory.items.Clear();
        for (int i = 1; i < content.Length; i++)
        {
            contentJson = content[i];
            Item item;
            item = ScriptableObject.CreateInstance<Equipment>();
            JsonUtility.FromJsonOverwrite(contentJson, item);
            if(item.itemType == ItemType.Item)
            {
                item = ScriptableObject.CreateInstance<Item>();
                JsonUtility.FromJsonOverwrite(contentJson, item);
            }
            if (item.itemType == ItemType.Blueprint)
            {
                item = ScriptableObject.CreateInstance<Blueprint>();
                JsonUtility.FromJsonOverwrite(contentJson, item);
                Blueprint blueprint = (Blueprint)item;
                Blueprint blueprintTemp = Library.LoadBlueprint((Blueprint)item);
                blueprint.resultItem = blueprintTemp.resultItem;
                blueprint.neededItems = blueprintTemp.neededItems;
            }
            item.icon = Library.LoadImage(item.itemName);
            item.name = item.itemName;
            newInventory.items.Add(item);
        }
        Inventory.Instance.info = newInventory;

        //Load pets
        if(File.Exists(petsPath))
        {
            List<Pet> pets = new List<Pet>();
            fileContent = File.ReadAllText(petsPath);
            content = fileContent.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < content.Length; i++)
            {
                contentJson = content[i];
                Pet pet = ScriptableObject.CreateInstance<Pet>();
                JsonUtility.FromJsonOverwrite(contentJson, pet);
                pets.Add(pet);
            }
            Player.Instance.myPets = pets;
        }     
    }

    public static bool Check()
    {
        if (File.Exists(playerPath)) return true;
        else return false;
    }

    public static void DeleteSave()
    {
        File.Delete(playerPath);
        File.Delete(inventoryPath);
        File.Delete(petsPath);
    }
}
