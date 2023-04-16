using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private EnemyBehavior[] enemies;

    public enum CombatMode { Menu, Attack }
    private CombatMode combatMode = CombatMode.Menu;

    private IEnumerator waveRoutine;

    private void Start()
    {
        ShowCombatMenu();
    }

    private void Update()
    {
        if (combatMode == CombatMode.Menu)
            if (Input.GetKeyUp(KeyCode.Space))
                TriggerWave();
    }

    private void ShowCombatMenu()
    {
        combatMode = CombatMode.Menu;
    }

    private void TriggerWave()
    {
        combatMode = CombatMode.Attack;
        waveRoutine = WaveRoutine();
        StartCoroutine(waveRoutine);
    }

    private IEnumerator WaveRoutine()
    {
        float minDuration = float.PositiveInfinity;

        foreach (EnemyBehavior enemy in enemies)
        {
            float duration = enemy.NextWave();

            if (duration < minDuration)
                minDuration = duration;
        }

        yield return new WaitForSeconds(minDuration);

        foreach (EnemyBehavior enemy in enemies)
            enemy.StopWave();

        ShowCombatMenu();
    }
}
