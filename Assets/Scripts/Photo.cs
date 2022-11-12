using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[SerializeField]
public class Photo
{
    public readonly Dictionary<TilemapLayer.Type, TileBase[]> tilesByType = new Dictionary<TilemapLayer.Type, TileBase[]>();
    public bool hasCollider; // not working 
    public bool hasRididbody; // not working

    public int width, height;

    public Photo(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    internal void SetTiles(TilemapLayer.Type type, TileBase[] tiles)
    {
        int len = tiles.Length;
        if (tilesByType.ContainsKey(type) == false || tilesByType[type] == null)
            tilesByType[type] = new TileBase[len];

        for (int i = 0; i < len; i++)
        {
            tilesByType[type][i] = tiles[i];
        }
    }
}
