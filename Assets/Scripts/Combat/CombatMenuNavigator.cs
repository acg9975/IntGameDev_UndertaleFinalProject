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


    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;

    [SerializeField]
    private Image[] combatButtons;
    int selected;


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
                        break;
                    default:
                        break;
                }
            }



        }
    }
}
