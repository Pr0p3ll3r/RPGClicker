using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hp;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI defense;
    [SerializeField] private TextMeshProUGUI rarity;
    [SerializeField] private GameObject lootItemPrefab;
    [SerializeField] private Transform itemList;

    public void SetUpInfo(EnemyInfo enemy)
    {
        hp.text = enemy.maxHealth.ToString();
        damage.text = enemy.damage.GetValue().ToString();
        defense.text = enemy.defense.GetValue().ToString();
        switch (enemy.possibleRarity)
        {
            case 0:
                rarity.text = "Common";
                break;
            case 1:
                rarity.text = "Common/Rare";
                break;
            case 2:
                rarity.text = "Common/Rare/Epic";
                break;
        }

        bool nextBlueprint = false;
        foreach (LootItem lootItem in enemy.loot)
        {
            if (lootItem.item.itemType == ItemType.Blueprint && nextBlueprint) continue;
            if (lootItem.item.itemType == ItemType.Blueprint) nextBlueprint = true;
            GameObject item = Instantiate(lootItemPrefab, itemList); 
            item.GetComponent<InventorySlot>().AddItem(lootItem.item);
        }
    }
}
