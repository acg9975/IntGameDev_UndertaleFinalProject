using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.SceneManagement;

public class inventoryUIManager : MonoBehaviour
{
    public static inventoryUIManager instance;
    private void Awake()
    {
        instance = this;
    }

    //not too sure where to put this object

    //opens the inventory and allows them to support 
    //check if player is in combat

    private bool canActivateInventory = true;

    public bool isUIActive = false;

    [SerializeField]
    private GameObject UI;
    [SerializeField]
    private TextMeshProUGUI[] itemText;
    int position = 0;
    public void setActive(bool v)
    {
        if (v && !canActivateInventory)
        {
            //sometimes it gets stuck here from the combatMenuNav - so we just need to reset the combat mode
            CombatManager.instance.combatMode = CombatManager.CombatMode.Menu;
            return;
        }
        else 
        {
            UI.SetActive(v);
            isUIActive = v;
        }
    }



    public void updateText()
    {
        //we only display the name of the item if that slot is filled
        //get handle to inventory and fill in the items as needed
        int cap = PlayerData.inventory.getCurrentItemArray().Length;
        //Debug.Log("current cap"+ cap);
        for (int i = 0; i < cap; i++)
        {
            itemText[i].text = PlayerData.inventory.getCurrentItemArray()[i].itemName;
            itemText[i].gameObject.SetActive(true);
            /*
            if (PlayerData.inventory.getCurrentItemArray()[i] )
            {
                itemText[i].text = PlayerData.inventory.getCurrentItems()[i].itemName;
            }
            */
        }


        //if the length of the inventory is shorter than the length of the item array capacity
        if (PlayerData.inventory.getCurrentItemArray().Length < itemText.Length)
        {
            cap = PlayerData.inventory.getCurrentItemArray().Length;

            //int lastpopulatedposition = itemText.Length - cap;
            //Debug.Log("lastpop: "+cap);
            
            for (int i = itemText.Length - 1; i >= cap ; i--)
            {
                //Debug.Log("i:"+i);
                itemText[i].gameObject.SetActive(false);
                
            }
        }

    }

    void itemSelection()
    {
        int cap = PlayerData.inventory.getCurrentItemArray().Length;
        int lastpopulatedposition = itemText.Length - cap;
        //handle going back
        if (SceneManager.GetActiveScene().name == "Combat")
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                //Debug.Log("IUIM:setting unactive");
                setActive(false);
                CombatManager.instance.combatMode = CombatManager.CombatMode.Menu;
            }
            //handle selection
            if (Input.GetKeyDown(KeyCode.W))
            {
                position--;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                position++;
            }
            //Debug.Log("Pos: "+position + " cap:" + cap);
            if (position < 0)
            {
                position = cap - 1;
            }

            if (position > cap - 1)
            {
                position = 0;
            }

            //handle changing color
            itemText[position].color = new Color32(65, 65, 65, 255); ;
            for (int i = 0; i < itemText.Length; i++)
            {
                if (i != position)
                {
                    itemText[i].color = Color.white;
                }
            }
        }
        
        //handle selecting an item and using it

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C)) && canActivateInventory)
        {
            //if there is at least one item in inventory, allow space to 
            if (cap != 0 && SceneManager.GetActiveScene().name == "Combat")
            {

                //find the item we are gonna use and then use it
                item it = PlayerData.inventory.findItem(position);
                PlayerData.inventory.useItem(it);
                //update our inventory text
                updateText();
                //close the inventory
                setActive(false);
                //make it the enemys turn - this connects into a coroutine, so we still see the player's health as it updates
                CombatManager.instance.finishPlayerTurn();
                //update UI
                CombatManager.instance.combatMode = CombatManager.CombatMode.Inactive;//this just makes it flash the dialogue screen for a quick second - will change if we update dialogue system in fights
                CombatMenuNavigator.instance.UpdateCombatUI();

            }
            else if(cap == 0)//set it to false after space is pressed - therefore escape and space can both be used
            {
                Debug.Log("setting to false");
                setActive(false);
                //if in combat, go to combat mode menu
                if (SceneManager.GetActiveScene().name == "Combat")
                {
                    CombatManager.instance.combatMode = CombatManager.CombatMode.Menu;
                    StartCoroutine(canActivateInventoryTimer());
                    //Debug.Log("canActivateinv false");

                }
                else {
                    OverworldMovement.canMove = true;
                }
            }

        }


    }


    IEnumerator canActivateInventoryTimer()
    {
        CombatManager.instance.combatMode = CombatManager.CombatMode.Menu;
        CombatMenuNavigator.instance.UpdateCombatUI();
        canActivateInventory = false;
        yield return new WaitForSeconds(1);//wait for half a second to turn this back on
        canActivateInventory = true ;
        //Debug.Log("canActivateinv true  ");

    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Combat")
        {
            CombatManager.CombatMode combatMode = CombatManager.instance.combatMode;
            //debated on whether to control inventory activeness checking in here or in 
            if (combatMode == CombatManager.CombatMode.Inventory)
            {
                itemSelection();
            }

        }
        else if (Input.GetKeyDown(KeyCode.C) && canActivateInventory && !isUIActive)
        {

            if (canActivateInventory)
            {
                updateText();
                //make sure player cannot move
                OverworldMovement.canMove = false;
                setActive(true);
            }
            
        }
        else if (isUIActive)
        {
            itemSelection();

        }



    }
}
