using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [HideInInspector] public Item item;
    public Image icon;
    public Button removeButton;
    public Text stackingNumber;

    private void Start()
    {
        stackingNumber.enabled = false;
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = newItem.icon;
        icon.enabled = true;
        removeButton.interactable = true;

        if (item.type == ItemType.Block)
        {
            stackingNumber.enabled = true;
            stackingNumber.text = item.stacking.ToString();
        }
        else
            stackingNumber.enabled = false;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        stackingNumber.enabled = false;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }

    public void UseItem()
    {
        if (item != null)
            item.Use();
    }
}
