using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxInput : GraphNode
{
    public override void Calculate(InOutNode changeingNode, float wantedValue)
    {
        float maxValue = outputs[0].GetFieldAmount();

        if (maxValue < wantedValue)
        {
            outputs[0].SetAmountWithAmount(maxValue);
            //Einfärben
            outputs[0].TellCounterpart();
        }
        else
        {
            outputs[0].SetAmountWithAmount(wantedValue);
        }
    }

    public override void Reset()
    {
        outputs[0].Reset();
    }
}
