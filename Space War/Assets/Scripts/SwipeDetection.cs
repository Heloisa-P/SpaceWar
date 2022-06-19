using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public Player player;
    public int pixelDistToDetect = 20;

    private Vector2 touchPosition;
    private bool fingerDown;

    private void Update()
    {
        //if the finger is pressing, if it's touching more than once and if it's the first touch
        if(!fingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            touchPosition = Input.touches[0].position;
            fingerDown = true;
        }

        if(fingerDown)
        {
            if (Input.touches[0].position.y >= touchPosition.y + pixelDistToDetect)
            {
                fingerDown = false;
                player.goUp();
            }
            else if(Input.touches[0].position.y <= touchPosition.y - pixelDistToDetect)
            {
                fingerDown = false;
                player.goDown();
            }
        }

        if(fingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            fingerDown = false;
        }
    }
}
