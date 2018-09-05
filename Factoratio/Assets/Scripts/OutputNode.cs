using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputNode : InOutNode
{
    public override void TellNode(float wantedValue)
    {
        if (wantedValue > GetTextAmount())
        {
            graphNode.Calculate(this, wantedValue);
        }
    }
}
