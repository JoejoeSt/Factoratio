using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    private List<TargetOutput> targets;
    private List<MaxInput> inputs;

    public void Calculate()
    { 
        FillLists();

        foreach (TargetOutput target in targets)
        {
            CalculateFromTarget(target);
        }

        foreach (MaxInput input in inputs)
        {
            CalculateFromInput(input);
        }
    }

    private void CalculateFromTarget(TargetOutput target)
    {
        List<GraphNode> activeNodes = new List<GraphNode>();


    }

    private void CalculateFromInput(MaxInput input)
    {
        List<GraphNode> activeNodes = new List<GraphNode>();


    }

    private void FillLists()
    {
        targets = new List<TargetOutput>();
        inputs = new List<MaxInput>();

        foreach (GameObject targetOutput in GameObject.FindGameObjectsWithTag("TargetOutput"))
        {
            targets.Add(targetOutput.GetComponent<TargetOutput>());
        }

        foreach (GameObject maxInput in GameObject.FindGameObjectsWithTag("MAxInput"))
        {
            inputs.Add(maxInput.GetComponent<MaxInput>());
        }
    }
}
