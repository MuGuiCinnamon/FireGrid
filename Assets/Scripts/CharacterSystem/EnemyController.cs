using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector2Int gridPosition;
    public Vector2Int moveDirection = Vector2Int.right; // 初始方向
    public float gridSize = 0.96f;

    private bool isMoving = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        Vector3 pos = transform.position;
        gridPosition = new Vector2Int(Mathf.RoundToInt(pos.x / gridSize), Mathf.RoundToInt(-pos.y / gridSize));
        transform.position = new Vector3(gridPosition.x * gridSize, -gridPosition.y * gridSize, 0f);

        UpdateFacingDirection(); // 设置初始朝向动画
        EnemyManager.Instance.RegisterEnemy(this);
    }

    public void MoveIfPossible()
    {
        if (isMoving) return;

        Vector2Int targetPos = gridPosition + moveDirection;
        Tile targetTile = TileMapManager.Instance.GetTileAt(targetPos);
        if (targetTile != null && targetTile.isWalkable)
        {
            StartCoroutine(MoveToPosition(targetPos));
        }
        // 若不能走，不动（可扩展为转弯）
    }

    private System.Collections.IEnumerator MoveToPosition(Vector2Int targetPos)
    {
        isMoving = true;

        Vector3 start = transform.position;
        Vector3 end = new Vector3(targetPos.x * gridSize, -targetPos.y * gridSize, 0f);

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 10f;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
        gridPosition = targetPos;
        isMoving = false;

        UpdateFacingDirection();
        if (FireUtility.IsFireActiveAt(gridPosition))
        {
            Debug.Log("🔥 敌人接触火焰，被烧毁！");
            Destroy(gameObject);
        }
    }

    private void UpdateFacingDirection()
    {
        if (animator == null) return;

        if (moveDirection == Vector2Int.up)
            animator.SetInteger("Direction", 3);
        else if (moveDirection == Vector2Int.down)
            animator.SetInteger("Direction", 0);
        else if (moveDirection == Vector2Int.left)
            animator.SetInteger("Direction", 1);
        else if (moveDirection == Vector2Int.right)
            animator.SetInteger("Direction", 2);
    }
}
