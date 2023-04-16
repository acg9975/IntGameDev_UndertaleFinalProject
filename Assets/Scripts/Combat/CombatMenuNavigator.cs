using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMenuNavigator : MonoBehaviour
{
    public static CombatMenuNavigator instance;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject attackBox;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateCombatUI()
    {
        CombatManager.CombatMode combatMode = CombatManager.instance.combatMode;

        dialogueBox.SetActive(combatMode == CombatManager.CombatMode.Menu);
        attackBox.SetActive(combatMode == CombatManager.CombatMode.Attack);
    }
}
