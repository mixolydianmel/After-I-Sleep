using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGeneration : MonoBehaviour
{
    public GameObject platform;
    public Transform currentPlatform; //set to rootPoint in editor

    public float minXGaps;
    public float maxXGaps;
    public float minYGaps;
    public float maxYGaps; // ! implement this to be dependent on ingredients later on

    private object[] allPlatforms;

    private float XLimit = 40;
    private float YTopLimit = 30;
    private float YBotLimit = -3;

    void Start()
    {
        allPlatforms = Resources.LoadAll("PlaceholderPlatforms"); // ! change folder later on

        for (int i = 0; i < 8; i++)
        {
            GeneratePlatform();
        }
    }

    void GeneratePlatform()
    {
        int p = Random.Range(0, allPlatforms.Length);
        Debug.Log(p);
        Debug.Log(allPlatforms[p]);
        GameObject platformType = (GameObject) allPlatforms[p];

        float newX = Random.Range(minXGaps, maxXGaps);
        if (newX + currentPlatform.localPosition.x > XLimit)
        {
            newX *= -1;
        }
        else if (!(newX + currentPlatform.localPosition.x < -1 * XLimit))
        {
            newX *= Random.Range(0, 2) * 2 - 1;
        }
        newX += currentPlatform.localPosition.x;

        float newY = Random.Range(minYGaps, maxYGaps);
        if (newY + currentPlatform.localPosition.y > YTopLimit)
        {
            newY *= -1;
        }
        else if (!(newY + currentPlatform.localPosition.y < -1 * YBotLimit))
        {
            newY *= Random.Range(0, 2) * 2 - 1;
        }
        newY += currentPlatform.localPosition.y;

        GameObject newPlat = Instantiate(platformType, transform);
        newPlat.transform.localPosition = new Vector2(newX, newY);
        newPlat.layer = 6;

        currentPlatform = newPlat.transform;
    }
}
