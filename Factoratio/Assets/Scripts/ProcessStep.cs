using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessStep : GraphNode
{
    public InputField speed;
    public InputField time;
    public Text machineNumber;

    private bool isAtMax;
    private float cyclesPerSecondMachine;
    private List<float> wantedMachineCounts;

    private new void Start()
    {
        base.Start();

        wantedMachineCounts = new List<float>();
        for (int i = 0; i < outputs.Count; i++)
        {
            wantedMachineCounts.Add(0);
        }
    }

    public void CalculateCycles()
    {
        if (time.text.Length != 0 && speed.text.Length != 0)
        {
            cyclesPerSecondMachine = 1 / float.Parse(time.text) * float.Parse(speed.text);
        }
    }

    public override void Calculate(InOutNode changeingNode, float wantedValue)
    {
        foreach(InputNode inputNode in inputs)
        {
            if (inputNode == changeingNode)
            {
                CalculateForInput(inputNode, wantedValue);
                return;
            }
        }
        foreach (OutputNode outputNode in outputs)
        {
            if (outputNode == changeingNode)
            {
                CalculateForOutput(outputNode, wantedValue);
                return;
            }
        }
    }

    private void CalculateForInput(InputNode inputNode, float wantedValue)
    {
        if (wantedValue / (inputNode.GetFieldAmount() * cyclesPerSecondMachine) < GetMaxWantedMachines())
        {
            isAtMax = true;
        }
        if(inputNode.GetTextAmount() < wantedValue && !intermediateProducts.isOn)
        {
            return;
        }

        float recipeAmount = inputNode.GetFieldAmount();
        float possibleMachines = wantedValue / (recipeAmount * cyclesPerSecondMachine);

        SetMachineText(possibleMachines);

        foreach (InputNode input in inputs)
        {
            inputNode.SetAmountWithCycles(possibleMachines * cyclesPerSecondMachine);
            if (!nodesToTell.Contains(inputNode) && inputNode != input)
            {
                nodesToTell.Add(inputNode);
            }
        }
        foreach (OutputNode output in outputs)
        {
            output.SetAmountWithCycles(possibleMachines * cyclesPerSecondMachine);
            if (!nodesToTell.Contains(output))
            {
                nodesToTell.Add(output);
            }
        }

        TellNodes();
    }

    private void CalculateForOutput(OutputNode outputNode, float wantedValue)
    {
        if (isAtMax)
        {
            outputNode.TellCounterpart();
            return;
        }
        if (wantedValue < outputNode.GetTextAmount())
        {
            outputNode.TellCounterpart();
            return;
        }

        float recipeAmount = outputNode.GetFieldAmount();
        float wantedMachines = wantedValue / (recipeAmount * cyclesPerSecondMachine);
        wantedMachineCounts[outputs.IndexOf(outputNode)] = wantedMachines;

        SetMachineText(wantedMachines);

        foreach (InputNode input in inputs)
        {
            input.SetAmountWithCycles(wantedMachines * cyclesPerSecondMachine);
            if (!nodesToTell.Contains(input))
            {
                nodesToTell.Add(input);
            }
        }
        foreach (OutputNode output in outputs)
        {
            outputNode.SetAmountWithCycles(wantedMachines * cyclesPerSecondMachine);
            if (!nodesToTell.Contains(outputNode) && outputNode != output)
            {
                nodesToTell.Add(outputNode);
            }
        }

        TellNodes();
    }

    private void SetMachineText(float machineCount)
    {
        machineNumber.text = machineCount + " (" + Mathf.CeilToInt(machineCount) + ")";
    }

    private float GetMaxWantedMachines()
    {
        float max = 0;
        foreach (float number in wantedMachineCounts)
        {
            max = Mathf.Max(number, max);
        }

        return max;
    }

    public override void Reset()
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            inputs[i].Reset();
        }
        for (int i = 0; i < outputs.Count; i++)
        {
            outputs[i].Reset();
            wantedMachineCounts[i] = 0;
        }

        machineNumber.text = "X";
        isAtMax = false;
    }
}