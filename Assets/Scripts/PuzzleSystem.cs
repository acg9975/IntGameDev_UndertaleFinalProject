using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSystem : MonoBehaviour
{

    [SerializeField]
    private item[] keyItems;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {

            bool containsAllKeys = false;
            //for every item in the list of key items,
            //if it is not in the inventory then set the current item to false.
            //If it is in there, set to true
            foreach (var it in keyItems)
            {


                if (!PlayerData.inventory.getCurrentItems().Contains(it))//item is in our list
                {
                    containsAllKeys = false;
                    return;
                }
                else
                {
                    containsAllKeys = true;
                }
            }

            //if it contains all the keys, just remove every item in the inventory
            if (containsAllKeys)
            {
                foreach (var it in keyItems)
                {
                    PlayerData.inventory.useItem(it);
                }
                Destroy(gameObject);
            }
            /*
            if (PlayerData.inventory.getItem(keyItem))
            {
                PlayerData.inventory.useItem(keyItem);
                Destroy(gameObject);
            }
            */
        }
    }

    

}
