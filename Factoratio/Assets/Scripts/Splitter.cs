using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Splitter : GraphNode
{
    private List<int> inputPriorities;
    private List<bool> maxedOutInputs;
    private List<int> outputPriorities;
    private List<float> wantedOutputValues;

    public Toggle dumpExcess;
    public Text dumpAmount;

    private new void Start()
    {
        base.Start();

        inputPriorities = new List<int>();
        maxedOutInputs = new List<bool>();
        for (int i = 0; i < inputs.Count; i++)
        {
            inputPriorities.Add(0);
            maxedOutInputs.Add(false);
        }
        outputPriorities = new List<int>();
        wantedOutputValues = new List<float>();
        for (int i = 0; i < outputs.Count; i++)
        {
            outputPriorities.Add(0);
            wantedOutputValues.Add(0);
        }
    }

    public override void Calculate(InOutNode changeingNode, float wantedValue)
    {
        foreach (InputNode inputNode in inputs)
        {
            if (inputNode == changeingNode)
            {
                CalculateForInput(inputNode, wantedValue);
                break;
            }
        }
        foreach (OutputNode outputNode in outputs)
        {
            if (outputNode == changeingNode)
            {
                CalculateForOutput(outputNode, wantedValue);
                break;
            }
        }

        if (GetCurrentInputAmount() > GetCurrentOutputAmount())
        {
            HandleMoreInput();
        }
        else
        {
            List<InputNode> inputNodes = GetAvailableInputs();
            if (inputNodes.Count == 0)
            {
                LimitOutputs();
            }
            else
            {
                DemandFromInputs(inputNodes);
            }
        }
        TellNodes();
    }

    private void CalculateForInput(InputNode inputNode, float wantedValue)
    {
        if (inputNode.GetTextAmount() > wantedValue)
        {
            maxedOutInputs[inputs.IndexOf(inputNode)] = true;
        }
        inputNode.SetAmountWithAmount(wantedValue);
    }

    private void CalculateForOutput(OutputNode outputNode, float wantedValue)
    {
        outputNode.SetAmountWithAmount(wantedValue);
        wantedOutputValues[outputs.IndexOf(outputNode)] = wantedValue;
    }

    private void LimitOutputs()
    {
        int priorityToLimit = outputs.Count - 1;
        while(GetCurrentOutputAmount() > GetCurrentInputAmount())
        {
            List<OutputNode> outputsToLimit = GetOutputsByPriority(priorityToLimit, false);
            float amountToMuch = GetCurrentOutputAmount() - GetCurrentInputAmount();
            foreach (OutputNode outputNode in outputsToLimit)
            {
                outputNode.SetAmountWithAmount(Mathf.Max(outputNode.GetTextAmount() - amountToMuch / outputsToLimit.Count, 0));
                if (!nodesToTell.Contains(outputNode))
                {
                    nodesToTell.Add(outputNode);
                }
            }
            if(GetOutputsByPriority(priorityToLimit, false).Count == 0)
            {
                priorityToLimit--;
            }
        }
    }

    private void DemandFromInputs(List<InputNode> inputNodes)
    {
        float demandedAmount = GetCurrentOutputAmount() - GetCurrentInputAmount();
        foreach (InputNode inputNode in inputNodes)
        {
            inputNode.SetAmountWithAmount(inputNode.GetTextAmount() + demandedAmount / inputNodes.Count);
            if(!nodesToTell.Contains(inputNode))
            {
                nodesToTell.Add(inputNode);
            }
        }
    }

    private void HandleMoreInput()
    {
        GiveMoreToOutputs(false);
        DemandLessFromInputs();
        if(dumpExcess.isOn)
        {
            Dump();
        }
        else
        {
            GiveMoreToOutputs(true);
        }
    }

    private void GiveMoreToOutputs(bool dontUseWantedValues)
    {
        int priorityToGive = 0;
        while (GetCurrentInputAmount() > GetCurrentOutputAmount() && priorityToGive < outputs.Count)
        {
            List<OutputNode> outputsToGive = GetOutputsByPriority(priorityToGive, true);
            float amountToMuch = GetCurrentInputAmount() - GetCurrentOutputAmount();
            foreach (OutputNode outputNode in outputsToGive)
            {
                if(outputNode.GetTextAmount() < wantedOutputValues[outputs.IndexOf(outputNode)] || dontUseWantedValues)
                {
                    outputNode.SetAmountWithAmount(outputNode.GetTextAmount() + amountToMuch / outputsToGive.Count);
                    if(!nodesToTell.Contains(outputNode))
                    {
                        nodesToTell.Add(outputNode);
                    }
                }
            }
            priorityToGive++;
        }
    }

    private void DemandLessFromInputs()
    {

    }

    private void Dump()
    {
        dumpAmount.text = (GetCurrentInputAmount() - GetCurrentOutputAmount()).ToString();
    }

    private List<InputNode> GetAvailableInputs()
    {
        List<InputNode> returnValue = new List<InputNode>();
        for (int i = 0; i < inputs.Count; i++)
        {
            for (int j = 0; j < inputs.Count; j++)
            {
                if (!maxedOutInputs[j] && inputPriorities[j] == i)
                {
                    returnValue.Add(inputs[j]);
                }
            }
            if(returnValue.Count > 0)
            {
                return returnValue;
            }
        }
        return new List<InputNode>();
    }

    private List<OutputNode> GetOutputsByPriority(int priority, bool wantZeroOutputs)
    {
        List<OutputNode> returnValue = new List<OutputNode>();
        for (int i = 0; i < outputs.Count; i++)
        {
            if(outputPriorities[i] == priority && (outputs[i].GetTextAmount() > 0 || wantZeroOutputs))
            {
                returnValue.Add(outputs[i]);
            }
            if (returnValue.Count > 0)
            {
                return returnValue;
            }
        }
        return new List<OutputNode>();
    }

    private float GetCurrentOutputAmount()
    {
        float returnValue = 0;
        foreach (OutputNode outputNode in outputs)
        {
            returnValue += outputNode.GetTextAmount();
        }
        return returnValue;
    }

    private float GetCurrentInputAmount()
    {
        float returnValue = 0;
        foreach (InputNode inputNode in inputs)
        {
            returnValue += inputNode.GetTextAmount();
        }
        return returnValue;
    }

    public void ChangePriority(InOutNode inOutput, int priority)
    {
        foreach (InputNode input in inputs)
        {
            if(input == inOutput)
            {
                inputPriorities[inputs.IndexOf(input)] = priority;
                return;
            }
        }
        foreach (OutputNode output in outputs)
        {
            if (output == inOutput)
            {
                outputPriorities[outputs.IndexOf(output)] = priority;
                return;
            }
        }
    }

    public override void Reset()
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            inputs[i].Reset();
            maxedOutInputs[i] = false;
        }
        for (int i = 0; i < outputs.Count; i++)
        {
            outputs[i].Reset();
            wantedOutputValues[i] = 0;
        }
        dumpAmount.text = "X";
    }
}