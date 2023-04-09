using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogueBox : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI Text;
    string currentText;



    public void ChangeText(string msg)
    {
        currentText = msg;
        Text.text = currentText;
    }




}
