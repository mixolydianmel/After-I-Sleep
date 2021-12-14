using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mika : MonoBehaviour
{
    public GameObject dialogueCanvas;
    private bool activated = false;

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Player" && activated == false)
        {
            dialogueCanvas.SetActive(true);
            dialogueCanvas.SendMessage("BeginDialogue", GameManager.day, SendMessageOptions.RequireReceiver);
            activated = true;
            GameManager.day++;
        }
    }
}
