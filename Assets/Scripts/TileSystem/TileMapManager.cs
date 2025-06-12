using System.Collections.Generic;
using UnityEngine;

public class TileMapManager : MonoBehaviour
{
    public static TileMapManager Instance;
    private Dictionary<Vector2Int, Tile> tileMap = new Dictionary<Vector2Int, Tile>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterTile(Vector2Int position, Tile tile)
    {
        tileMap[position] = tile;
    }

    public Tile GetTileAt(Vector2Int position)
    {
        tileMap.TryGetValue(position, out Tile tile);
        return tile;
    }
}