using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item", menuName ="InventorySystem/Item")]
public class item : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    [SerializeField]
    private int healingAmount;

    public int HealingAmount
    {
        get
        {
            return healingAmount;
        }
        set
        {
            healingAmount = Mathf.Clamp(value, 0, 155);
        }

    }
}
