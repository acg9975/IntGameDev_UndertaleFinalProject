using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] [TextArea(1, 4)] private string[] dialogue;
    [SerializeField] private GameObject textboxPrefab;

    private DialogueBox db;
    
    private bool dialogueTriggered = false;
    private int dialoguePosition = 0;
    private bool talkable = true;


    [SerializeField]
    Sprite[] characterFaces;

    Sprite currentFace;

    //currently have two variables to control when we change their sprite
    [SerializeField][Tooltip("At dialogue position x, change character sprite to sprite at index Y")]
    Vector2Int[] characterSpritePair;//x = pos to change sprite, y = characterFaces pos to change to

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

            if (db == null)
            {
                //Debug.Log("Created db");
                //if this does not exist, create it
                db = Instantiate(textboxPrefab, transform.position - Vector3.down * -2.5f, Quaternion.identity).GetComponent<DialogueBox>();
                db.ChangeText(dialogue[0], currentFace);
                dialoguePosition++;
            }
            else if (db != null)
            {




                if (Input.GetKeyDown(KeyCode.Space) && dialoguePosition < dialogue.Length)
                {
                    //check if we are at the position to change the character sprite based on the dialogue position.
                    //if we want to change it to something in line 4, we simply make posToChangeSprite to 4

                    for (int i = 0; i < characterSpritePair.Length; i++)
                    {
                        //Debug.Log("DP: " + dialoguePosition + " cSP pos" + i + " cSP x: " + characterSpritePair[i].x);
                        if (dialoguePosition == characterSpritePair[i].x)
                        {
                            currentFace = characterFaces[characterSpritePair[i].y];
                            Debug.Log("Changed to character sprite at " + characterSpritePair[i].y);
                        }
                    }


                    //Debug.Log("CurrentPlace is " + dialoguePosition);
                    //if we have a textbox already, simply show the dialogue at the position we set and increment position counter
                    db.ChangeText(dialogue[dialoguePosition], currentFace);
                    dialoguePosition++;

                    //Debug.Log("Advancing to " + dialoguePosition);

                }
                else if (Input.GetKeyDown(KeyCode.Space) && dialoguePosition >= dialogue.Length)
                {
                    //delete textbox. set dialogue triggered to false. reset dialogue position
                    Destroy(db.gameObject);
                    dialogueTriggered = false;
                    StartCoroutine(canTriggerDialogue());

                    dialoguePosition = 0;
                    //Debug.Log("Reset dialogue " + dialoguePosition);

                }

            }


        }


        
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player") && talkable)
        {
            //NOTE - getkeydown is kinda finnicky for me. May be my spacebar but I find usng this to be easier to detect keypresses
            if (Input.GetKey(KeyCode.Space) && !dialogueTriggered)
            {
                //spawn dialogue boxes
                dialogueTriggered = true;
                talkable = false;
            }
            //Debug.Log("player is here");
        }

    }

    IEnumerator canTriggerDialogue()
    {
        yield return new WaitForSeconds(1f);
        talkable = true;

        //Debug.Log("Player is able to talk again");

    }

}
