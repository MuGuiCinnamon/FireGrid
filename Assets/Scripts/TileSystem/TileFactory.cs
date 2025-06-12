using UnityEngine;

public class TileFactory : MonoBehaviour
{
    public GameObject tilePrefab; // 拖入你的统一TilePrefab
    public TileDatabase tileDatabase; // 拖入TileDatabase.asset

    [TextArea(5, 10)]
    public string[] mapRows = new string[]
    {
        "GGDDR",
        "GGDDR",
        "GDRRR",
        "DDDRR",
        "DDRRG"
    };

    [ContextMenu("🔧 生成Tile地图")]
    public void GenerateMap()
    {
        ClearExistingTiles();

        for (int y = 0; y < mapRows.Length; y++)
        {
            var row = mapRows[y];
            for (int x = 0; x < row.Length; x++)
            {
                string code = row[x].ToString();
                Vector2Int pos = new Vector2Int(x, y);

                var tileGO = Instantiate(tilePrefab, transform);
                tileGO.name = $"Tile_{code}_{x}_{y}";

                var tile = tileGO.GetComponent<Tile>();
                tile.Initialize(Tile.TileType.Dirt, pos); // 初始坐标
                tile.ApplyConfig(tileDatabase.GetConfig(code));
            }
        }
    }
    

    private void ClearExistingTiles()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
