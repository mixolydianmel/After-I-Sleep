using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TitleScreenButtons : MonoBehaviour
{
    void Clicked()
    {
        if (name == "Begin")
        {
            Debug.Log("Start Game");
        }
        if (name == "Exit")
        {
            Application.Quit();
        }
    }
}
