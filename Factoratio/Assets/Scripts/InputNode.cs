using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNode : InOutNode
{
    public override void TellNode(float wantedValue)
    {
        if (amountText.text == "Amount/s" || wantedValue < float.Parse(amountText.text) || transform.parent.name == "TargetOutput(Clone)")
        {
            graphNode.Calculate(this, wantedValue);
        }
    }
}
