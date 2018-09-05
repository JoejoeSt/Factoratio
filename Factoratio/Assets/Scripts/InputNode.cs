using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNode : InOutNode
{
    public Toggle intermediateProducts;

    private void Start()
    {
        intermediateProducts = GameObject.Find("Toggle").GetComponent<Toggle>();
    }

    public override void TellNode(float wantedValue)
    {
        if(amountText.text == "Amount/s" && !intermediateProducts.isOn)
        {
            return;
        }
        else if(wantedValue < GetTextAmount() || intermediateProducts.isOn)
        {
            graphNode.Calculate(this, wantedValue);
        }
    }
}
