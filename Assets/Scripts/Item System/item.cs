using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item", menuName ="InventorySystem/Item")]
public class item : ScriptableObject
{
    public string itemName;
    public string itemDescription;

    public int amountHealed
    {
        get
        {
            return amountHealed;
        }
    }

    public bool isWeapon;



}
