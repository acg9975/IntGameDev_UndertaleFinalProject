using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject textboxPrefab;
    private DialogueBox dialogueBox;
    
    [SerializeField] private Dialogue dialogue;

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

        dialogue.Reset();

        dialogueBox = Instantiate(textboxPrefab, transform.position - Vector3.down * -2.5f, Quaternion.identity).GetComponent<DialogueBox>();
        dialogueBox.UpdateText(dialogue.Item);
    }


    private void CheckDialogue()
    {
        dialogue.Trigger();

        if (dialogue.Item == null)
            EndDialogue();
        else
            dialogueBox.UpdateText(dialogue.Item);
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

        //save player position by just getting the player position and calling it a day
        Vector3 playerPos = GameObject.Find("Player").GetComponent<Transform>().position;

        PlayerPrefs.SetFloat("lastOverworldX", playerPos.x);
        PlayerPrefs.SetFloat("lastOverworldY", playerPos.y);
        Debug.Log("x " + playerPos.x + " y " + playerPos.y);
    }
}
