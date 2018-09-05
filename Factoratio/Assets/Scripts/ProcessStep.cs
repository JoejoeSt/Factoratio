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
    private float cyclesPerSecondMachine;

    public void CalculateCycles()
    {
        if (time.text.Length != 0 && speed.text.Length != 0)
        {
            cyclesPerSecondMachine = (float)1 / float.Parse(time.text) * float.Parse(speed.text);
        }
    }

    public override void Calculate(InOutNode changeingNode, float wantedValue)
    {
        foreach (InputNode input in inputs)
        {
            if(input == changeingNode)
            {
                CalculateForInput((InputNode) changeingNode, wantedValue);
                return;
            }
        }
        CalculateForOutput((OutputNode) changeingNode, wantedValue);

        TellNodes();
    }

    private void CalculateForInput(InputNode input, float wantedValue)
    {
        if (input.GetTextAmount() > wantedValue)
        {
            isAtMax = true;
        }

        float recipeAmount = input.GetFieldAmount();
        float possibleMachines = CalculatePossibleMachines(wantedValue / (recipeAmount * cyclesPerSecondMachine));

        input.SetAmountWithCycles(possibleMachines * cyclesPerSecondMachine);
        foreach (InputNode inputNode in inputs)
        {
            if (inputNode != input)
            {
                inputNode.SetAmountWithCycles(possibleMachines * cyclesPerSecondMachine);
                if (!nodesToTell.Contains(inputNode))
                {
                    nodesToTell.Add(inputNode);
                }
            }
        }
        foreach (OutputNode outputNode in outputs)
        {
            outputNode.SetAmountWithCycles(possibleMachines * cyclesPerSecondMachine);
            if (!nodesToTell.Contains(outputNode))
            {
                nodesToTell.Add(outputNode);
            }
        }
    }

    private void CalculateForOutput(OutputNode output, float wantedValue)
    {
        if(isAtMax)
        {
            output.TellCounterpart();
            return;
        }

        float recipeAmount = output.GetFieldAmount();
        float possibleMachines = CalculatePossibleMachines(wantedValue / (recipeAmount * cyclesPerSecondMachine));

        output.SetAmountWithCycles(possibleMachines * cyclesPerSecondMachine);
        foreach (InputNode inputNode in inputs)
        {
            inputNode.SetAmountWithCycles(possibleMachines * cyclesPerSecondMachine);
            if (!nodesToTell.Contains(inputNode))
            {
                nodesToTell.Add(inputNode);
            }
        }
        foreach (OutputNode outputNode in outputs)
        {
            if (outputNode != output)
            {
                outputNode.SetAmountWithCycles(possibleMachines * cyclesPerSecondMachine);
                if (!nodesToTell.Contains(outputNode))
                {
                    nodesToTell.Add(outputNode);
                }
            }
        }
    }

    private float CalculatePossibleMachines(float currentMachines)
    {
        float possibleMachines = currentMachines;
        if(!isAtMax)
        {
            machineNumber.text = possibleMachines + " (" + Mathf.CeilToInt(possibleMachines) + ")";
            return possibleMachines;
        }
        foreach (InputNode inputNode in inputs)
        {
            if (inputNode.amountText.text != "Amount/s")
            {
                possibleMachines = Mathf.Min(inputNode.GetTextAmount() / (inputNode.GetFieldAmount() * cyclesPerSecondMachine), possibleMachines);
            }
        }
        machineNumber.text = possibleMachines + " (" + Mathf.CeilToInt(possibleMachines) + ")";
        return possibleMachines;
    }

    public override void Reset()
    {
        foreach (InputNode input in inputs)
        {
            input.Reset();
        }
        foreach (OutputNode output in outputs)
        {
            output.Reset();
        }

        machineNumber.text = "X";
        isAtMax = false;
    }
}