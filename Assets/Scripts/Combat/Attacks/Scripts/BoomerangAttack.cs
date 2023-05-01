using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test", menuName = "Combat/Attacks/BoomerangAttack")]
public class BoomerangAttack : Attack
{
    // Start is called before the first frame update
    [SerializeField] protected GameObject upProjectilePrefab;
    [SerializeField] protected GameObject downProjectilePrefab;


    [SerializeField] protected float spawnDelay = 1.5f;

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
            //going up 
            float x1 = Random.Range(-1.5f, 1.75f);
            Vector2 spawnPos1 = new Vector2(x1, -2.76f);
            GameObject projectile1 = Instantiate(upProjectilePrefab, spawnPos1, Quaternion.identity);
            projectiles.Add(projectile1);
            Destroy(projectile1, 1.5f);
            
            //going down
            float x2 = Random.Range(-1.5f, 1.75f);
            Vector2 spawnPos2 = new Vector2(x2, 2.15f);
            GameObject projectile2 = Instantiate(downProjectilePrefab, spawnPos2, Quaternion.Euler(0, 0, 180));
            projectiles.Add(projectile2);
            Destroy(projectile2, 1.5f);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public override void Stop()
    {
        base.Stop();
        if (runRoutine != null)
        {
            CombatManager.instance.StopCoroutine(runRoutine);
        }
    }
}
