using UnityEngine;

public class TileFireManager : MonoBehaviour
{
    public static TileFireManager Instance;
    public GameObject firePrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void CreateFireAt(Vector2Int gridPosition)
    {
        Tile tile = TileMapManager.Instance.GetTileAt(gridPosition);
        if (tile == null || !tile.isBurnable || tile.hasFire) return;

        Vector3 worldPos = new Vector3(
            gridPosition.x * TileFactory.GridSize,
            -gridPosition.y * TileFactory.GridSize,
            0
        );

        GameObject fire = Instantiate(firePrefab, worldPos, Quaternion.identity);
        fire.GetComponent<Fire>().Init(tile);
        tile.hasFire = true;
    }
}