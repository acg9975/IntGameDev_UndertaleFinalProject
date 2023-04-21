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
        int cap = PlayerData.inventory.getCurrentItems().Capacity;
        Debug.Log("current cap"+ cap);
        for (int i = 0; i < PlayerData.inventory.getCurrentItems().Capacity - 1; i++)
        {
            Debug.Log("Update text:"+ i);
            itemText[i].text = PlayerData.inventory.getCurrentItems()[i].itemName;
        }
        

        //if the length of the inventory is shorter than the length of the item array capacity
        if (PlayerData.inventory.getCurrentItems().Capacity < itemText.Length)
        {
            cap = PlayerData.inventory.getCurrentItems().Capacity;

            //int lastpopulatedposition = itemText.Length - cap;
            //Debug.Log("lastpop: "+cap);
            
            for (int i = itemText.Length - 1; i >= cap ; i--)
            {
                Debug.Log("i:"+i);
                //if (i > cap)
                //{
                itemText[i].gameObject.SetActive(false);
                //}
            }
        }

    }

    void itemSelection()
    {
        int cap = PlayerData.inventory.getCurrentItems().Capacity;
        int lastpopulatedposition = itemText.Length - cap;
        //handle going back
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("IUIM:setting unactive");
            setActive(false);
            CombatManager.instance.combatMode = CombatManager.CombatMode.Menu;
        }
        //handle selection
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            position--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            position++;
        }

        if (position < 0 )
        {
            position = lastpopulatedposition;
        }

        if (position > cap)
        {
            position = 0;
        }

        //handle changing color
        itemText[position].color = new Color32(0,0,0,0); ;

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
            //make it the enemys turn
            //set back the combatmode to enemyfight
            //probably have a coroutine do this to give the player a second before combat
            if (SceneManager.GetActiveScene().name == "Combat")
            {
                CombatManager.instance.combatMode = CombatManager.CombatMode.PlayerDefend;
            }
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
