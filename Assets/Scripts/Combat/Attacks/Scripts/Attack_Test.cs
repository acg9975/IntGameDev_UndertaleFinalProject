using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test", menuName = "Combat/Attacks/Test")]
public class Attack_Test : Attack
{
    [SerializeField] protected AttackProjectile projectilePrefab;
    [SerializeField] protected float projectileSpeed = 1f;
    [SerializeField] protected float spawnDelay = 1f;
    [SerializeField] protected float spawnRadius = 3f;

    protected float spawnTimer = 0f;

    protected IEnumerator runRoutine;

    public override void Run()
    {
        runRoutine = RunRoutine();
        CombatManager.instance.StartCoroutine(runRoutine);
    }

    protected IEnumerator RunRoutine()
    {
        while (true)
        {
            Vector2 spawnPos = CombatManager.AttackCenter + Random.insideUnitCircle.normalized * spawnRadius;
            AttackProjectile projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            projectiles.Add(projectile);

            projectile.MoveTowards(CombatMovement.PlayerPosition, projectileSpeed);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public override void Stop()
    {
        base.Stop();

        CombatManager.instance.StopCoroutine(runRoutine);
    }
}
