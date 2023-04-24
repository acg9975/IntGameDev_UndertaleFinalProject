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

        public Condition[] conditions;
        public DialogueItemType type;

        public Sprite sprite;
        public string text;

        public UnityEvent onEnd;
        public Choices choices;
    }

    [System.Serializable]
    public class Condition
    {
        public string parameter;
        public string value;
    }

    [System.Serializable]
    public class Choices
    {
        public string parameter;
        public Option option1;
        public Option option2;
    }

    [System.Serializable]
    public class Option
    {
        public string value;
        public string text;
        public UnityEvent onEnd;
    }

    public void Reset()
    {
        id = 0;
    }

    public void Trigger(int selectedOption = 0)
    {
        switch (Item.type)
        {
            case DialogueItem.DialogueItemType.Standard:
                Item.onEnd?.Invoke();
                break;
            case DialogueItem.DialogueItemType.Decision:
                SelectOption(selectedOption);
                break;
        }

        FindNextItem();
    }

    private void SelectOption(int selectedOption)
    {
        Option option = (selectedOption == 0) ? Item.choices.option1 : Item.choices.option2;

        PlayerPrefs.SetString(Item.choices.parameter, option.value);
        option.onEnd?.Invoke();
    }

    private void FindNextItem()
    {
        
    }

    private bool CheckConditions()
    {
        foreach (Condition condition in Item.conditions)
            if (PlayerPrefs.GetString(condition.parameter) != condition.value)
                return false;

        return true;
    }
}
