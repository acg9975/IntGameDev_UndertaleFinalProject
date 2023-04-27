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
    public enum WeakTo { criticise, compliment, threat };
    public WeakTo weakTo= WeakTo.criticise;
    private int weaknessCheck = 3;

    private Attack currentAttack;

    [System.Serializable]
    public class Phase
    {
        public enum IterationType { InOrder, Random }

        public IterationType iterationType;
        public Attack[] attacks;
        [TextArea(1, 4)] public string[] descriptions;

        private int attackIndex = -1;
        private int descriptionIndex = -1;

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

        public string GetNextDescription()
        {
            if (descriptions.Length == 0) return null;
            
            if (iterationType == IterationType.InOrder)
            {
                descriptionIndex++;
                if (descriptionIndex >= descriptions.Length) descriptionIndex = 0;

                return descriptions[descriptionIndex];
            }
            else
            {
                return descriptions[Random.Range(0, descriptions.Length)];
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
        currentAttack = phases[phaseIndex].GetNextAttack();

        currentAttack.Run();
        return currentAttack.duration;
    }

    public void StopWave()
    {
        currentAttack.Stop();
    }

    public string NextDescription()
    {
        return phases[phaseIndex].GetNextDescription();
    }
}
