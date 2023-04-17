using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [Header("Generic Dialogue")]
    [SerializeField] private GameObject genericDialogueBox;
    [SerializeField] private TextMeshProUGUI genericText;

    [Header("Character Dialogue")]
    [SerializeField] private GameObject characterDialogueBox;
    [SerializeField] private TextMeshProUGUI characterText;
    [SerializeField] private Image characterImage;

    private string currentText;
    private char[] separatedText;

    private Sprite currentSprite;

    public void UpdateText(NPCDialogue.DialogueItem dialogueItem)
    {
        UpdateText(dialogueItem.text, dialogueItem.sprite);
    }


    public void UpdateText(string text, Sprite sprite)
    {
        currentText = text;
        currentSprite = sprite;

        genericDialogueBox.SetActive(sprite == null);
        characterDialogueBox.SetActive(sprite != null);

        genericText.text = text;
        characterText.text = text;
        characterImage.sprite = sprite;
    }

    //dialogue has to be created line by line
    //separate the msg into an array of single character strings and add it into a currentString string to be displayed
    



}
