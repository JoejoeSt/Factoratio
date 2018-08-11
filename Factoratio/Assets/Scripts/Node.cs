using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public Text amountText;
    public InputField amountField;
    public InputField item;
    public GraphNode node;
    public GameObject connector;

    private Node counterpart;
    private GameObject connectingLine;

    public void SetNode(GraphNode parentNode)
    {
        node = parentNode;
    }

    public void SetCounterpart(Node newCounterpart)
    {
        counterpart = newCounterpart;
    }

    public void SetConnectingLine(GameObject newConnectingLine)
    {
        connectingLine = newConnectingLine;
    }

    public void ClearConnection()
    {
        if (counterpart == null)
        {
            return;
        }

        counterpart.ClearCounterpart();
        ClearCounterpart();
        Destroy(connectingLine);
    }

    public void ClearCounterpart()
    {
        counterpart = null;
    }

    public void CheckItemName()
    {
        if (counterpart == null)
        {
            return;
        }

        if(item.transform.Find("Text").GetComponent<Text>().text == "" || counterpart.item.transform.Find("Text").GetComponent<Text>().text == "")
        {
            ColorItemField(Color.yellow);
            counterpart.ColorItemField(Color.yellow);
        }
        else if(item.transform.Find("Text").GetComponent<Text>().text != counterpart.item.transform.Find("Text").GetComponent<Text>().text)
        {
            ColorItemField(Color.red);
            counterpart.ColorItemField(Color.red);
        }
        else
        {
            ColorItemField(Color.white);
            counterpart.ColorItemField(Color.white);
        }
    }

    public void ColorItemField(Color color)
    {
        item.GetComponent<Image>().color = color;
    }

    public void Connect()
    {
        GameObject.Find("Connectors").GetComponent<Connector>().Connect(this.gameObject, connector.transform.position);
    }
}