using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InOutNode : MonoBehaviour
{
    public Text amountText;
    public InputField amountField;
    public InputField item;
    public GraphNode graphNode;
    public GameObject connector;

    private InOutNode counterpart;
    private GameObject connectingLine;

    public void SetNode(GraphNode parentNode)
    {
        graphNode = parentNode;
    }

    public void SetCounterpart(InOutNode newCounterpart)
    {
        counterpart = newCounterpart;
    }

    public void SetConnectingLine(GameObject newConnectingLine)
    {
        connectingLine = newConnectingLine;
    }

    public InOutNode GetCounterpart()
    {
        return counterpart;
    }

    public void ClearConnection()
    {
        if (counterpart == null)
        {
            return;
        }

        ColorItemField(Color.white);
        counterpart.ColorItemField(Color.white);

        counterpart.SetCounterpart(null);
        counterpart = null;
        Destroy(connectingLine);
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

    public void Reconnect()
    {
        if(counterpart == null)
        {
            return;
        }

        Vector2 differenceVector = connector.transform.position - counterpart.connector.transform.position;

        connectingLine.GetComponent<RectTransform>().sizeDelta = new Vector2(differenceVector.magnitude, 10);
        connectingLine.GetComponent<RectTransform>().position = connector.transform.position;
        float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
        connectingLine.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, angle);
    }
}