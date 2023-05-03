using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Dialogue;

public class InteractableStateController : MonoBehaviour
{
    [System.Serializable]
    struct InteractableState
    {
        public string label;
        public Preference[] conditions;
        public GameObject gameObject;
    }

    [SerializeField] private GameObject defaultState;
    [SerializeField] private InteractableState[] states;

    private void Start()
    {
        UpdateState();
    }

    public void UpdateState()
    {
        defaultState.SetActive(false);
        foreach (InteractableState state in states)
            state.gameObject.SetActive(false);

        bool foundState = false;
        foreach (InteractableState state in states)
        {
            bool active = true;

            foreach (Preference condition in state.conditions)
                if (PlayerPrefs.GetString(condition.parameter) != condition.value)
                    active = false;

            if (active)
            {
                foundState = true;
                state.gameObject.SetActive(true);
                break;
            }
        }

        if (!foundState)
            defaultState.SetActive(true);
    }
}
