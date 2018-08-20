using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = this.GetComponent<Camera>();
    }

    protected void MoveCamera(Vector3 movement)
    {
        this.transform.position += movement;
    }

    protected void ZoomCamera(float zoom)
    {
        cam.orthographicSize += zoom;
    }
}