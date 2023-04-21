using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    private static int maxHealth = 10;
    private static int health = 10;

    public static int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = Mathf.Max(0, value);
        }
    }

    public static int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
        }
    }

    public static bool IsAlive { get { return health > 0; } }

    public static InventoryManager inventory = new InventoryManager();


    public class InventoryManager
    {
        //should be able to hold a list of scriptable objects that represent items the player has in game
        //player should be able to access this at any point in the overworld to see what items they have

        List<item> currentItems = new List<item>();

        public List<item> getCurrentItems()
        {
            return currentItems;
        }

        public item getItem(item item)
        {
            if (currentItems.Contains(item))
            {
                return item;

            }
            else
            {
                return null;
            }

        }
        
        public void useItem(item chosenItem)
        {
            //applies item's effects and destroys item
            if (currentItems.Contains(chosenItem))
            {
                int i = currentItems.IndexOf(chosenItem);
                Health += currentItems[i].HealingAmount;
                currentItems.Remove(chosenItem);
            }
        }

        public void addItem(item item)
        {
            if (currentItems.Capacity < 7)
            {
                currentItems.Add(item);
                Debug.Log("Item added");
            }
        }

        public void removeItem(item item)
        {
            //simply destroys item
            if (currentItems.Contains(item))
            {
                currentItems.Remove(item);
            }
        }
        public item findItem(int position)
        {
            return currentItems.ToArray()[position];
        }
            


        }
    }
