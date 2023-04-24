using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestDialogue : MonoBehaviour
{
    private int id;
    [SerializeField] private DialogueItemBranch[] content;

    [System.Serializable]
    public class DialogueItem
    {
        public Sprite sprite;
        public string text;
        public UnityEvent onEnd;
        public PrefSetter[] setPrefs;
    }

    [System.Serializable]
    public class DialogueItemBranch : DialogueItem
    {
        public DialoguePath[] choices;
    }

    [System.Serializable]
    public class PrefSetter
    {
        public string key;
        public string value;
    }

    [System.Serializable]
    public class DialoguePath
    {
        private int id;
        [SerializeField] private string text;
        [SerializeField] private DialogueItem[] content;
    }
}
