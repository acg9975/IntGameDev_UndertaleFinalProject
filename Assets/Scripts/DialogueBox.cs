using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogueBox : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI Text;
    string currentText;
    char[] separatedText;

    [SerializeField]
    Image img;
    Sprite spr;


    public void ChangeText(string msg, Sprite spr)
    {
        img.sprite = spr;
        /*
        for (int i = 0; i < msg.Length; i++)
        {
            Debug.Log(msg[i]);
            separatedText[i] = msg[i];
        }
        */
        this.currentText = msg;
        Text.text = currentText;


    }

    //dialogue has to be created line by line
    //separate the msg into an array of single character strings and add it into a currentString string to be displayed
    



}
