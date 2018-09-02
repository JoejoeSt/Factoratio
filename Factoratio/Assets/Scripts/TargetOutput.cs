using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetOutput : GraphNode
{
    public void Calculate()
    {
        inputs[0].amountText.text = inputs[0].amountField.GetComponentInChildren<Text>().text;
        inputs[0].TellCounterpart();
    }

    public override void Calculate(InOutNode changeingNode, float wantedValue)
    {
        if (wantedValue <= float.Parse(inputs[0].amountField.GetComponentInChildren<Text>().text))
        {
            inputs[0].amountText.text = wantedValue.ToString();
            //Einfärben
        }
    }
}