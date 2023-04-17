using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatMenuNavigator : MonoBehaviour
{
    public static CombatMenuNavigator instance;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject attackBox;
    [SerializeField] private GameObject playerAttackBox;


    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;


    private void Awake()
    {
        instance = this;
    }

    public void UpdateCombatUI()
    {
        CombatManager.CombatMode combatMode = CombatManager.instance.combatMode;

        dialogueBox.SetActive(combatMode == CombatManager.CombatMode.Menu);
        attackBox.SetActive(combatMode == CombatManager.CombatMode.Attack);
        playerAttackBox.SetActive(combatMode == CombatManager.CombatMode.PlayerAttack);

        healthText.text = PlayerData.Health + "/" + PlayerData.MaxHealth;
        enemyHealthText.text = CombatManager.enemyHealth + "/" + 10;
    }
}
