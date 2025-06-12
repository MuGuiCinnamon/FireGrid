using UnityEngine;
using System.Collections.Generic;

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
        if (!tileMap.ContainsKey(position))
        {
            tileMap.Add(position, tile);
        }
    }

    public Tile GetTileAt(Vector2Int position)
    {
        tileMap.TryGetValue(position, out var tile);
        return tile;
    }

    public void ClearMap()
    {
        tileMap.Clear();
    }
}
