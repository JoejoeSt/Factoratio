using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connector : MonoBehaviour
{
    private GameObject start;
    private Vector2 startConnectionPoint;
    public GameObject connectingLinePrefab;

    public void Connect(GameObject node, Vector3 connectionPoint)
    {
        node.GetComponent<Node>().ClearConnection();

        if(start == null || start == node)
        {
            start = node;
            startConnectionPoint = connectionPoint;
        }
        else
        {
            if(start.GetComponent<InputNode>() == node.GetComponent<InputNode>() || start.GetComponent<OutputNode>() == node.GetComponent<OutputNode>() || start.GetComponent<Node>().node == node.GetComponent<Node>().node)
            {
                start = node;
                startConnectionPoint = connectionPoint;
            }
            else
            {
                ConnectNodes(node, connectionPoint);
                start = null;
            }
        }
    }

    private void ConnectNodes(GameObject node, Vector2 connectionPoint)
    {
        start.GetComponent<Node>().SetCounterpart(node.GetComponent<Node>());
        node.GetComponent<Node>().SetCounterpart(start.GetComponent<Node>());

        GameObject newConnectingLine = Instantiate(connectingLinePrefab, GameObject.Find("Connectors").transform);

        Vector2 differenceVector = connectionPoint - startConnectionPoint;

        newConnectingLine.GetComponent<RectTransform>().sizeDelta = new Vector2(differenceVector.magnitude, 10);
        newConnectingLine.GetComponent<RectTransform>().position = startConnectionPoint;
        float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
        newConnectingLine.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, angle);

        start.GetComponent<Node>().SetConnectingLine(newConnectingLine);
        node.GetComponent<Node>().SetConnectingLine(newConnectingLine);

        start.GetComponent<Node>().CheckItemName();
    }

}
