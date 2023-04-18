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

    //[SerializeField] private bool isFightable = false;
    //[SerializeField] private EnemyBehavior enemyBehavior;

    //[SerializeField] private bool givesItem = false;
    //private bool ableToGiveItem = false;

    //we first check if the player is in the area to talk with the npc
    //if they are and they press space, we make dialogueTriggered = true
    //once that happens the function in update checks to see if there is any current dialogue.
    //If there is none then it is the start of the conversation - show the dialogue at dialogue[0] and increment dialoguePosition
    //if there is something then we are not at the start of the conversation, and simply show it at dialogue[dialoguePosition] and incrementDialoguePosition
    //if dialoguePosition has become greater than the amount of dialogue we have, we have run out of dialogue. Delete the textbox, set dialogueTriggered to false using a coroutine and reset the dialogue to 0



    //====TODO====
    //Display face to right side and display NPC name
    //maybe stop the player from moving and only allow it once the dialogue is not triggered
    //maybe use a single textbox that appears and dissapears when we need it to
    //Have options to trigger a battle at the end of dialogue - How are we handling battles? How will they be loaded and displayed?

    void Update()
    {
        if (inRange && Input.GetKeyUp(KeyCode.Space))
        {
            if (!dialogueTriggered)
                StartDialogue();
            else
            {
                if (dialogueIndex <= dialogue.Length - 1)
                    CheckDialogue();
                else
                    EndDialogue();
            }
        }
    }

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

    private void StartDialogue()
    {
        dialogueTriggered = true;
        OverworldMovement.canMove = false;

        dialogueBox = Instantiate(textboxPrefab, transform.position - Vector3.down * -2.5f, Quaternion.identity).GetComponent<DialogueBox>();
        dialogueIndex = 0;
        dialogueBox.UpdateText(dialogue[dialogueIndex]);
    }

    private void EndDialogue()
    {
        dialogueTriggered = false;
        OverworldMovement.canMove = true;

        Destroy(dialogueBox.gameObject);
        dialogueIndex = 0;

        ////if this NPC is fightable, we then go into a fight scene
        ////with the current system, it doesnt make sense to not just have the fight cause dialogue to end here in the overworld, and just continue in the combat scene
        //if (isFightable)
        //{
        //    CombatManager.SetEnemies(enemyBehavior);
        //    SceneTransition.ChangeScene("Combat");
        //}


        ////if this player is able to give an item, check at the end of the dialogue and 
        //if (givesItem && ableToGiveItem)
        //{
        //    //communicate with player inventory to add object to player's inventory
        //    //possibly will need a reference to the object in this 
        //    ableToGiveItem = false;

        //    //maybe change the dialogue somehow to allow for the NPC to react to not having the dialogue
        //    // either through making them give the item earlier or some other way
        //}
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

        dialogueBox.UpdateText(dialogue[dialogueIndex]);
    }

    public void FightEnemy(EnemyBehavior enemyBehavior)
    {
        CombatManager.SetEnemies(enemyBehavior);
        SceneTransition.ChangeScene("Combat");
    }
}
