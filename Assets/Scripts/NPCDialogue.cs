using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private GameObject textboxPrefab;

    [System.Serializable]
    public class DialogueItem
    {
        public enum TriggerType { None, SkipTo, Custom }

        public Sprite sprite;
        public string text;
        public TriggerType trigger;
        public int skipToIndex;
        public UnityEvent onEnd;
    }
    [SerializeField] public DialogueItem[] dialogue;

    private DialogueBox dialogueBox;
    
    private bool dialogueTriggered = false;
    private int dialogueIndex = 0;
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
        dialogueIndex = 0;
        OverworldMovement.canMove = false;

        dialogueBox = Instantiate(textboxPrefab, transform.position - Vector3.down * -2.5f, Quaternion.identity).GetComponent<DialogueBox>();
        dialogueBox.UpdateText(dialogue[dialogueIndex]);
    }

    private void CheckDialogue()
    {
        switch (dialogue[dialogueIndex].trigger)
        {
            case DialogueItem.TriggerType.None:
                dialogueIndex++;
                break;
            case DialogueItem.TriggerType.SkipTo:
                dialogueIndex = dialogue[dialogueIndex].skipToIndex;
                break;
            case DialogueItem.TriggerType.Custom:
                dialogue[dialogueIndex].onEnd.Invoke();
                break;
        }

        if (dialogueIndex >= dialogue.Length)
            EndDialogue();
        else
            dialogueBox.UpdateText(dialogue[dialogueIndex]);
    }

    private void EndDialogue()
    {
        dialogueTriggered = false;
        OverworldMovement.canMove = true;

        Destroy(dialogueBox.gameObject);
    }

    public void FightEnemy(EnemyBehavior enemyBehavior)
    {
        CombatManager.SetEnemies(enemyBehavior);
        SceneTransition.ChangeScene("Combat");
    }
}
