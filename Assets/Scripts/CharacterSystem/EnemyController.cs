using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        // 示例默认朝下
        SetDirection(Vector2.down);
    }

    public void SetDirection(Vector2 dir)
    {
        if (dir == Vector2.up)
            animator.SetInteger("Direction", 3);
        else if (dir == Vector2.down)
            animator.SetInteger("Direction", 0);
        else if (dir == Vector2.left)
            animator.SetInteger("Direction", 1);
        else if (dir == Vector2.right)
            animator.SetInteger("Direction", 2);
    }
}

