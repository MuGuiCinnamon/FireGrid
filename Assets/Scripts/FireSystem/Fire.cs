using UnityEngine;

public class Fire : MonoBehaviour
{
    private Tile tile;
    private int burnStep = 0;
    private Animator animator;
    private bool isDying = false;

    public void Init(Tile tileRef)
    {
        tile = tileRef;
        burnStep = 0;
        // 可以播放 Fire_Ready 动画
        animator = GetComponent<Animator>();
        //tile.AttachFire(step);
        if (animator != null)
        {
            //animator.SetInteger("Step", burnStep); // 播放 Fire_Ready
            animator.SetTrigger("Ready");
        }
    }
    void OnEnable()
    {
        PlayerController.OnStep += Step;
    }

    void OnDisable()
    {
        PlayerController.OnStep -= Step;
    }


    public void Step()
    {
        if (isDying) return;
        burnStep++;
        tile?.UpdateFireTransparency(burnStep);
        if (burnStep == 1)
        {
            animator.SetTrigger("On"); // 播放 fire_on，然后转为 fire_keep
        }
        else if (burnStep >= 6)
        {
            isDying = true;
            animator.SetTrigger("Off"); // 播放 fire_off 动画

            // 延迟执行销毁（等待 fire_off 播放完）
            StartCoroutine(DelayedDestroy());
        }
        // 🔥🔥 蔓延逻辑
        if (burnStep > 0 && burnStep % 3 == 0)
        {
            TrySpread();
        }
    }
    private void TrySpread()
    {
        if (tile == null) return;

        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var dir in directions)
        {
            Vector2Int checkPos = tile.gridPosition + dir;
            Tile neighbor = TileMapManager.Instance.GetTileAt(checkPos);

            if (neighbor != null && neighbor.isBurnable && !neighbor.hasFire)
            {
                TileFireManager.Instance.CreateFireAt(checkPos);
            }
        }
    }

    private System.Collections.IEnumerator DelayedDestroy()
    {
        // 等待动画播放完成（假设 fire_off 是 0.5 秒）
        yield return new WaitForSeconds(1.1f);

        if (tile != null)
        {
            tile.hasFire = false;
            tile.isBurnable = false;
        }

        Destroy(gameObject);
    }
}
