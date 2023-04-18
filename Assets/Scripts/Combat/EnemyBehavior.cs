using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Behavior", menuName = "Combat/Enemy Behavior")]
public class EnemyBehavior : ScriptableObject
{
    [SerializeField] [Min(0)] private int maxHealth;
    [SerializeField] [Min(0)] private int health;

    public int MaxHealth { get { return maxHealth; } }

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = Mathf.Clamp(value, 0, MaxHealth);
        }
    }

    [System.Serializable]
    public class Phase
    {
        public enum IterationType { InOrder, Random }

        public IterationType iterationType;
        public Attack[] attacks;

        private int attackIndex = -1;

        public Attack GetNextAttack()
        {
            if (iterationType == IterationType.InOrder)
            {
                attackIndex++;
                if (attackIndex >= attacks.Length) attackIndex = 0;

                return attacks[attackIndex];
            }
            else
            {
                return attacks[Random.Range(0, attacks.Length)];
            }
        }
    }

    [SerializeField] private Phase[] phases;

    private int phaseIndex = 0;

    public void Init()
    {
        Health = MaxHealth;
    }

    public float NextWave()
    {
        Attack attack = phases[phaseIndex].GetNextAttack();

        attack.Run();
        return attack.duration;
    }

    public void StopWave()
    {
        Attack attack = phases[phaseIndex].GetNextAttack();
        attack.Stop();
    }
}
