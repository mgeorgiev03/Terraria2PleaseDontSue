using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile", menuName = "Inventory/Tile")]
public class CustomTile : ScriptableObject
{
    public int ID;
    new public string name;
    public Tile tile;

    public static CustomTile CreateCustomTile(int ID, string name, Tile tile)
    {
        CustomTile customTile = CreateInstance<CustomTile>();
        customTile.ID = ID;
        customTile.name = name;
        customTile.tile = tile;

        return customTile;
    }
}
