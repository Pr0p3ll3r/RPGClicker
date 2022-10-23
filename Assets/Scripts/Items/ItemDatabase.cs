using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = "RPG/Database")]
public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public Item[] items;

    public void OnBeforeSerialize()
    {     
    }

    public void OnAfterDeserialize()
    {     
        for (int i = 0; i < items.Length; i++)
        {
            items[i].ID = i;
        }
    }

    public Item GetItemById(int _id)
    {
        if (_id >= items.Length)
            return null;

        return items[_id];
    }
}
