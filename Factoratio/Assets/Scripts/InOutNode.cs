using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InOutNode : MonoBehaviour
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

        Destroy(connectingLine);

        GameObject.Find("Connectors").GetComponent<Connector>().Reconnect(this.gameObject, connector.transform.position, counterpart.gameObject, counterpart.connector.transform.position);
    }

    public void TellCounterpart()
    {
        if(counterpart == null)
        {
            return;
        }

        float amount = float.Parse(amountText.text);

        counterpart.TellNode(amount);
    }

    public abstract void TellNode(float wantedValue);

    public void SetAmount(float machinecount)
    {
        float amount = machinecount * float.Parse(amountField.GetComponentInChildren<Text>().text);
        amountText.text = amount.ToString();
    }
}