using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private GameObject textboxPrefab;

    [System.Serializable]
    public class DialogueItem
    {
        public Sprite sprite;
        public string text;
    }
    [SerializeField] public DialogueItem[] dialogue;

    private DialogueBox db;
    
    private bool dialogueTriggered = false;
    private int dialogueIndex = 0;
    private bool talkable = true;

    [SerializeField]
    private bool isFightable = false;

    [SerializeField]
    private bool givesItem = false;
    private bool ableToGiveItem = false;

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
        if (dialogueTriggered)
        {
            //disable player movement 
            OverworldMovement.canMove = false;
            Debug.Log(OverworldMovement.canMove);
            if (db == null)
            {
                //if this does not exist, create it
                db = Instantiate(textboxPrefab, transform.position - Vector3.down * -2.5f, Quaternion.identity).GetComponent<DialogueBox>();
                db.ChangeText(dialogue[0].text, dialogue[0].sprite);
                dialogueIndex++;
            }
            else if (db != null)
            {
                if (Input.GetKeyDown(KeyCode.Space) && dialogueIndex < dialogue.Length)
                {
                    //if we have a textbox already, simply show the dialogue at the position we set and increment position counter
                    db.ChangeText(dialogue[dialogueIndex].text, dialogue[dialogueIndex].sprite);
                    dialogueIndex++;
                }
                else if (Input.GetKeyDown(KeyCode.Space) && dialogueIndex >= dialogue.Length)
                {
                    //delete textbox. set dialogue triggered to false. reset dialogue position
                    Destroy(db.gameObject);
                    dialogueTriggered = false;
                    StartCoroutine(canTriggerDialogue());

                    dialogueIndex = 0;

                    //if this NPC is fightable, we then go into a fight scene
                    //with the current system, it doesnt make sense to not just have the fight cause dialogue to end here in the overworld, and just continue in the combat scene
                    if (isFightable)
                    {
                        //Transition to combat manager - combat manager may need to be static?
                    }
                    

                    //if this player is able to give an item, check at the end of the dialogue and 
                    if (givesItem && ableToGiveItem)
                    {
                        //communicate with player inventory to add object to player's inventory
                        //possibly will need a reference to the object in this 
                        ableToGiveItem = false;

                        //maybe change the dialogue somehow to allow for the NPC to react to not having the dialogue
                        // either through making them give the item earlier or some other way
                    }

                    
                }

                

            }
        }
        else
        {
            OverworldMovement.canMove = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && talkable)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !dialogueTriggered)
            {
                //spawn dialogue boxes
                dialogueTriggered = true;
                OverworldMovement.canMove = false;
                talkable = false;
            }
        }
    }

    IEnumerator canTriggerDialogue()
    {
        yield return new WaitForSeconds(1f);
        talkable = true;
    }
}
