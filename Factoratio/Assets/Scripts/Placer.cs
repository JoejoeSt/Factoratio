using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placer : MonoBehaviour
{
    private GameObject currentObjectToPlace;

    public void GiveObjectToPlace(GameObject objectToPlace)
    {
        currentObjectToPlace = objectToPlace;
    }


    void Update()
    {
        if (currentObjectToPlace != null)
        {
            currentObjectToPlace.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0) + GetPositionOfCurserRelativeToCam();
        }
        else
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x < (float) Screen.width * 0.8f)
            {
                currentObjectToPlace.GetComponent<GraphNode>().Reconnect();
                currentObjectToPlace = null;
            }
            else
            {
                currentObjectToPlace.GetComponent<GraphNode>().ClearConnections();
                Destroy(currentObjectToPlace);
            }
        }

    }

    public Vector3 GetPositionOfCurserRelativeToCam()
    {
        Vector3 positionOfCursorRelativeToCenter = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);    
        float zoomFactor = (float) Screen.height / 2 / Camera.main.orthographicSize;
        Vector3 mousePositionRelativeToCam = positionOfCursorRelativeToCenter / zoomFactor;

        return mousePositionRelativeToCam;
    }
}
