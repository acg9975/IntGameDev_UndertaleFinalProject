using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatMenuNavigator : MonoBehaviour
{
    public static CombatMenuNavigator instance;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject playerAttackBox;
    [SerializeField] private GameObject playerDefendBox;


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
        playerAttackBox.SetActive(combatMode == CombatManager.CombatMode.PlayerAttack);
        playerDefendBox.SetActive(combatMode == CombatManager.CombatMode.PlayerDefend);

        healthText.text = PlayerData.Health + "/" + PlayerData.MaxHealth;
        enemyHealthText.text = CombatManager.Enemy.Health + "/" + CombatManager.Enemy.MaxHealth;
    }
}
