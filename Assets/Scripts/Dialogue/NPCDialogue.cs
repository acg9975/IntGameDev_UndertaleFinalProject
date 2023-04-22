using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Dialogue;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private GameObject textboxPrefab;
    private DialogueBox dialogueBox;
    
    [SerializeField] private Dialogue dialogue;
    private Dialogue currentDialogue;

    private bool dialogueTriggered = false;
    private bool inRange = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            inRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            inRange = false;
    }

    void Update()
    {
        if (inRange && Input.GetKeyUp(KeyCode.Space))
        {
            if (!dialogueTriggered)
                StartDialogue();
            else
                CheckDialogue();
        }
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

        if (currentDialogue == null)
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
