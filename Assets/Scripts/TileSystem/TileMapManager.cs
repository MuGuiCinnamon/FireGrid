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
        Debug.Log("✅ TileMapManager 已初始化");
    }

    public void RegisterTile(Vector2Int position, Tile tile)
    {
        // if (!tileMap.ContainsKey(position))
        // {
        //     tileMap.Add(position, tile);
        // }
        if (tileMap.ContainsKey(position))
        {
            Debug.LogError($"位置 {position} 已注册过瓦片！");
            return;
        }
        
        tileMap.Add(position, tile);
        Debug.Log($"注册瓦片：{position} | 类型：{tile.tileType}", tile.gameObject);
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
