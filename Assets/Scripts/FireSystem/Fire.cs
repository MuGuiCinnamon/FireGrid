using UnityEngine;

public class Fire : MonoBehaviour
{
    private Tile tile;
    private int burnStep = 0;

    public void Init(Tile tileRef)
    {
        tile = tileRef;
        burnStep = 0;
        // 可以播放 Fire_Ready 动画
    }

    public void Step()
    {
        burnStep++;
        if (burnStep == 1)
        {
            // 播放 fire_on 动画
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
