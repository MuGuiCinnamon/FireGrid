using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType { Grass, Dirt, Rock }

    public TileType tileType;
    public Vector2Int gridPosition;
    public bool isWalkable = true;
    public bool isBurnable = true;
    public bool hasFire = false;

    private void Awake()
    {
        // 确保Tile在网格上对齐
        SnapToGrid();
    }

    public void Initialize(Vector2Int gridPos)
    {
        gridPosition = gridPos;
        SnapToGrid();
    }
    // 在 Tile.cs 中添加/修改这个方法
    public void ApplyConfig(TileTypeConfig config)
    {
        if (config == null) return;
        
        // 根据配置设置属性
        tileType = config.code switch
        {
            "G" => TileType.Grass,
            "D" => TileType.Dirt,
            "R" => TileType.Rock,
            _ => TileType.Dirt
        };
        
        isWalkable = config.isWalkable;
        isBurnable = config.isFlammable;
        
        // 这里可以添加其他配置应用逻辑
    }

    private void SnapToGrid()
    {
        transform.position = new Vector3(
            gridPosition.x * TileFactory.GridSize,
            -gridPosition.y * TileFactory.GridSize,
            0
        );
    }

    public void OnPlayerInteract()
    {
        if (isBurnable && !hasFire)
        {
            TileFireManager.Instance.CreateFireAt(gridPosition);
            hasFire = true;
        }
    }
}