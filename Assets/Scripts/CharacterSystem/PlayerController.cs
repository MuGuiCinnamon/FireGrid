using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gridSize = 0.96f; // 每格大小
    public Vector2Int gridPosition; // 当前玩家坐标（Tile Grid）
    private Tile currentTile;
    public Sprite idleSprite;
    public Sprite pressedSprite;

    private SpriteRenderer spriteRenderer;


    private bool isMoving = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Vector3 pos = transform.position;
        gridPosition = new Vector2Int(Mathf.RoundToInt(pos.x / gridSize), Mathf.RoundToInt(-pos.y / gridSize));
        if (TileMapManager.Instance != null)
        {
            UpdatePosition();
        }
        else
        {
            // Try again after a short delay if manager isn't ready
            StartCoroutine(DelayedInit());
        }
    }
    private IEnumerator DelayedInit()
    {
        yield return new WaitForSeconds(0.1f);
        UpdatePosition();
    }

    void Update()
    {
        if (isMoving) return;

        Vector2Int direction = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W)) direction = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S)) direction = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A)) direction = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D)) direction = Vector2Int.right;
        else if (Input.GetKeyDown(KeyCode.Space)) TryTriggerTileEffect();

        if (direction != Vector2Int.zero)
        {
            Vector2Int targetPos = gridPosition + direction;

            Tile targetTile = TileMapManager.Instance.GetTileAt(targetPos);
            if (targetTile != null && targetTile.isWalkable)
            {
                gridPosition = targetPos;
                StartCoroutine(MoveToPosition());
            }
        }
        // 空格触发反馈
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spriteRenderer.sprite = pressedSprite;
            Tile currentTile = TileMapManager.Instance.GetTileAt(gridPosition);


            currentTile?.OnPlayerInteract();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            spriteRenderer.sprite = idleSprite;
        }
    }

    private System.Collections.IEnumerator MoveToPosition()
    {
        isMoving = true;
        Vector3 target = new Vector3(gridPosition.x * gridSize, -gridPosition.y * gridSize, 0f);
        float t = 0;
        Vector3 start = transform.position;

        while (t < 1)
        {
            t += Time.deltaTime * 10; // 控制移动速度
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;
        isMoving = false;

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        // 更新当前所处 Tile
        currentTile = TileMapManager.Instance.GetTileAt(gridPosition);
    }

    private void TryTriggerTileEffect()
    {
        if (currentTile != null)
        {
            currentTile.OnPlayerInteract();
        }
    }
}
