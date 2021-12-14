using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector2 velocity;

    public float smoothTimeY;
    public float smoothTimeX;

    public float yHeight;

    private GameObject player, level;


    // Use this for initialization
    void Start()
    {
        Camera.main.orthographicSize = 5;

        player = GameObject.FindGameObjectWithTag("Player");
        level = GameObject.Find("Platforms");
    }


    void FixedUpdate()
    {

        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y + yHeight, ref velocity.y, smoothTimeY);

        int onScreen = 0;
        for (int i = 0; i < level.transform.childCount; i++) //checks how many platforms are on screen
        {
            Transform check = level.transform.GetChild(i);

            if (Camera.main.WorldToScreenPoint(check.position).x <= Camera.main.pixelWidth
                && Camera.main.WorldToScreenPoint(check.position).x >= 0
                && Camera.main.WorldToScreenPoint(check.position).y <= Camera.main.pixelHeight
                && Camera.main.WorldToScreenPoint(check.position).y >= 0) {
                onScreen++;
            }
        }

        if (player.GetComponent<PlayerController>().getGrounded()) { //zoom out/in camera to fit platforms while player is grounded
            if (onScreen < 3)
                Camera.main.orthographicSize += 0.02f;
            if (onScreen > 4)
                Camera.main.orthographicSize -= 0.01f;
        }

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 2, 8);

        transform.position = new Vector3(posX, posY, transform.position.z);
    }
}