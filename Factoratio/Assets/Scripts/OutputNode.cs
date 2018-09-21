using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputNode : InOutNode
{
    public override void TellNode(float wantedValue)
    {
        graphNode.Calculate(this, wantedValue);
    }
}
