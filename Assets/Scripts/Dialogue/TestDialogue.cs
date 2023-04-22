using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : MonoBehaviour
{
    [System.Serializable]
    public class DialogueItem
    {
        public enum TriggerType { None, SkipTo, Custom }

        public Sprite sprite;
        public string text;

        [System.Serializable]
        public struct DialoguePath
        {
            public DialogueItem[] dialogue;
        }
        public DialoguePath[] choicePaths;
    }
    [SerializeField] public DialogueItem[] dialogue;
}
