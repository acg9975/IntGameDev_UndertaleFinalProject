using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test", menuName = "Combat/Attacks/CurvedAttack")]
public class CurvedAttackTest : Attack
{
    // Start is called before the first frame update
    [SerializeField] protected GameObject projectilePrefab;

    [SerializeField] protected float spawnDelay = 1f;

    protected float spawnTimer = 0f;

    protected IEnumerator runRoutine;
    private bool canRun;

    public override void Run()
    {
        canRun = true;
        runRoutine = RunRoutine();
        CombatManager.instance.StartCoroutine(runRoutine);
    }

    protected IEnumerator RunRoutine()
    {
        while (canRun)
        {
            float y = Random.Range(-1.5f,1.6f);
                
            Vector2 spawnPos = new Vector2(CombatManager.AttackCenter.x + 1.8f, y);
            GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            projectiles.Add(projectile);
            Destroy(projectile, 1.5f);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public override void Stop()
    {
        base.Stop();
        if (runRoutine != null)
        {
            CombatManager.instance.StopCoroutine(runRoutine);
            canRun = false;
        }
    }
}
