using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestDialogue : MonoBehaviour
{
    public enum DialogueItemType { Standard, Decision }

    private int id;
    [SerializeField] private DialogueItem[] content;

    [System.Serializable]
    public class DialogueItem
    {
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
}
