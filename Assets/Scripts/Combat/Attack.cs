using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : ScriptableObject
{
    public float duration = 10f;

    protected List<GameObject> projectiles = new List<GameObject>();

    public virtual void Run()
    {

    }

    public virtual void Stop()
    {
        foreach (GameObject projectile in projectiles)
            if (projectile != null)
                Destroy(projectile.gameObject);

        projectiles.Clear();
    }
}
