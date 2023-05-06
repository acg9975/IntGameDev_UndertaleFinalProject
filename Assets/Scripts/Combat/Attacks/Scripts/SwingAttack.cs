using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "SwingAttack", menuName = "Combat/Attacks/SwingAttack")]
public class SwingAttack : Attack
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float spawnDelay = 1f;
    [SerializeField] protected float destroyTime = 1f;
    [SerializeField][Tooltip("1 = normal. Less means slower attack, higher means faster")] protected float attackSpeed = 1f;

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
            //spawns at the center of the box, at a random rotation
            //timeline does most of the work
            Vector2 spawnPos = CombatManager.AttackCenter;
            float z = Random.Range(0, 359);

            GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

            if (projectile.GetComponentInChildren<PlayableDirector>().playableGraph.GetRootPlayable(0).GetSpeed() != attackSpeed)
            {
                projectile.GetComponentInChildren<PlayableDirector>().playableGraph.GetRootPlayable(0).SetSpeed(attackSpeed);
            }

            projectiles.Add(projectile);
            Destroy(projectile, destroyTime);
            projectile.transform.Rotate(new Vector3(0,0,z));
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public override void Stop()
    {
        base.Stop();

        CombatManager.instance.StopCoroutine(runRoutine);
    }
}
