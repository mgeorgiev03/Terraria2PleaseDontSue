using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    new private bool name = false;
    InventorySlot[] slots;

    private void Start()
    {
        Inventory.instance.onItemChangedCallback += UpdateUI;
        if (gameObject.name != "HotbarCanvas")
            name = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && name)
            inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    void UpdateUI()
    {
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (Inventory.instance.items[i] != null)
                slots[i].AddItem(Inventory.instance.items[i]);
            else
                slots[i].ClearSlot();


            if (slots[i].item != null)
            {
                if (slots[i].item.type == ItemType.Block)
                {
                    slots[i].stackingNumber.enabled = true;
                    slots[i].stackingNumber.text = slots[i].item.stacking.ToString();
                }
                else
                    slots[i].stackingNumber.enabled = false;
            }
        }

    }
}