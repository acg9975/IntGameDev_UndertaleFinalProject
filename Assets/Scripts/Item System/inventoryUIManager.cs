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
    

    public bool isUIActive = false;

    [SerializeField]
    private GameObject UI;
    [SerializeField]
    private TextMeshProUGUI[] itemText;
    int position = 0;
    public void setActive(bool v)
    {
        UI.SetActive(v);
        isUIActive = v;
    }



    public void updateText()
    {
        //we only display the name of the item if that slot is filled
        //get handle to inventory and fill in the items as needed
        int cap = PlayerData.inventory.getCurrentItemArray().Length;
        Debug.Log("current cap"+ cap);
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
                Debug.Log("i:"+i);
                itemText[i].gameObject.SetActive(false);
                
            }
        }

    }

    void itemSelection()
    {
        int cap = PlayerData.inventory.getCurrentItemArray().Length;
        int lastpopulatedposition = itemText.Length - cap;
        //handle going back
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
        if (position < 0 )
        {
            position = cap - 1;
        }

        if (position > cap - 1)
        {
            position = 0;
        }

        //handle changing color
        itemText[position].color = new Color32(65,65,65,255); ;
        for (int i = 0; i < itemText.Length; i++)
        {
            if (i != position)
            {
                itemText[i].color = Color.white;
            }
        }
        //handle selecting an item and using it

        if (Input.GetKeyDown(KeyCode.Space))
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


    }

    private void Update()
    {
        CombatManager.CombatMode combatMode = CombatManager.instance.combatMode;
        //debated on whether to control inventory activeness checking in here or in 
        if (combatMode == CombatManager.CombatMode.Inventory)
        {
            itemSelection();
        }



    }
}
