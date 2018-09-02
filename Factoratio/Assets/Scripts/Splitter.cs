using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Splitter : GraphNode
{
    public Dropdown mode;

    private bool isAtMax;

    public override void Calculate(InOutNode changeingNode, float wantedValue)
    {
        switch (mode.value)
        {
            case 0:
                break;
            default:
                break;
        }
    }

    public void ChangeMode()
    {

    }
}