using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	#region Singleton

	public static Inventory instance;
	public static Hotbar instanceHotbar;

	void Awake()
	{
		instance = this;
	}
	#endregion

	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;
	public int space = 10;
	public Item[] items;

    private void Start()
    {
		items = new Item[space];
    }

    public void Add(Item item)
	{
		if (item.showInInventory)
		{
			if (items.GetUpperBound(0) >= space)
			{
				Debug.Log("Not enough room.");
				return;
			}
			else
			{
                for (int i = 0; i < items.Length; i++)
                {
					if (items[i] != null)
					{
						if (items[i].ID == item.ID && items[i].stacking < items[i].maxStacking)
						{
							items[i].stacking++;
							break;
						}
					}
					else if (items[i] == null)
                    {
						items[i] = ScriptableObject.CreateInstance<Item>();
						items[i].ID = item.ID;
						items[i].name = item.name;
						items[i].icon = item.icon;
						items[i].type = item.type;
						items[i].showInInventory = item.showInInventory;
						items[i].stacking = item.stacking;
						items[i].maxStacking = item.maxStacking;
						break;
                    }
                }
			
				if (onItemChangedCallback != null)
					onItemChangedCallback.Invoke();
			}
		}
	}

	public void Remove(Item item)
	{
        for (int i = 0; i < items.GetUpperBound(0); i++)
        {
			if (items[i] == item)
			{
				items[i] = null;
				break;
			}
        }

		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
	}
}