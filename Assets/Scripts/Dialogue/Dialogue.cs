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
        public enum TriggerType { NextItem, Choice, Custom }

        public Sprite sprite;
        public string text;
        public TriggerType trigger;

        public DialoguePath[] choices;

        public UnityEvent onEnd;
    }

    [System.Serializable]
    public class DialoguePath
    {
        public string text;
        public Dialogue dialogue;
    }

    public void Reset()
    {
        id = 0;

        foreach (DialogueItem item in content)
            foreach (DialoguePath path in item.choices)
                path.dialogue.Reset();
    }

    public Dialogue Trigger()
    {
        switch (Item.trigger)
        {
            case DialogueItem.TriggerType.NextItem:
                id++;
                break;
            case DialogueItem.TriggerType.Choice:
                ;
                break;
            case DialogueItem.TriggerType.Custom:
                Item.onEnd.Invoke();
                id++;
                break;
        }

        if (Item == null) return null;

        return this;
    }
}
