using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public GameObject itemsParent;
    Inventory currentInventory;
    [HideInInspector] public int selectedItemPosition;

    private void Start()
    {
        currentInventory = Inventory.instance;
        selectedItemPosition = 0;
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            if (selectedItemPosition < itemsParent.transform.childCount - 1)
            {
                selectedItemPosition++;
                UpdateHotbarUI();
            }
            else
            {
                selectedItemPosition = 0;
                UpdateHotbarUI();
            }
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            if (selectedItemPosition > 0)
            {
                selectedItemPosition--;
                UpdateHotbarUI();
            }
            else
            {
                selectedItemPosition = itemsParent.transform.childCount - 1;
                UpdateHotbarUI();
            }
        }
    }

    void UpdateHotbarUI()
    {
        for (int i = 0; i < itemsParent.transform.childCount; i++)
        {
            if (i == selectedItemPosition)
            {
                Color colour = itemsParent.transform.GetChild(i).GetChild(0).GetComponent<Image>().color;
                itemsParent.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(colour.r, colour.g, colour.b, 0);
            }
            else
            {
                Color colour = itemsParent.transform.GetChild(i).GetChild(0).GetComponent<Image>().color;
                itemsParent.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(colour.r, colour.g, colour.b, 255);
            }
        }
    }
}
