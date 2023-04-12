using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : ScriptableObject
{
    protected List<AttackProjectile> projectiles = new List<AttackProjectile>();

    public virtual void Run()
    {

    }

    public virtual void Stop()
    {
        foreach (AttackProjectile projectile in projectiles)
            Destroy(projectile);

        projectiles.Clear();
    }
}
