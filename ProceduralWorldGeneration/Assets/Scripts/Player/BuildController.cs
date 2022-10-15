using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildController : MonoBehaviour
{
    private Item activeItem;
    Hotbar hotbar;
    [HideInInspector] public Item[] blocks; 

    public Tilemap groundTilemap;

    public float maxDistanceForEffectingBlock = 5f;
    public float castDistance = 2f;
    public LayerMask groundLayer;

    bool destroyingBlock = false;
    float blockDestroyTime = 0.5f;
    Vector3 mousePos;

    [HideInInspector] public Item[] allItems;

    private void Start()
    {
        hotbar = FindObjectOfType<Hotbar>();
        blocks = Resources.LoadAll("Blocks", typeof(Item)).Cast<Item>().ToArray();
    }

    private void FixedUpdate()
    {
        activeItem = Inventory.instance.items[hotbar.selectedItemPosition];

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            if (Vector2.Distance(transform.position, mousePos) <= maxDistanceForEffectingBlock)
                RaycastDirection();
        }
    }

    void RaycastDirection()
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.right, 0.2f, groundLayer.value);

        if (activeItem != null)
        {
            if (activeItem.type == ItemType.Pickaxe)
            {
                if (hit.collider && !destroyingBlock)
                {
                    destroyingBlock = true;
                    StartCoroutine(SpawnBlock(hit.collider.gameObject.GetComponent<Tilemap>(), hit.collider.gameObject.GetComponent<Tilemap>().LocalToCell(mousePos)));
                    StartCoroutine(DestroyBlock(hit.collider.gameObject.GetComponent<Tilemap>(), mousePos));
                }
            }
            else if (activeItem.type == ItemType.Block) 
            {
                if (!hit.collider)
                {
                    if (activeItem.type == ItemType.Block)
                        StartCoroutine(PlaceBlock(groundTilemap, mousePos, ProceduralGeneration.GetCustomTile(activeItem.ID).tile));

                    if (activeItem.stacking == 1)
                    {
                        for (int i = 0; i < Inventory.instance.items.Length; i++)
                        {
                            if (Inventory.instance.items[i] != null)
                            {
                                if (Inventory.instance.items[i].name == activeItem.name)
                                    Inventory.instance.items[i] = null;
                            }
                        }
                    }
                    else
                        activeItem.stacking--;

                    Inventory.instance.onItemChangedCallback();
                }
                
            }
        }
    }

    IEnumerator SpawnBlock(Tilemap map, Vector3Int cellPos)
    {
        Item item = ScriptableObject.CreateInstance<Item>();
        GameObject block = new GameObject();
        Tile tile = map.GetTile(cellPos) as Tile;
        CustomTile customTile = ProceduralGeneration.GetCustomTile(tile.name); //should be id? or dictionary
        Sprite sprite = map.GetSprite(cellPos);

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].name == customTile.name)
                item = blocks[i];
        }

        block.name = item.name;
        block.layer = LayerMask.NameToLayer("Item");
        block.transform.position = map.CellToWorld(cellPos) + new Vector3(map.cellSize.x / 2, map.cellSize.y / 2);
        block.transform.localScale = new Vector3(block.transform.localScale.x * 8 / 10, block.transform.localScale.y * 8 / 10, 0);
        block.AddComponent<SpriteRenderer>();
        block.GetComponent<SpriteRenderer>().sprite = sprite;
        block.AddComponent<ItemPickUp>();
        block.GetComponent<ItemPickUp>().radius = 10;
        block.GetComponent<ItemPickUp>().item = item;
        block.AddComponent<Rigidbody2D>();
        //block.GetComponent<Rigidbody2D>().isKinematic = true; kinematic doesn't allow adding forces
        block.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
        block.GetComponent<Rigidbody2D>().freezeRotation = true;
        block.AddComponent<BoxCollider2D>();
        
        yield return new WaitForSeconds(0f);
    }

    IEnumerator DestroyBlock(Tilemap map, Vector2 pos)
    {
        yield return new WaitForSeconds(blockDestroyTime);
        Vector3Int position = map.WorldToCell(pos);
        TileBase currentTile = map.GetTile(position);
        map.SetTile(position, null);
        destroyingBlock = false;
    }

    IEnumerator PlaceBlock(Tilemap map, Vector2 pos, Tile tile)
    {
        yield return new WaitForSeconds(0f);

        Vector3Int position = map.WorldToCell(pos);
        Vector3Int downOfBlock = map.WorldToCell(new Vector3(pos.x, pos.y - map.cellSize.y, 0));
        Vector3Int rightOfBlock = map.WorldToCell(new Vector3(pos.x + map.cellSize.x, pos.y, 0));
        Vector3Int upOfBlock = map.WorldToCell(new Vector3(pos.x, pos.y + map.cellSize.y, 0));
        Vector3Int leftOfBlock = map.WorldToCell(new Vector3(pos.x - map.cellSize.x, pos.y, 0));

        if (map.HasTile(downOfBlock) || map.HasTile(rightOfBlock) || map.HasTile(upOfBlock) || map.HasTile(leftOfBlock))
            map.SetTile(position, tile);
    }
}
