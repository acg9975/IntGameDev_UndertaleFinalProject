using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : ScriptableObject
{
    public float duration = 10f;

    protected List<AttackProjectile> projectiles = new List<AttackProjectile>();

    public virtual void Run()
    {

    }

    public virtual void Stop()
    {
        foreach (AttackProjectile projectile in projectiles)
            if (projectile != null)
                Destroy(projectile.gameObject);

        projectiles.Clear();
    }
}
