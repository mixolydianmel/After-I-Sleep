using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGeneration : MonoBehaviour
{
    public GameObject platform;
    public Transform currentPlatform; //set to rootPoint in editor

    public int numberOfPlatforms;
    public float minXGaps;
    public float maxXGaps;
    public float minYGaps;
    public float maxYGaps; // ! implement this to be dependent on ingredients later on

    private object[] allPlatforms;
    private List<Vector2> platformPositions;
    private List<int> duplicatePosPlatforms;
    private float XLimit = 45;
    private float YTopLimit = 40;
    private float YBotLimit = -3;

    private int posFitCounter = 0;

    void Start()
    {
        allPlatforms = Resources.LoadAll("PlaceholderPlatforms"); // ! change folder later on

        duplicatePosPlatforms = new List<int>();
        platformPositions = new List<Vector2>();

        for (int i = 0; i < numberOfPlatforms; i++)
        {
            GeneratePlatform(i);
        }

        duplicatePosPlatforms.Reverse();
        foreach(int i in duplicatePosPlatforms)
        {
            Destroy(transform.GetChild(i + 1).gameObject);
        }
    }

    void GeneratePlatform(int i)
    {
        int p = Random.Range(0, allPlatforms.Length);
        GameObject platformType = (GameObject) allPlatforms[p];

        GameObject newPlat = Instantiate(platformType, transform);
        newPlat.transform.localPosition = NewPlatformPos(i);
        newPlat.layer = 6;

        currentPlatform = newPlat.transform;
    }

    Vector2 PerfectPosition(Vector2 position, int posIndex)
    {
        float currentPosX = position.x;
        float currentPosY = position.y;

        for (int i = 0; i < platformPositions.Count; i++)
        {
            if (Mathf.Abs(currentPosX - platformPositions[i].x) < minXGaps && Mathf.Abs(currentPosY - platformPositions[i].y) < minYGaps)
            {
                posFitCounter++;
                if (posFitCounter >= 12) // if the platform cannot find a valid space in 12 tries, just copy the position of one of the conflicting platforms
                {
                    duplicatePosPlatforms.Add(i);
                    return platformPositions[i];
                }

                return NewPlatformPos(posIndex);
            }
        }

        platformPositions.Add(position);
        posFitCounter = 0;
        return position;
    }

    Vector2 NewPlatformPos(int i)
    {
        float newX = Random.Range(minXGaps, maxXGaps);
        if (newX + currentPlatform.localPosition.x > XLimit)
        {
            newX *= -1;
        }
        else if (!((newX * -1) + currentPlatform.localPosition.x < -1 * XLimit))
        {
            newX *= Random.Range(0, 2) * 2 - 1;
        }
        newX += currentPlatform.localPosition.x;

        float newY = Random.Range(minYGaps, maxYGaps);
        if (newY + currentPlatform.localPosition.y > YTopLimit)
        {
            newY *= -1;
        }
        else if (!((newY * -1) + currentPlatform.localPosition.y < -1 * YBotLimit))
        {
            newY *= Random.Range(0, 2) * 2 - 1;
        }
        newY += currentPlatform.localPosition.y;

        return PerfectPosition(new Vector2(newX, newY), i);
    }
}
