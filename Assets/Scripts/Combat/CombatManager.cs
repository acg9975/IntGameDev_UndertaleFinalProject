using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    public static EnemyBehavior Enemy;
    [SerializeField] private EnemyBehavior enemy;

    [SerializeField] private GameObject playerPrefab;
    private GameObject player;

    [SerializeField] private Transform playerSpawn;
    public static Vector2 AttackCenter { get { return instance.playerSpawn.position; } }

    public enum CombatMode { Menu, PlayerAttack, PlayerDefend, Inventory, Inactive, Mercy, Act}
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

        if (Enemy == null)
            SetEnemy(enemy);
    }


    public static void SetEnemy(EnemyBehavior enemy)
    {
        Enemy = enemy;
        Enemy.PhraseInitialization();
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
    }
    

    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
    }

    private void ShowCombatMenu()
    {
        combatMode = CombatMode.Menu;
        Destroy(player);
        CombatMenuNavigator.instance.UpdateCombatUI(Enemy.NextDescription());
    }

    public void PlayerAttacks()
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
                Enemy.Health -= 0;
                break;
            case AttackSlider.AttackValue.low:
                Enemy.Health -= 2;
                break;
            case AttackSlider.AttackValue.high:
                Enemy.Health -= 6;
                break;
            case AttackSlider.AttackValue.medium:
                Enemy.Health -= 4;
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


        if (Enemy.Health <= 0)
            enemyDeath();
        else
            TriggerWave();

        //go to enemy attack 


    }
    
    private void TriggerWave()
    {
        //we check if we need to change the phase yet or not
        Enemy.checkChangePhase();

        combatMode = CombatMode.PlayerDefend;
        //Debug.Log("PLAYER DEFEND");
        SpawnPlayer();
        CombatMenuNavigator.instance.UpdateCombatUI();

        waveRoutine = WaveRoutine();
        StartCoroutine(waveRoutine);
    }

    private IEnumerator WaveRoutine()
    {
        float minDuration = float.PositiveInfinity;

        float duration = Enemy.NextWave();

        if (duration < minDuration)
            minDuration = duration;

        yield return new WaitForSeconds(minDuration);

        Enemy.StopWave();

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
        SceneTransition.ChangeScene("MainMenu");
        PlayerData.Health = PlayerData.MaxHealth;
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
        yield return new WaitForSeconds(3f);
        TriggerWave();
    }

    public void showInventory()
    {
        //Debug.Log("CM: " + combatMode);
        combatMode = CombatMode.Inventory;
        inventoryUIManager.instance.updateText();
        inventoryUIManager.instance.setActive(true);
    }

    

    public void fleeSequence()
    {
        //player has a 10% chance to flee. if they do, then they automatically leave the battle
        //if they succeed, flash the dialogue that they have fled onto the dialogue box, set the state to inactive, and then after 5 seconds move back to overworld
        int x = Random.Range(1,10);

        combatMode = CombatMode.Inactive;
        Debug.Log("Flee chance: " + x);
        if (x == 1)
        {
            StartCoroutine(fleeSequenceTimer(true)) ;
        }
        else
        {
            StartCoroutine(fleeSequenceTimer(false));

        }

    }

    IEnumerator fleeSequenceTimer(bool isSuccessful)
    {

        //throw up dialogue box with text
        //set to inactive
        if (isSuccessful)
        {
            //throw up positive text - "You were able to escape"
            Debug.Log("Player is successful! They escape");
            CombatMenuNavigator.instance.UpdateCombatUI("You manage to escape!");
        }
        else
        {
            CombatMenuNavigator.instance.UpdateCombatUI("You attempt to flee but are unsuccessful!");
            //throw up negative text - "You were unable to escape"
            Debug.Log("Player fails!");
        }

        yield return new WaitForSeconds(3f);
        if (isSuccessful)
        {
            //player flees
            playerFlee();
        }
        else
        {
            TriggerWave();
        }
        CombatMenuNavigator.instance.UpdateCombatUI();

    }

    public void spareSequence()
    {
        //if enemy health is low enough (25%), and if the enemy can flee (make bool for this), allow them to flee
        //if it is not low enough, flash dialogue box with enemy taunting the player. set this for 5 seconds, and then move to enemy's turn
        if ((Enemy.Health <= Enemy.MaxHealth * 0.25f  && Enemy.CanBeSpared )|| Enemy.WeaknessCheck == 0)
        {
            StartCoroutine(spareTimer(true));
        }
        else
        {
            StartCoroutine(spareTimer(false));
        }

    }

    IEnumerator spareTimer(bool isSuccessful)
    {
        if (isSuccessful)
        {
            //display success text
            Debug.Log("Player spare success");
            CombatMenuNavigator.instance.UpdateCombatUI("The enemy sees that they are outmatched. They take the opportunity to flee.");
        }
        else
        {
            //display failure text
            Debug.Log("Player spare fail");
            CombatMenuNavigator.instance.UpdateCombatUI("Your enemy laughs at your attempt to spare them");
        }

        yield return new WaitForSeconds(5f);
        if (isSuccessful)
        {
            playerSpare();
        }
        else
        {
            TriggerWave();
        }


    }


    public void playerSpare()
    {
        
        //currently the same as enemy death, but later on will have different functionality
        //player might gain items on enemy death
        SceneTransition.ChangeScene(SceneTransition.previousScene);
    }
    public void playerFlee()
    {
        //currently the same as enemy death, but later on will have different functionality
        //player might gain items on enemy death - player will not gain items from this
        SceneTransition.ChangeScene(SceneTransition.previousScene);


    }


    public void actSequence(EnemyBehavior.WeakTo wt)//parameters will need to be an enemy weakness enum state
    {
        //player has 4 options
        //check - criticise - compliment - threat
        //certain enemies are weaker to certain options
        //we need a check in the enemybehavior scriptable object to see which the enemy is susceptible to
        //enum weakTo {check - criticise - compliment - threat}
        combatMode = CombatMode.Inactive;
        if (Enemy.weakTo == wt)
        {
            //get the current phase's correct phrase
            CombatMenuNavigator.instance.UpdateCombatUI(Enemy.WEP);
            Enemy.WeaknessCheck--;
            StartCoroutine(actTimer(true));

        }
        else
        {
            CombatMenuNavigator.instance.UpdateCombatUI(Enemy.WFP);
            StartCoroutine(actTimer(false));
        }
        //coroutine then takes in what the player chose and compares to what enemy is weak to
        //possibly show more than just "threat failed" or so based on what narrative designers make
        //but if its what the enemy is vulnerable to - show "threat success" and turn isSpareable to true
        //then allow them to spare the character in the next turn. 

        //we should probably implement a static counter for amount of enemies killed
    }

    IEnumerator actTimer(bool successful)
    {
        if (successful)
        {
            CombatMenuNavigator.instance.UpdateCombatUI(Enemy.WEP);

        }
        else
        {
            CombatMenuNavigator.instance.UpdateCombatUI(Enemy.WFP);

        }
        //weird bug in the ordering of when these are called, causing the updateCombatUI function to be called at the wrong point passing in null instead of the proper text
        yield return new WaitForSeconds(3f);
        TriggerWave();

    }

    public void checkSequence()
    {
        //once selected, show the player what the enemy is weak to then move to the checkTimer and then enemy attack
        StartCoroutine(checkTimer());
        combatMode = CombatMode.Inactive;
        CombatMenuNavigator.instance.UpdateCombatUI("Enemy is weak to " + Enemy.weakTo + ". You need " + Enemy.WeaknessCheck + " more attempts to get them to flee!");

    }

    private IEnumerator checkTimer()
    {
        yield return new WaitForSeconds(3f);
        TriggerWave();

    }

}
