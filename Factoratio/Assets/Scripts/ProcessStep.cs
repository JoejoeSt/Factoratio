using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessStep : GraphNode
{
    public InputField speed;
    public InputField time;
    public Text machineNumber;

    private bool isAtMax = false;
    private float cyclesPerSecond;

    public void CalculateCycles()
    {
        if (time.text.Length != 0 && speed.text.Length != 0)
        {
            cyclesPerSecond = (float)1 / float.Parse(time.text) * float.Parse(speed.text);
        }
    }

    public override void Calculate(InOutNode changeingNode, float wantedValue)
    {
        foreach (InputNode input in inputs)
        {
            if(input == changeingNode)
            {
                CalculateForInput(changeingNode, wantedValue);
                return;
            }
        }
        CalculateForOutput(changeingNode, wantedValue);
    }

    private void CalculateForInput(InOutNode input, float wantedValue)
    {
        //Zwischenprodukte erlaubt ?
        isAtMax = true;
        float recipeAmount = float.Parse(input.amountField.GetComponentInChildren<Text>().text);
        float possibleMachines = wantedValue / (recipeAmount * cyclesPerSecond);
        machineNumber.text = possibleMachines + " (" + Mathf.CeilToInt(possibleMachines) + ")";

        foreach (InputNode inputNode in inputs)
        {
            if(inputNode == input)
            {
                inputNode.SetAmount(possibleMachines * cyclesPerSecond);
                continue;
            }
            inputNode.SetAmount(possibleMachines * cyclesPerSecond);
            inputNode.TellCounterpart();
        }
        foreach (OutputNode outputNode in outputs)
        {
            outputNode.SetAmount(possibleMachines * cyclesPerSecond);
            outputNode.TellCounterpart();
        }
    }

    private void CalculateForOutput(InOutNode output, float wantedValue)
    {
        if(isAtMax)
        {
            output.TellCounterpart();
            return;
        }

        float recipeAmount = float.Parse(output.amountField.GetComponentInChildren<Text>().text);
        float possibleMachines = wantedValue / (recipeAmount * cyclesPerSecond);
        machineNumber.text = possibleMachines + " (" + Mathf.CeilToInt(possibleMachines) + ")";

        foreach (InputNode inputNode in inputs)
        {
            inputNode.SetAmount(possibleMachines * cyclesPerSecond);
            inputNode.TellCounterpart();
        }
        foreach (OutputNode outputNode in outputs)
        {
            if(outputNode == output)
            {
                continue;
            }
            outputNode.SetAmount(possibleMachines * cyclesPerSecond);
        }
    }
}