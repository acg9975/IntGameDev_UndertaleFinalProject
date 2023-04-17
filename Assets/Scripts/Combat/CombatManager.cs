using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    [SerializeField] private GameObject playerPrefab;
    private GameObject player;

    [SerializeField] private Transform playerSpawn;
    public static Vector2 AttackCenter { get { return instance.playerSpawn.position; } }

    public enum CombatMode { Menu, Attack , PlayerAttack}//consider changing Attack to Defense
    [HideInInspector] public CombatMode combatMode = CombatMode.Menu;

    private static EnemyBehavior[] enemies;

    private IEnumerator waveRoutine;

    [SerializeField]
    private GameObject attackSliderPrefab;
    private GameObject attackSlider;
    [SerializeField]
    private GameObject attackSprite;//only a particle system until our artists create a proper sprite

    [SerializeField]
    private Transform attackSpriteTransform;
    //be able to set this from the enemy that transitions into the combat manager in the future
    //I know this is bad form but its late rn, will decide on how to make properly later
    public static int enemyHealth = 10;



    [SerializeField]
    private Transform sliderSpawn;
    private void Awake()
    {
        instance = this;
    }


    public static void SetEnemies(EnemyBehavior _enemy)
    {
        enemies = new EnemyBehavior[]{ _enemy };
    }

    public static void SetEnemies(EnemyBehavior[] _enemies)
    {
        enemies = _enemies;
    }

    private void Start()
    {
        ShowCombatMenu();
    }

    private void Update()
    {
        if (!PlayerData.IsAlive)
        {
            playerDeath();
        }

        if (combatMode == CombatMode.Menu)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                PlayerAttacks();
            //if (Input.GetKeyDown(KeyCode.Space))
            //    TriggerWave();

        }
    }

    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
    }

    private void ShowCombatMenu()
    {

        combatMode = CombatMode.Menu;
        Destroy(player);
        CombatMenuNavigator.instance.UpdateCombatUI();
    }

    private void TriggerWave()
    {
        combatMode = CombatMode.Attack;
        Debug.Log(combatMode);
        SpawnPlayer();
        CombatMenuNavigator.instance.UpdateCombatUI();

        waveRoutine = WaveRoutine();
        StartCoroutine(waveRoutine);
    }

    private IEnumerator WaveRoutine()
    {
        float minDuration = float.PositiveInfinity;

        foreach (EnemyBehavior enemy in enemies)
        {
            float duration = enemy.NextWave();

            if (duration < minDuration)
                minDuration = duration;
        }

        yield return new WaitForSeconds(minDuration);

        foreach (EnemyBehavior enemy in enemies)
            enemy.StopWave();

        ShowCombatMenu();
    }

    private void PlayerAttacks()
    {
        //a bar goes left and right, player has to press space at the right time, 
        combatMode = CombatMode.PlayerAttack;
        Debug.Log(combatMode);

        spawnSlider();
        CombatMenuNavigator.instance.UpdateCombatUI();
        //now waits for the slider info

    }

    public void setSliderInfo(AttackSlider.AttackValue av)
    {
        //attack enemy for the amount of damage specified
        switch (av)
        {
            case AttackSlider.AttackValue.low:
                //no damage
                enemyHealth -= 1;
                break;
            case AttackSlider.AttackValue.medium:
                enemyHealth -= 4;

                break;
            case AttackSlider.AttackValue.high:
                enemyHealth -= 8;

                break;
            case AttackSlider.AttackValue.mediumHigh:
                enemyHealth -= 6;
                break;
            default:
                break;
        }
        if (enemyHealth < 0)
            enemyHealth = 0;

        CombatMenuNavigator.instance.UpdateCombatUI();
        StartCoroutine(playerAttackFinish());

    }

    IEnumerator playerAttackFinish()
    {
        //spawn in attack sprite on enemy sprite
        //currently this is a particle system so we just find and activate it
        GameObject AS = Instantiate(attackSprite, attackSpriteTransform.position, Quaternion.identity);
        AS.GetComponent<ParticleSystem>().Play();
        Destroy(AS,1.5f);
        yield return new WaitForSeconds(1f);
        //send an order to display any dialogue in a sort of textbox
        //add a waitforseconds 


        if (enemyHealth <= 0)
            enemyDeath();
        else
            TriggerWave();

        //go to enemy attack 


    }
    
    void enemyDeath()
    {
        //give player item if necessary and say what the player was given
        //return player back to overworld
        SceneTransition.ChangeScene(SceneTransition.previousScene);
        //SceneTransition.instance.ChangeScene(SceneTransition.previousScene);
    }

    void playerDeath()
    {
        //goto main menu
        PlayerData.Health = PlayerData.MaxHealth;
        SceneTransition.ChangeScene("MainMenu");
    }

    void spawnSlider()
    {
        attackSlider = Instantiate(attackSliderPrefab, sliderSpawn.position, Quaternion.identity);
        attackSlider.GetComponent<AttackSlider>().setCM(this);
    }

}
