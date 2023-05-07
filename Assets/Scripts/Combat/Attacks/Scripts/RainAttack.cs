using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rain", menuName = "Combat/Attacks/RainAttack")]
public class RainAttack : Attack
{
    [SerializeField] protected GameObject boxPrefab;
    [SerializeField] protected GameObject[] dropPrefabs;

    [Space]
    [SerializeField] protected Vector2 dropSpawn;
    [SerializeField] protected float dropRange = 2f;
    [SerializeField] protected float dropLeftSpeed;
    [SerializeField] protected float dropRightSpeed;

    [Space]
    [SerializeField] protected float initialDelay = 1f;
    [SerializeField] protected float spawnDelay = 1f;

    protected float spawnTimer = 0f;

    protected IEnumerator runRoutine;

    public override void Run()
    {
        runRoutine = RunRoutine();
        CombatManager.instance.StartCoroutine(runRoutine);
    }

    protected IEnumerator RunRoutine()
    {
        GameObject box = Instantiate(boxPrefab);
        Destroy(box, duration);

        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            Vector2 spawnPos = dropSpawn + Random.insideUnitCircle.normalized * dropRange;
            GameObject drop = Instantiate(dropPrefabs[Random.Range(0, dropPrefabs.Length)], spawnPos, Quaternion.identity);

            projectiles.Add(drop);

            Vector2 dir = Vector2.right * Random.Range(-dropLeftSpeed, dropRightSpeed);
            drop.GetComponent<AttackProjectile>().SetVelocity(dir);

            Destroy(drop, 4f);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public override void Stop()
    {
        base.Stop();

        CombatManager.instance.StopCoroutine(runRoutine);
    }
}
