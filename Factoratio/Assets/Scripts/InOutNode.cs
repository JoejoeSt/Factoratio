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

        if (item.text == "" || counterpart.item.text == "")
        {
            ColorItemField(Color.yellow);
            counterpart.ColorItemField(Color.yellow);
        }
        else if(item.text != counterpart.item.text)
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

        float amount;
        if(amountText.text == "Amount/s")
        {
            amount = 0;
        }
        else
        {
            amount = float.Parse(amountText.text);
        }

        counterpart.TellNode(amount);
    }

    public abstract void TellNode(float wantedValue);

    public void SetAmountWithCycles(float cyclesPerSecond)
    {
        float amount = cyclesPerSecond * GetFieldAmount();
        amountText.text = amount.ToString();
    }

    public void SetAmountWithAmount(float amount)
    {
        amountText.text = amount.ToString();
    }

    public float GetTextAmount()
    {
        if (amountText.text == "Amount/s")
        {
            return 0;
        }
        else
        {
            return float.Parse(amountText.text);
        }
    }

    public float GetFieldAmount()
    {
        float amount;
        if (!float.TryParse(amountField.text, out amount))
        {
            return 0;
        }
        else
        {
            return amount;
        }
    }

    public void Reset()
    {
        amountText.text = "Amount/s";
    }
}