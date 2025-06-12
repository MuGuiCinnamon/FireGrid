using UnityEngine;

public class TileFactory : MonoBehaviour
{
    public const float GridSize = 0.96f; // 网格大小常量
    public GameObject tilePrefab;
    public TileDatabase tileDatabase;

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
                Vector2Int gridPos = new Vector2Int(x, y);

                // 计算世界坐标（严格对齐网格）
                Vector3 worldPos = new Vector3(
                    gridPos.x * GridSize,
                    -gridPos.y * GridSize,
                    0
                );

                var tileGO = Instantiate(tilePrefab, worldPos, Quaternion.identity, transform);
                tileGO.name = $"Tile_{code}_{x}_{y}";

                var tile = tileGO.GetComponent<Tile>();
                tile.Initialize(gridPos);
                tile.ApplyConfig(tileDatabase.GetConfig(code));
            }
        }
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        for (int y = 0; y < mapRows.Length; y++) {
            for (int x = 0; x < mapRows[y].Length; x++) {
                Vector3 center = new Vector3(x * GridSize, -y * GridSize, 0);
                Gizmos.DrawWireCube(center, new Vector3(GridSize, GridSize, 0));
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