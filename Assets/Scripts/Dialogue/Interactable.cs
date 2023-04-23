using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject textboxPrefab;
    private DialogueBox dialogueBox;
    
    [SerializeField] private Dialogue dialogue;
    private Dialogue currentDialogue;

    private bool dialogueTriggered = false;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void TriggerInteraction()
    {
        if (!dialogueTriggered)
            StartDialogue();
        else
            CheckDialogue();
    }

    private void StartDialogue()
    {
        dialogueTriggered = true;
        OverworldMovement.canMove = false;

        currentDialogue = dialogue;
        currentDialogue.Reset();

        dialogueBox = Instantiate(textboxPrefab, transform.position - Vector3.down * -2.5f, Quaternion.identity).GetComponent<DialogueBox>();
        dialogueBox.UpdateText(currentDialogue.Item);
    }


    private void CheckDialogue()
    {
        currentDialogue = currentDialogue.Trigger();

        if (currentDialogue == null || currentDialogue.Item == null)
            EndDialogue();
        else
            dialogueBox.UpdateText(currentDialogue.Item);
    }

    private void EndDialogue()
    {
        dialogueTriggered = false;
        OverworldMovement.canMove = true;

        Destroy(dialogueBox.gameObject);
    }

    public void FightEnemy(EnemyBehavior enemyBehavior)
    {
        CombatManager.SetEnemy(enemyBehavior);
        SceneTransition.ChangeScene("Combat");
    }
}
