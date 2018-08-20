using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementTouch : CameraMovement
{

    private void FixedUpdate()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            MoveCamera(-Input.GetTouch(i).deltaPosition / 10 / Input.touchCount);
        }

        if (Input.touchCount > 1)
        {
            ZoomCamera(CalculateZoom() / 10);
        }
    }

    private float CalculateZoom()
    {
        Vector2 middle = new Vector2();

        for (int i = 0; i < Input.touchCount; i++)
        {
            middle += Input.GetTouch(i).position;
        }

        middle /= Input.touchCount;


        float distanceNow = 0;
        float distanceBefore = 0;

        for (int i = 0; i < Input.touchCount; i++)
        {
            distanceNow += Vector2.Distance(Input.GetTouch(i).position, middle);
            distanceBefore += Vector2.Distance(Input.GetTouch(i).position - Input.GetTouch(i).deltaPosition, middle);
        }

        return distanceBefore - distanceNow;
    }
}
