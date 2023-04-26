using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    private int id;
    [SerializeField] private DialogueItem[] content;

    public DialogueItem Item
    {
        get
        {
            if (id >= content.Length) return null;
            else return content[id];
        }
    }

    [System.Serializable]
    public class DialogueItem
    {
        public enum DialogueItemType { Standard, Decision }

        public Preference[] conditions;
        public DialogueItemType type;

        public Sprite sprite;
        public string text;

        public UnityEvent onEnd;
        public Choices choices;
    }

    [System.Serializable]
    public class Choices
    {
        public Option option1;
        public Option option2;
    }

    [System.Serializable]
    public class Option
    {
        public string text;
        public Preference[] setValues;
        public UnityEvent onEnd;
    }

    [System.Serializable]
    public class Preference
    {
        public string parameter;
        public string value;
    }

    public void Reset()
    {
        id = 0;
    }

    public void Trigger()
    {
        switch (Item.type)
        {
            case DialogueItem.DialogueItemType.Standard:
                Item.onEnd?.Invoke();
                break;
            case DialogueItem.DialogueItemType.Decision:
                SelectOption(DialogueBox.selectedIndex);
                break;
        }

        FindNextItem();
    }

    private void SelectOption(int selectedOption)
    {
        Option option = (selectedOption == 0) ? Item.choices.option1 : Item.choices.option2;

        foreach (Preference pref in option.setValues)
            PlayerPrefs.SetString(pref.parameter, pref.value);

        option.onEnd?.Invoke();
    }

    private void FindNextItem()
    {
        while (true)
        {
            id++;

            if (Item == null)
                break;

            bool check = CheckConditions();
            if (check)
                break;
        }
    }

    private bool CheckConditions()
    {
        foreach (Preference condition in Item.conditions)
            if (PlayerPrefs.GetString(condition.parameter) != condition.value)
                return false;

        return true;
    }
}
