using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatMenuNavigator : MonoBehaviour
{
    public static CombatMenuNavigator instance;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject playerAttackBox;
    [SerializeField] private GameObject playerDefendBox;
    [SerializeField] private GameObject mercyBox;

    [SerializeField]private DialogueBox db;


    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;

    [SerializeField]
    private Image[] combatButtons;
    int selected;

    [SerializeField]private TextMeshProUGUI[] mercyButtons;

    bool canPressbutton = false;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateCombatUI()
    {
        CombatManager.CombatMode combatMode = CombatManager.instance.combatMode;

        dialogueBox.SetActive(combatMode == CombatManager.CombatMode.Menu || combatMode == CombatManager.CombatMode.Inactive);
        playerAttackBox.SetActive(combatMode == CombatManager.CombatMode.PlayerAttack);
        playerDefendBox.SetActive(combatMode == CombatManager.CombatMode.PlayerDefend);
        mercyBox.SetActive(combatMode == CombatManager.CombatMode.Mercy);


        //inventory is set active in inventoryUIManager

        healthText.text = PlayerData.Health + "/" + PlayerData.MaxHealth;
        enemyHealthText.text = CombatManager.Enemy.Health + "/" + CombatManager.Enemy.MaxHealth;



    }

    private void Update()
    {
        CombatManager.CombatMode combatMode = CombatManager.instance.combatMode;

        if (combatMode == CombatManager.CombatMode.Menu)
        {
            Debug.Log("Combat Mode menu - CombatManagerNav");
            if (Input.GetKeyDown(KeyCode.A))
            {
                selected--;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                selected++;
            }

            // if its the menu - have one option selected, out of an array of the buttons available
            if (selected > combatButtons.Length - 1)
            {
                selected = 0;
            }
            else if (selected < 0)
            {
                selected = combatButtons.Length - 1;
            }


            //get the image and change the color of it - give feedback that a button is selected
            combatButtons[selected].color = new Color32(65, 65, 65, 255);
            for (int i = 0; i < combatButtons.Length; i++)
            {
                if (i != selected)
                {
                    combatButtons[i].color = Color.white;
                }
            }

            //handle pressing spacebar to select option - use the button gameobject's name to find whats necessary - possibly use enum?
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log(combatButtons[selected].name);
                switch (combatButtons[selected].name)
                {
                    case "Fight":
                        //go to combat manager and activate combat
                        CombatManager.instance.PlayerAttacks();
                        break;
                    case "Act":
                        break;
                    case "Item":
                        CombatManager.instance.showInventory();
                        break;
                    case "Mercy":
                        CombatManager.instance.combatMode = CombatManager.CombatMode.Mercy;
                        UpdateCombatUI();
                        selected = 0;
                        StartCoroutine(spaceBarDelay());
                        break;
                    default:
                        break;
                }
            }
        }

        if (combatMode == CombatManager.CombatMode.Mercy)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                selected--;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                selected++;
            }

            // if its the menu - have one option selected, out of an array of the buttons available
            if (selected > mercyButtons.Length - 1)
            {
                selected = 0;
            }
            else if (selected < 0)
            {
                selected = mercyButtons.Length - 1;
            }


            //get the image and change the color of it - give feedback that a button is selected
            mercyButtons[selected].color = new Color32(150, 65, 65, 255);
            for (int i = 0; i < mercyButtons.Length; i++)
            {
                if (i != selected)
                {
                    mercyButtons[i].color = Color.white;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && canPressbutton)
            {
                switch (mercyButtons[selected].name)
                {
                    
                    case "Spare":
                        //activate combat manager spare routine
                        CombatManager.instance.spareSequence();
                        CombatManager.instance.combatMode = CombatManager.CombatMode.Inactive;
                        UpdateCombatUI();
                        break;
                    case "Flee":
                        //activate combat manager flee chance
                        CombatManager.instance.fleeSequence();
                        CombatManager.instance.combatMode = CombatManager.CombatMode.Inactive;

                        UpdateCombatUI();
                        break;
                    default:
                        break;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                CombatManager.instance.combatMode = CombatManager.CombatMode.Menu; 
                UpdateCombatUI();

            }

        }

        IEnumerator spaceBarDelay()
        {
            yield return new WaitForSeconds(1f);
            canPressbutton = true;
        }
    }

    public void changeDialogueBoxText()
    {
        //will communicate with dialogue box with the specific text needed.
        //Maybe this should take in a string. The string being sourced from the different bits of dialogue that would be said when scared, fighting, or generally reacting


        //Would it be easier to include a combat dialogue script to control what the NPC's say during dialogue?


    }

}
