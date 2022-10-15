using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
	public int ID;
	new public string name = "New Item";
	public ItemType type;
	public Sprite icon = null;             
	public bool showInInventory = true;
	public int stacking = 1;
	public int maxStacking = 1;

	public static Item Create(int ID, string name, ItemType type, Sprite icon, bool showInInventory, int stacking, int maxStacking)
    {
		Item newItem = CreateInstance<Item>();
		newItem.ID = ID;
		newItem.name = name;
		newItem.type = type;
		newItem.icon = icon;
		newItem.showInInventory = showInInventory;
		newItem.stacking = stacking;
		newItem.maxStacking = maxStacking;

		return newItem;
	}

	public virtual void Use()
	{
	}

	public void RemoveFromInventory()
	{
		Inventory.instance.Remove(this);
	}
}

public enum ItemType
{
	Pickaxe,
	Axe,
	Hammer,
	Weapon,
	Block,
	Equipment,
	Accessories,
	Potions
}