using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplitterPriorityser : MonoBehaviour
{
    public InOutNode inOutput;
    private Splitter node;

    private bool isInput;

    private void Start()
    {
        inOutput = this.GetComponentInParent<InOutNode>();

        node = (Splitter) inOutput.graphNode;
        isInput = false;
        foreach (InputNode input in node.inputs)
        {
            if(input == inOutput)
            {
                isInput = true;
            }
        }
    }

    public void ChangePriority()
    {
        if(isInput)
        {
            ChangePriorityForInput();
        }
        else
        {
            ChangePriorityForOutput();
        }
    }

    private void ChangePriorityForInput()
    {
        int value;
        if(this.GetComponentInChildren<Text>().text == "No Demanding")
        {
            value = node.inputs.Count + 1;
        }
        else
        {
            value = int.Parse(this.GetComponentInChildren<Text>().text);
        }

        value = value % (node.inputs.Count + 1) + 1;

        if (value == node.inputs.Count + 1)
        {
            this.GetComponentInChildren<Text>().text = "No Demanding";
        }
        else
        {
            this.GetComponentInChildren<Text>().text = value.ToString();
        }
        node.ChangePriority(inOutput, value - 1);
    }

    private void ChangePriorityForOutput()
    {
        int value = int.Parse(this.GetComponentInChildren<Text>().text);
        value = value % node.outputs.Count + 1;

        this.GetComponentInChildren<Text>().text = value.ToString();
        node.ChangePriority(inOutput, value - 1);
    }
}
