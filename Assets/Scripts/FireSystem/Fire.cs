using UnityEngine;

public class Fire : MonoBehaviour
{
    private Tile tile;
    private int burnStep = 0;
    private Animator animator;

    public void Init(Tile tileRef)
    {
        tile = tileRef;
        burnStep = 0;
        // 可以播放 Fire_Ready 动画
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetInteger("Step", burnStep); // 播放 Fire_Ready
        }
    }

    public void Step()
    {
        burnStep++;
        if (animator != null)
        {
            animator.SetInteger("Step", burnStep);
        }
        else if (burnStep >= 6)
        {
            // 播放 fire_off 动画
            tile.hasFire = false;
            tile.isBurnable = false;
            Destroy(gameObject);
        }
    }
}
