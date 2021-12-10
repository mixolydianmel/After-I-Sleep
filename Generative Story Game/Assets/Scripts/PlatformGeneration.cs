using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGeneration : MonoBehaviour
{
    public Transform currentPlatform; // set to rootPoint in editor
    public GameObject background; // set to Background in editor
    public GameObject audioSource; // set to Main Camera in editor

    public int coreWorld;
    public int numberOfPlatforms;
    public float minXGaps;
    public float maxXGaps;
    public float minYGaps;
    public float maxYGaps; // ! implement this to be dependent on ingredients later on

    private object[] allPlatforms;
    private object[] altPlatforms; // only used for Bridge World
    private Rect bridgeWorldDarkZone; // only used for Bridge World
    private List<Vector2> platformPositions;
    private List<int> duplicatePosPlatforms;
    private float XLimit = 45;
    private float YTopLimit = 40;
    private float YBotLimit = -3;

    private int posFitCounter = 0;

    private string[] worlds = { "BridgeWorld", "CloudWorld", "SpaceWorld", "UnderwaterWorld" };

    void Start()
    {
        allPlatforms = Resources.LoadAll(worlds[coreWorld], typeof(GameObject));
        Object[] backgrounds = Resources.LoadAll(worlds[coreWorld], typeof(Sprite));
        Object[] songs = Resources.LoadAll(worlds[coreWorld], typeof(AudioClip));

        // select correct background
        background.GetComponent<SpriteRenderer>().sprite = (Sprite) backgrounds[0];

        // select correct song
        audioSource.GetComponent<AudioSource>().clip = (AudioClip) songs[0];
        audioSource.GetComponent<AudioSource>().Play();

        // special requirements for Bridge World platforms
        if (coreWorld == 0)
        {
            object[] newAllPlatforms = new object[allPlatforms.Length / 2];
            altPlatforms = new object[allPlatforms.Length / 2];

            int i1 = 0;
            int i2 = 0;
            foreach(GameObject p in allPlatforms)
            {
                if (p.tag == "altPlatform")
                {
                    altPlatforms[i1] = p;
                    i1++;
                }
                else
                {
                    newAllPlatforms[i2] = p;
                    i2++;
                }
            }

            allPlatforms = newAllPlatforms;
            bridgeWorldDarkZone = new Rect(18, -5, 26, 48);
        }

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

        // special instructions for Bridge World platforms
        Canvas.ForceUpdateCanvases();
        if (coreWorld == 0 && bridgeWorldDarkZone.Contains(newPlat.transform.localPosition))
        {
            int cCount = newPlat.transform.childCount;
            for (int c = 0; c < cCount; c++)
            {
                Destroy(newPlat.transform.GetChild(c).gameObject);
            }
            for (int c = 0; c < cCount; c++)
            {
                Instantiate(((GameObject)altPlatforms[p]).transform.GetChild(c), newPlat.transform);
            }
        }

        newPlat.layer = 6;

        currentPlatform = newPlat.transform;
    }

    Vector2 PerfectPosition(Vector2 position, int posIndex)
    {
        float currentPosX = position.x;
        float currentPosY = position.y;

        for (int i = platformPositions.Count - 1; i >= 0; i--)
        {
            if (Mathf.Abs(currentPosX - platformPositions[i].x) < minXGaps && Mathf.Abs(currentPosY - platformPositions[i].y) < minYGaps)
            {
                posFitCounter++;
                if (posFitCounter >= 12) // if the platform cannot find a valid space in 12 tries, just copy the position of one of the conflicting platforms
                {
                    duplicatePosPlatforms.Add(i);
                    platformPositions.Add(platformPositions[i]);
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
