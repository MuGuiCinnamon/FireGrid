using UnityEngine;
using System.Collections.Generic; // 添加这行

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
        tile.AttachFire(PlayerController.Instance.StepCount);


        tile.hasFire = true;
        // ✅ 先找出要被烧毁的敌人
        List<EnemyController> toRemove = new List<EnemyController>();
        foreach (var enemy in EnemyManager.Instance.GetAllEnemies())
        {
            if (enemy.gridPosition == position)
            {
                toRemove.Add(enemy);
            }
        }

        // ✅ 统一移除
        foreach (var enemy in toRemove)
        {
            Debug.Log("🔥 敌人被火焰烧毁（火焰蔓延过来）！");
            EnemyManager.Instance.UnregisterEnemy(enemy);
            Destroy(enemy.gameObject);
        }

    }
}
