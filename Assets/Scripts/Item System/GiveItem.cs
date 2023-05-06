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

}
