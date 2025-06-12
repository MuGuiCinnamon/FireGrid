using UnityEngine;
using System.Collections; // 添加这行

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Sprite idleSprite;
    public Sprite pressedSprite;

    private SpriteRenderer spriteRenderer;
    private Vector2Int gridPosition;
    private bool isMoving = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 初始化时对齐到最近的网格
        SnapToGrid();
    }

    private void Update()
    {
        if (isMoving) return;

        HandleMovementInput();
        HandleInteractionInput();
    }

    private void HandleMovementInput()
    {
        Vector2Int direction = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W)) direction = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S)) direction = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A)) direction = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D)) direction = Vector2Int.right;

        if (direction != Vector2Int.zero)
        {
            Vector2Int targetPos = gridPosition + direction;
            TryMoveTo(targetPos);
        }
    }

    private void HandleInteractionInput()
    {
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

    private void TryMoveTo(Vector2Int targetPos)
    {
        Tile targetTile = TileMapManager.Instance.GetTileAt(targetPos);
        if (targetTile != null && targetTile.isWalkable)
        {
            gridPosition = targetPos;
            StartCoroutine(MoveToPosition());
        }
    }

    private IEnumerator MoveToPosition()
    {
        isMoving = true;
        Vector3 targetWorldPos = new Vector3(
            gridPosition.x * TileFactory.GridSize,
            -gridPosition.y * TileFactory.GridSize,
            0
        );

        float t = 0;
        Vector3 startPos = transform.position;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, targetWorldPos, t);
            yield return null;
        }

        transform.position = targetWorldPos;
        isMoving = false;
    }

    private void SnapToGrid()
    {
        // 计算最近的网格位置
        gridPosition = new Vector2Int(
            Mathf.RoundToInt(transform.position.x / TileFactory.GridSize),
            Mathf.RoundToInt(-transform.position.y / TileFactory.GridSize)
        );

        // 对齐到网格
        transform.position = new Vector3(
            gridPosition.x * TileFactory.GridSize,
            -gridPosition.y * TileFactory.GridSize,
            0
        );
    }
}