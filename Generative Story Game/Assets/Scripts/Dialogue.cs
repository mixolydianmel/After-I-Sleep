using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public GameObject writtenText;

    private string[] currentBook;
    private string message;
    private int currentLine;
    private float typeTime;
    private int index;
    private bool typing = false;
    private bool finalText;

    void BeginDialogue(int day)
    {
        int totalDays = Resources.LoadAll("Dialogue").Length;
        finalText = day < totalDays - 1 ? false : true;

        TextAsset file = Resources.Load("Dialogue/day" + day) as TextAsset;
        string readableFile = file.text;
        currentBook = readableFile.Split('\n');

        TypeThis(0);
    }

    private void TypeThis(int newIndex)
    {
        index = newIndex;
        message = currentBook[index];
        typeTime = 0.05f;

        /*for (int i = 55; i < message.Length; i--)
        {
            if (message[i] == ' ')
            {
                message = message.Remove(i, 1);
                message = message.Insert(i, "\n");
                i += 55;
            }
        }*/

        if (typing == false)
        {
            writtenText.GetComponent<TextMeshProUGUI>().text = "";
            StartCoroutine(TypeText());
        }
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) && typing)
        {
            StopAllCoroutines();
            writtenText.GetComponent<TextMeshProUGUI>().text = message;
            typing = false;
        }
        else if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) && !typing)
        {
            if(index < currentBook.Length - 1)
            {
                TypeThis(index + 1);
            }
            else
            {
                if (finalText)
                {
                    GameObject.Find("GameManager").SendMessage("TitleScreen");
                }
                else
                {
                    GameObject.Find("GameManager").SendMessage("CreationScreen");
                }
            }
        }
    }

    //CREDIT TO http://wiki.unity3d.com/index.php/AutoType?_ga=2.28672252.1760231856.1570400781-1881874195.1512603304
    IEnumerator TypeText()
    {
        typing = true;

        int lineCounter = 0;
        float RealTypeTime;

        foreach (char c in message.ToCharArray())
        {
            writtenText.GetComponent<TextMeshProUGUI>().text += c;

            if (c == ',' || c == '.' || c == '?' || c == '!' || c == ':' || c == '(')
            {
                RealTypeTime = typeTime;
            }
            else
            {
                RealTypeTime = typeTime / 2;
                if (c == '\n')
                {
                    RealTypeTime = 0;
                    lineCounter++;
                }
            }
            yield return 0;
            yield return Wait(RealTypeTime);
        }
        typing = false;
    }

    IEnumerator Wait(float waitTime)
    {
        float counter = 0;

        while (counter < waitTime)
        {
            //Increment Timer until counter >= waitTime
            counter += Time.unscaledDeltaTime;
            //Wait for a frame so that Unity doesn't freeze
            yield return null;
        }
    }
}
