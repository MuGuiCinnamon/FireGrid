using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    private List<EnemyController> enemies = new List<EnemyController>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public void StepAllEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.MoveIfPossible();
        }
    }
<<<<<<< Updated upstream
=======
    public List<EnemyController> GetAllEnemies()
    {
        return enemies;
    }
    public void UnregisterEnemy(EnemyController enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            CheckVictoryCondition(); // 敌人消失后检查
        }
    }

    private void CheckVictoryCondition()
    {
        if (enemies.Count == 0)
        {
            Debug.Log("🎉 所有敌人已被击败！");
            UIManager.Instance?.ShowVictory(); // 你可以实现这个函数弹出胜利界面
        }
    }
    


>>>>>>> Stashed changes
}
