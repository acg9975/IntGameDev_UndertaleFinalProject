using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private EnemyBehavior[] enemies;

    private void Start()
    {
        foreach (EnemyBehavior enemy in enemies)
            enemy.NextWave(true);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (EnemyBehavior enemy in enemies)
                enemy.NextWave();
        }
    }
}
