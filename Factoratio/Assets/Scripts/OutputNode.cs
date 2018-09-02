using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputNode : InOutNode
{
    public override void TellNode(float wantedValue)
    {
        if (amountText.text == "Amount/s" || wantedValue > float.Parse(amountText.text))
        {
            graphNode.Calculate(this, wantedValue);
        }
    }
}
