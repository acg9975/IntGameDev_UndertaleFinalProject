using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test", menuName = "Combat/Attacks/DoubleFly")]
public class DoubleFlyAttack : Attack
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float projectileSpeed = 1f;
    [SerializeField] protected float spawnDelay = 0.8f;
    [SerializeField] protected float spawnRadius = 3f;
    [SerializeField] protected float changeDirTime = 1f;
    [SerializeField] protected float waitTime = 0.25f;

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
            //spawn on the unit circle
            //go towards whereever heart is currently
            //then go towards where heart is again

            //initialization - spawn on unit circle
            Vector2 spawnPos = CombatManager.AttackCenter + Random.insideUnitCircle.normalized * spawnRadius;
            GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            projectiles.Add(projectile);

            while (projectile != null)
            {
                //go to wherever heart is currently
                //projectile.MoveTowards(CombatMovement.PlayerPosition, projectileSpeed);
                projectile.GetComponent<AttackProjectile>().MoveTowards(CombatMovement.PlayerPosition, projectileSpeed);
                projectile.transform.up = (Vector3)CombatMovement.PlayerPosition - projectile.transform.position;
                //after whatever amount of time - change direction to where player is
                yield return new WaitForSeconds(changeDirTime);
                //stay in place for 0.25seconds
                if (projectile != null)
                {
                    projectile.GetComponent<AttackProjectile>().MoveTowards(projectile.transform.position, 0);
                    projectile.transform.up = (Vector3)CombatMovement.PlayerPosition - projectile.transform.position;

                    yield return new WaitForSeconds(waitTime);
                    Vector3 newTargetPos = CombatMovement.PlayerPosition;
                    projectile.GetComponent<AttackProjectile>().MoveTowards(newTargetPos, projectileSpeed);
                    projectile.transform.up = (Vector3)CombatMovement.PlayerPosition - projectile.transform.position;

                    Debug.Log("Moving towards second goal");
                }
            }


            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public override void Stop()
    {
        base.Stop();

        CombatManager.instance.StopCoroutine(runRoutine);
    }
}
