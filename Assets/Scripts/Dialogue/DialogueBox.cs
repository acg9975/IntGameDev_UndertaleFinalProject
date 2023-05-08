using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private float textRevealTick = 0.05f;
    [SerializeField] private string textRevealSound;
    [SerializeField] private float textSoundTick = 0.05f;

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

    private Dialogue.DialogueItem item;
    private string currentText;

    private IEnumerator revealRoutine;
    public bool isRevealing = false;

    private void Awake()
    {
        selectedIndex = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            selectedIndex--;
        if (Input.GetKeyDown(KeyCode.D))
            selectedIndex++;

        if (selectedIndex < 0)
            selectedIndex = 1;
        if (selectedIndex > 1)
            selectedIndex = 0;

        if (option1Marker != null) option1Marker.SetActive(selectedIndex == 0);
        if (option2Marker != null) option2Marker.SetActive(selectedIndex == 1);
    }

    public void Trigger(Dialogue.DialogueItem newItem = null)
    {
        if (isRevealing)
        {
            if (revealRoutine != null)
                StopCoroutine(revealRoutine);

            isRevealing = false;
            currentText = item.text;

            UpdateText();
        }
        else
        {
            item = newItem;
            SetContent();

            revealRoutine = RevealRoutine();
            StartCoroutine(revealRoutine);
        }
    }

    private void SetContent()
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
                }
                else
                {
                    characterGroup.SetActive(true);

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

    private void UpdateText()
    {
        if (item.type == Dialogue.DialogueItem.DialogueItemType.Standard)
        {
            if (item.sprite == null)
            {
                genericText.text = currentText;
            }
            else
            {
                characterText.text = currentText;
            }
        }
    }

    private IEnumerator RevealRoutine()
    {
        isRevealing = true;
        currentText = "";

        float textSoundTimer = 0f;

        for (int i = 0; i < item.text.Length; i++)
        {
            currentText += item.text[i];
            UpdateText();

            if (textSoundTimer >= textSoundTick)
            {
                textSoundTimer = 0f;
                SoundManager.PlayMisc(textRevealSound);
            }

            yield return new WaitForSeconds(textRevealTick);
            textSoundTimer += textRevealTick;
        }

        isRevealing = false;
    }
}
