using UnityEngine;

public class TileFireManager : MonoBehaviour
{
    public static TileFireManager Instance;

    [SerializeField] private GameObject firePrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CreateFireAt(Vector2Int position)
    {
        Tile tile = TileMapManager.Instance.GetTileAt(position);
        if (tile == null || tile.hasFire || !tile.isBurnable) return;

        Vector3 spawnPos = new Vector3(position.x * 0.96f, -position.y * 0.96f, 0f);
        GameObject fire = Instantiate(firePrefab, spawnPos, Quaternion.identity, this.transform);
        fire.transform.localScale = Vector3.one;

        fire.GetComponent<Fire>().Init(tile);
        tile.hasFire = true;
    }
}
