using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test", menuName = "Combat/Attacks/Test")]
public class Attack_Test : Attack
{
    [SerializeField] protected AttackProjectile projectilePrefab;

    public override void Run()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-20f, 20f), Random.Range(-20f, 20f));
            AttackProjectile projectile = Instantiate(projectilePrefab, pos, Quaternion.identity);
            projectiles.Add(projectile);
        }
    }

    public override void Stop()
    {
        base.Stop();
    }
}
