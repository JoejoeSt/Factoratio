using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetOutput : GraphNode
{
    public void Calculate()
    {
        inputs[0].SetAmountWithAmount(inputs[0].GetFieldAmount());
        inputs[0].TellCounterpart();
    }

    public override void Calculate(InOutNode changeingNode, float wantedValue)
    {
        inputs[0].amountText.text = wantedValue.ToString();
    }

    public override void Reset()
    {
        inputs[0].Reset();
    }
}