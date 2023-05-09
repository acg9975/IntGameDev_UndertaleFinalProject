using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveItem : MonoBehaviour
{
    [SerializeField]
    private item[] keys;

    public void giveKey()
    {
        foreach (var key in keys)
        {
            PlayerData.inventory.addItem(key);
        }
        inventoryUIManager.instance.itemAddedNotif();
    }

    public void increaseMaxHealth()
    {
        PlayerData.MaxHealth = 20;
        PlayerData.Health = 20;//max health and current health is now set to 20 after calling this
        Debug.Log("Setting max health to 20");
    }


}
