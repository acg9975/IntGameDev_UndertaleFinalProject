using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveItem : MonoBehaviour
{
    [SerializeField]
    private item key;

    public void giveKey()
    {
        PlayerData.inventory.addItem(key);
    }

}
