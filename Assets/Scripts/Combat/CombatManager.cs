using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    [SerializeField] private GameObject playerPrefab;
    private GameObject player;

    [SerializeField] private Transform playerSpawn;
    public static Vector2 AttackCenter { get { return instance.playerSpawn.position; } }

    public enum CombatMode { Menu, Attack }
    [HideInInspector] public CombatMode combatMode = CombatMode.Menu;

    private static EnemyBehavior[] enemies;

    private IEnumerator waveRoutine;

    private void Awake()
    {
        instance = this;
    }

    public static void SetEnemies(EnemyBehavior _enemy)
    {
        enemies = new EnemyBehavior[]{ _enemy };
    }

    public static void SetEnemies(EnemyBehavior[] _enemies)
    {
        enemies = _enemies;
    }

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

    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
    }

    private void ShowCombatMenu()
    {
        combatMode = CombatMode.Menu;
        Destroy(player);
        CombatMenuNavigator.instance.UpdateCombatUI();
    }

    private void TriggerWave()
    {
        combatMode = CombatMode.Attack;
        SpawnPlayer();
        CombatMenuNavigator.instance.UpdateCombatUI();

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
