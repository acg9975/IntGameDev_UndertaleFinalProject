using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [Header("Generic Dialogue")]
    [SerializeField] private GameObject genericGroup;
    [SerializeField] private TextMeshProUGUI genericText;

    [Header("Character Dialogue")]
    [SerializeField] private GameObject characterGroup;
    [SerializeField] private TextMeshProUGUI characterText;
    [SerializeField] private Image characterImage;

    [Header("Decision")]
    [SerializeField] private GameObject decisionGroup;
    [SerializeField] private TextMeshProUGUI option1Text;
    [SerializeField] private GameObject option1Marker;
    [SerializeField] private TextMeshProUGUI option2Text;
    [SerializeField] private GameObject option2Marker;

    public static int selectedIndex = 0;

    private string currentText;
    private char[] separatedText;

    private Sprite currentSprite;

    private void Awake()
    {
        selectedIndex = 0;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
            selectedIndex--;
        if (Input.GetKeyUp(KeyCode.D))
            selectedIndex++;

        if (selectedIndex < 0)
            selectedIndex = 1;
        if (selectedIndex > 1)
            selectedIndex = 0;

        if (option1Marker != null) option1Marker.SetActive(selectedIndex == 0);
        if (option2Marker != null) option2Marker.SetActive(selectedIndex == 1);
    }

    public void UpdateText(Dialogue.DialogueItem item)
    {
        genericGroup.SetActive(false);
        characterGroup.SetActive(false);
        decisionGroup.SetActive(false);

        switch (item.type)
        {
            case Dialogue.DialogueItem.DialogueItemType.Standard:

                if (item.sprite == null)
                {
                    genericGroup.SetActive(true);

                    genericText.text = item.text;
                }
                else
                {
                    characterGroup.SetActive(true);

                    characterText.text = item.text;
                    characterImage.sprite = item.sprite;
                }

                break;
            case Dialogue.DialogueItem.DialogueItemType.Decision:

                decisionGroup.SetActive(true);

                option1Text.text = item.choices.option1.text;
                option2Text.text = item.choices.option2.text;

                break;
        }
    }

    //dialogue has to be created line by line
    //separate the msg into an array of single character strings and add it into a currentString string to be displayed
    



}
