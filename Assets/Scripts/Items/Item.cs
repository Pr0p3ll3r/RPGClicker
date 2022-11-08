using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "RPG/Items/Item")]
public class Item : ScriptableObject 
{
	public int ID;
	public string itemName;
	public Sprite icon;
	public ItemType itemType;
	[TextArea(4, 6)] public string description;
	public int maxInStack = 999;
	public bool stackable;
	public int price;

	public virtual Item GetCopy()
    {
		return Instantiate(this);
	}
}

public enum ItemType
{	
	Equipment,
	Health,
	Blueprint,
	Scroll,
	Pet,
	Default
}

[System.Serializable]
public class Loot
{
	public Item item;
	public float dropChance;
}

[System.Serializable]
public class NeedItem
{
	public Item item;
	public int amount;
}