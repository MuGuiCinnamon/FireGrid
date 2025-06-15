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
    public List<EnemyController> GetAllEnemies()
    {
        return enemies;
    }

}
