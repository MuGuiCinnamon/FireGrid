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

    void Start()
    {
        GenerateMap(); // 运行时自动生成地图
        
    }

    public void GenerateMap()
    {
        ClearExistingTiles();
        TileMapManager.Instance.ClearMap(); 

        for (int y = 0; y < mapRows.Length; y++)
        {
            var row = mapRows[y];
            for (int x = 0; x < row.Length; x++)
            {
                string code = row[x].ToString();
                Vector2Int pos = new Vector2Int(x, y);
                Debug.Log($"正在生成 [{x},{y}] 类型:{code}");

                var tileGO = Instantiate(tilePrefab, transform);
                tileGO.name = $"Tile_{code}_{x}_{y}";


                var tile = tileGO.GetComponent<Tile>();
                tile.Initialize(Tile.TileType.Dirt, pos); // 初始坐标
                //tile.ApplyConfig(tileDatabase.GetConfig(code));
                TileTypeConfig config = tileDatabase.GetConfig(code);
                if (config != null)
                {
                    tile.ApplyConfig(config);
                    Debug.Log($"即将注册 [{pos}] 类型:{config.code}"); // 注册前确认
                    //TileMapManager.Instance.RegisterTile(pos, tile);
                }
                else
                {
                    Debug.LogError($"无配置: {code}");
                }

                // 🔥 关键修改：立即注册到TileMapManager
                //TileMapManager.Instance.RegisterTile(pos, tile);
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (mapRows == null || mapRows.Length == 0) return;

        float cellSize = 0.96f; // 与 Initialize() 中的间距一致
        int width = mapRows[0].Length;
        int height = mapRows.Length;

        // 设置网格颜色
        Gizmos.color = Color.gray;

        // 绘制垂直线（列）
        for (int x = 0; x <= width; x++)
        {
            Vector3 start = new Vector3(x * cellSize, 0, 0);
            Vector3 end = new Vector3(x * cellSize, -height * cellSize, 0);
            Gizmos.DrawLine(start, end);
        }

        // 绘制水平线（行）
        for (int y = 0; y <= height; y++)
        {
            Vector3 start = new Vector3(0, -y * cellSize, 0);
            Vector3 end = new Vector3(width * cellSize, -y * cellSize, 0);
            Gizmos.DrawLine(start, end);
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
