using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;
    public float radius = 1f;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= radius)
            Interact();
    }

    public void Interact()
    {
        Inventory.instance.Add(item);
        Destroy(gameObject);
    }
}
