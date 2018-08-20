using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementMouse : CameraMovement
{
    private bool usedWithTouch;

    private void FixedUpdate ()
    {
        if(Input.touchCount != 0)
        {
            usedWithTouch = true;
        }
        else if(Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            usedWithTouch = false;
        }

        if(usedWithTouch)
        {
            return;
        }

        ZoomCamera(Input.GetAxis("Mouse ScrollWheel") * (-50));

        if(Input.mousePosition.x > (float) 80 * Screen.width / 100)
        {
            //Mouse is over Overlay
            return;
        }

        Vector3 movement = new Vector3();

        if (Input.mousePosition.x < (float) 5 * Screen.width / 100 || Input.GetAxis("Horizontal") == -1)
        {
            movement -= new Vector3(5, 0, 0);
        }
        else if (Input.mousePosition.x > (float) 75 * Screen.width / 100 || Input.GetAxis("Horizontal") == 1)
        {
            movement += new Vector3(5, 0, 0);
        }

        if (Input.mousePosition.y < (float) 5 * Screen.height / 100 || Input.GetAxis("Vertical") == -1)
        {
            movement -= new Vector3(0, 5, 0);
        }
        else if (Input.mousePosition.y > (float) 95 * Screen.height / 100 || Input.GetAxis("Vertical") == 1)
        {
            movement += new Vector3(0, 5, 0);
        }

        MoveCamera(movement);
    }
}
