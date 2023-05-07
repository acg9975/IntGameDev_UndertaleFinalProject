using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Behavior", menuName = "Combat/Enemy Behavior")]
public class EnemyBehavior : ScriptableObject
{
    [SerializeField] [Min(0)] private int maxHealth;
    [SerializeField] [Min(0)] private int health;
    public Sprite EnemySprite
    {
        get
        {
            return phases[phaseIndex].sprite;
        }
    }

    [SerializeField] private bool canBeSpared;
    public bool CanBeSpared
    {
        get
        {
            return canBeSpared;
        }
    }

    private string wfp;//weaknessfailphrase
    private string wep;//weaknessexploitedphrase
    //these get changed according to the current phrase
    public string WFP
    {
        get
        {
            return wfp;
        }
    }
    public string WEP
    {
        get
        {
            return wep;
        }
    }


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
    public enum WeakTo { criticize, compliment, threat };
    public WeakTo weakTo= WeakTo.criticize;
    [SerializeField]
    private int weaknessCheck = 3;
    public int WeaknessCheck
    {
        get
        {
            return weaknessCheck;
        }
        set
        {
            weaknessCheck = Mathf.Clamp(value, 0, 99);
        }
    }

    private Attack currentAttack;

    [System.Serializable]
    public class Phase
    {
        public Sprite sprite;

        public enum IterationType { InOrder, Random }

        public IterationType iterationType;
        public Attack[] attacks;
        [TextArea(1, 4)] public string[] descriptions;

        private int attackIndex = -1;
        private int descriptionIndex = -1;
        [SerializeField][Tooltip("Phrase used when the player chooses the correct weakness")]
        private string weaknessExploitedPhrase = "";

        [SerializeField][Tooltip("Phrase used when the player chooses the wrong weakness")]
        private string weaknessFailPhrase = "";
        public string WeaknessFailPhrase
        {
            get
            {
                return weaknessFailPhrase;
            }
        }
        public string WeaknessExploitedPhrase
        {
            get
            {
                return weaknessExploitedPhrase;
            }
        }

        [SerializeField] [Tooltip("If health is not higher than this, then it goes to the next phase")]
        private int healthRange = 0;
        public int HealthRange
        {
            get
            {
                return healthRange;
            }
        }
        [SerializeField][Tooltip("If weaknessCheck is below this amount, we go to the other phase")]
        private int weaknessRange = 0;
        public int WeaknessRange
        {
            get
            {
                return weaknessRange;
            }
        }

        public void Reset()
        {
            attackIndex = -1;
            descriptionIndex = -1;
        }

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
        phaseIndex = 0;

        foreach (Phase phase in phases)
            phase.Reset();
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

    public void nextPhase()
    {
        if (phaseIndex < phases.Length - 1)
        {
            phaseIndex++;
        }
        wfp = phases[phaseIndex].WeaknessFailPhrase;
        wep = phases[phaseIndex].WeaknessExploitedPhrase;

    }

    public void checkChangePhase()
    {
        /*failed attempt, discard
        //this is made to check if we need to change the phase of this enemy
        //if the health / phases length
        if (phases.Length > 1)
        {

        }
        int blockOfHealth = maxHealth / phases.Length;

        //if we have an enemy with 9 health, a block of health would be 3
        //we then see what block of health the player is in. As healthblocks will always scale with the amount of health
        //we can

        //if in  first health block (3), go to last phase
        //if in second health block (6), go to second to last phase
        //if in third health block (9), go to third to last 
        if (Health <= blockOfHealth )
        {
            phaseIndex = phases.Length - 1 ;
        }
        */
        //much simpler but more work required, if we are below the range described in the current phase, move to next phase
        if (Health < phases[phaseIndex].HealthRange)
        {
            nextPhase();
        }


        //if the current
        if (weaknessCheck < phases[phaseIndex].WeaknessRange)
        {
            nextPhase();
        }
    }

    public void PhraseInitialization()
    {
        //called at the start of the battle to set phrases
        wep = phases[phaseIndex].WeaknessExploitedPhrase;
        wfp = phases[phaseIndex].WeaknessFailPhrase;

    }

}
