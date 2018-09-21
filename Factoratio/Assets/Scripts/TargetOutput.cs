using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetOutput : GraphNode
{
    public void Calculate()
    {
        if(inputs[0].GetTextAmount() > inputs[0].GetFieldAmount())
        {
            return;
        }
        inputs[0].SetAmountWithAmount(inputs[0].GetFieldAmount());
        inputs[0].TellCounterpart();
    }

    public override void Calculate(InOutNode changeingNode, float wantedValue)
    {
        if (inputs[0].GetTextAmount() < wantedValue && !intermediateProducts.isOn)
        {
            return;
        }
        inputs[0].amountText.text = wantedValue.ToString();
    }

    public override void Reset()
    {
        inputs[0].Reset();
    }
}