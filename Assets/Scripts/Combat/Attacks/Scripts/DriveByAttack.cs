using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "DriveByAttack", menuName = "Combat/Attacks/DriveBy")]
public class DriveByAttack : Attack
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float attackSpeed = 1f;

    [Header("deprecated")]
    [SerializeField] protected Vector3 pos1Spawn = new Vector3(-1, -2.8f, 0);
    [SerializeField] protected Vector3 pos2Spawn = new Vector3(0, 2.15f, 0);
    [SerializeField] protected Vector3 pos3Spawn = new Vector3(1, -2.8f, 0);
    [SerializeField] protected float projSpeed;
    [SerializeField] protected float moveDistance;
    [SerializeField] protected float waveSpawnDelay = 1.8f;
    [SerializeField] protected float projDeleteDelay = 1.5f;

    protected float spawnTimer = 0f;

    protected IEnumerator runRoutine;

    public override void Run()
    {
        runRoutine = RunRoutine();
        CombatManager.instance.StartCoroutine(runRoutine);
    }

    protected IEnumerator RunRoutine()
    {
        GameObject car = Instantiate(projectilePrefab);

        if (car.GetComponentInChildren<PlayableDirector>().playableGraph.GetRootPlayable(0).GetSpeed() != attackSpeed)
        {
            car.GetComponentInChildren<PlayableDirector>().playableGraph.GetRootPlayable(0).SetSpeed(attackSpeed);
        }

        projectiles.Add(car);

        yield return new WaitForSeconds(0f);

        //while (true)
        //{
        //    //spawns the first one projectile going up. It takes 1.5f seconds to cross
        //    GameObject projectile = Instantiate(projectilePrefab, pos1Spawn, new Quaternion(0, 0, 0, 0));
        //    projectile.GetComponent<AttackProjectile>().MoveTowards(Vector3.up * moveDistance, projSpeed);
        //    projectile.transform.up = (Vector3)CombatMovement.PlayerPosition - projectile.transform.position;
        //    projectile.GetComponentInChildren<SpriteRenderer>().flipY = true;

        //    Destroy(projectile, projDeleteDelay);
        //    projectiles.Add(projectile);
        //    yield return new WaitForSeconds(waveSpawnDelay);
        //    GameObject projectile2 = Instantiate(projectilePrefab, pos2Spawn, new Quaternion(0, 0, 0, 0));
        //    projectile2.GetComponent<AttackProjectile>().MoveTowards(Vector3.down * moveDistance, projSpeed);
        //    projectile2.transform.up = (Vector3)CombatMovement.PlayerPosition - projectile2.transform.position;
        //    Destroy(projectile2, projDeleteDelay);
        //    projectiles.Add(projectile2);
        //    yield return new WaitForSeconds(waveSpawnDelay);
        //    GameObject projectile3 = Instantiate(projectilePrefab, pos3Spawn, new Quaternion(0,0,0,0));
        //    projectile3.GetComponent<AttackProjectile>().MoveTowards(Vector3.up * moveDistance, projSpeed);
        //    projectile3.transform.up = (Vector3)CombatMovement.PlayerPosition - projectile3.transform.position;
        //    projectile3.GetComponentInChildren<SpriteRenderer>().flipY = true;

        //    Destroy(projectile3, projDeleteDelay);
        //    projectiles.Add(projectile3);
        //    yield return new WaitForSeconds(waveSpawnDelay);
        //}
    }

    public override void Stop()
    {
        base.Stop();

        CombatManager.instance.StopCoroutine(runRoutine);
    }



}
