﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera cam;

	void Start ()
    {
        cam = this.GetComponent<Camera>();
	}
	
	void FixedUpdate ()
    {
        cam.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * (-50);

        if(Input.mousePosition.x < (float) 5 * Screen.width / 100 || Input.GetAxis("Horizontal") == -1)
        {
            this.transform.position -= new Vector3(1, 0, 0);
        }
        else if (Input.mousePosition.x > (float) 95 * Screen.width / 100 || Input.GetAxis("Horizontal") == 1)
        {
            this.transform.position += new Vector3(1, 0, 0);
        }

        if (Input.mousePosition.y < (float) 5 * Screen.height / 100 || Input.GetAxis("Vertical") == -1)
        {
            this.transform.position -= new Vector3(0, 1, 0);
        }
        else if (Input.mousePosition.y > (float) 95 * Screen.height / 100 || Input.GetAxis("Vertical") == 1)
        {
            this.transform.position += new Vector3(0, 1, 0);
        }
    }
}