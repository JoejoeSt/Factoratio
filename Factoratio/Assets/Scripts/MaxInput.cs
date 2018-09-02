using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxInput : GraphNode
{
    public override void Calculate(InOutNode changeingNode, float wantedValue)
    {
        float maxValue = float.Parse(changeingNode.amountField.GetComponentInChildren<Text>().text);

        if (maxValue < wantedValue)
        {
            outputs[0].amountText.text = maxValue.ToString();
            //Einfärben
            outputs[0].TellCounterpart();
        }
        else
        {
            outputs[0].amountText.text = wantedValue.ToString();
        }
    }
}
