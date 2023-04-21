using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    [SerializeField] private EnemyBehavior enemy;
    public static EnemyBehavior Enemy { get { return instance.enemy; } }

    [SerializeField] private GameObject playerPrefab;
    private GameObject player;

    [SerializeField] private Transform playerSpawn;
    public static Vector2 AttackCenter { get { return instance.playerSpawn.position; } }

    public enum CombatMode { Menu, PlayerAttack, PlayerDefend, Inventory, Inactive}
    [HideInInspector] public CombatMode combatMode = CombatMode.Menu;

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
    //public static int enemyHealth = 10;

    [SerializeField]
    private Transform sliderSpawn;

    [SerializeField]
    item exampleItem;

    private void Awake()
    {
        instance = this;
    }


    public static void SetEnemy(EnemyBehavior enemy)
    {
        instance.enemy = enemy;
    }

    private void Start()
    {
        Enemy.Init();
        ShowCombatMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerData.inventory.addItem(exampleItem);
            inventoryUIManager.instance.updateText();
        }

        if (!PlayerData.IsAlive)
        {
            playerDeath();
        }

        if (combatMode == CombatMode.Menu)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                PlayerAttacks();
            if (Input.GetKeyDown(KeyCode.C))
            {
                //Debug.Log("CM: " + combatMode);
                combatMode = CombatMode.Inventory;
                inventoryUIManager.instance.updateText();
                inventoryUIManager.instance.setActive(true);
            }
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
            case AttackSlider.AttackValue.fail:
                //no damage
                enemy.Health -= 0;
                break;
            case AttackSlider.AttackValue.low:
                enemy.Health -= 2;
                break;
            case AttackSlider.AttackValue.high:
                enemy.Health -= 6;
                break;
            case AttackSlider.AttackValue.medium:
                enemy.Health -= 4;
                break;
        }

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


        if (enemy.Health <= 0)
            enemyDeath();
        else
            TriggerWave();

        //go to enemy attack 


    }
    
    private void TriggerWave()
    {
        combatMode = CombatMode.PlayerDefend;
        Debug.Log("PLAYER DEFEND");
        SpawnPlayer();
        CombatMenuNavigator.instance.UpdateCombatUI();

        waveRoutine = WaveRoutine();
        StartCoroutine(waveRoutine);
    }

    private IEnumerator WaveRoutine()
    {
        float minDuration = float.PositiveInfinity;

        float duration = enemy.NextWave();

        if (duration < minDuration)
            minDuration = duration;

        yield return new WaitForSeconds(minDuration);

        enemy.StopWave();

        ShowCombatMenu();
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

    public void finishPlayerTurn()
    {
        //this is called in scripts like inventoryUIManager and likely others
        //main point is to just allow the turn to end, maybe show dialogue if necessary
        //or use a coroutine to give the player some seconds to recover (juice/polish)
        //then move to the enemy's turn

        StartCoroutine(waitforturn());
    }

    private IEnumerator waitforturn()
    {
        yield return new WaitForSeconds(1f);
        TriggerWave();
    }
}
