using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Splitter : GraphNode
{
    private List<int> inputPriorities;
    private List<bool> maxedOutInputs;
    private List<int> outputPriorities;
    private List<float> offeredInputAmounts;
    private List<float> wantedOutputValues;

    public Toggle dumpExcess;
    public Text dumpAmount;

    private new void Start()
    {
        base.Start();

        inputPriorities = new List<int>();
        maxedOutInputs = new List<bool>();
        offeredInputAmounts = new List<float>();
        for (int i = 0; i < inputs.Count; i++)
        {
            inputPriorities.Add(0);
            maxedOutInputs.Add(false);
            offeredInputAmounts.Add(0);
        }
        outputPriorities = new List<int>();
        wantedOutputValues = new List<float>();
        for (int i = 0; i < outputs.Count; i++)
        {
            outputPriorities.Add(0);
            wantedOutputValues.Add(0);
        }
    }

    /*
    public override void Calculate(InOutNode changeingNode, float wantedValue)
    {
        foreach (InputNode inputNode in inputs)
        {
            if (inputNode == changeingNode)
            {
                if (inputNode.GetTextAmount() < wantedValue && !intermediateProducts.isOn && inputPriorities[inputs.IndexOf(inputNode)] != inputs.Count)
                {
                    return;
                }
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

        dumpAmount.text = "X";
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
        if((GetCurrentInputAmount() - GetCurrentOutputAmount() - inputNode.GetTextAmount() + wantedValue) < 0)
        //if (inputNode.GetTextAmount() > wantedValue)
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
    */

    public override void Calculate(InOutNode changeingNode, float wantedValue)
    {
        foreach (InputNode inputNode in inputs)
        {
            if(inputNode == changeingNode)
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

        BalanceInAndOutputs();

        TellNodes();
    }

    private void CalculateForInput(InputNode inputNode, float wantedValue)
    {
        if (intermediateProducts.isOn)
        {
            inputNode.SetAmountWithAmount(wantedValue);
        }

        if (wantedValue > inputNode.GetTextAmount())
        {
            offeredInputAmounts[inputs.IndexOf(inputNode)] = wantedValue;
            //Umverteilen
        }
        else if(wantedValue < inputNode.GetTextAmount())
        {
            inputNode.SetAmountWithAmount(wantedValue);

            if (offeredInputAmounts[inputs.IndexOf(inputNode)] > wantedValue)
            {
                offeredInputAmounts[inputs.IndexOf(inputNode)] = wantedValue;
            }
            else
            {
                maxedOutInputs[inputs.IndexOf(inputNode)] = true;
            }
        }
    }

    private void CalculateForOutput(OutputNode outputNode, float wantedValue)
    {
        wantedOutputValues[outputs.IndexOf(outputNode)] = wantedValue;
        outputNode.SetAmountWithAmount(wantedValue);
    }

    private void BalanceInAndOutputs()
    {
        dumpAmount.text = "X";

        if(GetCurrentInputAmount() > GetCurrentOutputAmount())
        {
            HandleTooMuchInput();
        }
        else
        {
            HandleTooMuchOutput();
        }
    }

    private void HandleTooMuchInput()
    {
        FullfillWantedValues();
        if(Mathf.Abs(GetCurrentInputAmount() - GetCurrentOutputAmount()) < Mathf.Epsilon)
        {
            return;
        }

        DemandLess();
        if (Mathf.Abs(GetCurrentInputAmount() - GetCurrentOutputAmount()) < Mathf.Epsilon)
        {
            return;
        }

        if (!intermediateProducts.isOn)
        {
            TakeLess();
        }
        if (Mathf.Abs(GetCurrentInputAmount() - GetCurrentOutputAmount()) < Mathf.Epsilon)
        {
            return;
        }

        if (dumpExcess.isOn)
        {
            dumpAmount.text = (GetCurrentInputAmount() - GetCurrentOutputAmount()).ToString();
        }
        else
        {
            GiveMoreToPrio1();
        }
    }

    private void HandleTooMuchOutput()
    {
        //Mehr angebotenes nutzen
        if (Mathf.Abs(GetCurrentInputAmount() - GetCurrentOutputAmount()) < Mathf.Epsilon)
        {
            return;
        }

        //Mehr verlangen
        if (Mathf.Abs(GetCurrentInputAmount() - GetCurrentOutputAmount()) < Mathf.Epsilon)
        {
            return;
        }

        //Weniger Überschüssiges abgeben
        if (Mathf.Abs(GetCurrentInputAmount() - GetCurrentOutputAmount()) < Mathf.Epsilon)
        {
            return;
        }

        //Weniger abgeben
    }

    private void FullfillWantedValues()
    {
        int i = 0;
        while(i < outputs.Count && GetCurrentOutputAmount() < GetCurrentInputAmount())
        {
            List<OutputNode> nodesToGive = GetOutputNodesByPriority(i, true, true);
            float surplus = GetCurrentInputAmount() - GetCurrentOutputAmount();

            foreach (OutputNode outputNode in nodesToGive)
            {
                float outputAmount = Mathf.Min(outputNode.GetTextAmount() + surplus / nodesToGive.Count, wantedOutputValues[outputs.IndexOf(outputNode)]);
                outputNode.SetAmountWithAmount(outputAmount);
                if(!nodesToTell.Contains(outputNode))
                {
                    nodesToTell.Add(outputNode);
                }
            }

            if(GetOutputNodesByPriority(i, true, true).Count == 0)
            {
                i++;
            }
        }
    }

    private void DemandLess()
    {
        int i = inputs.Count - 1;
        while(i > 0 && GetCurrentOutputAmount() < GetCurrentInputAmount())
        {
            List<InputNode> nodesToDemandLess = GetInputNodesByPriority(i, true, false);
            float surplus = GetCurrentInputAmount() - GetCurrentOutputAmount();

            foreach (InputNode inputNode in nodesToDemandLess)
            {
                float inputAmount = Mathf.Max(inputNode.GetTextAmount() - surplus / nodesToDemandLess.Count, offeredInputAmounts[inputs.IndexOf(inputNode)]);
                inputNode.SetAmountWithAmount(inputAmount);
                if(!nodesToTell.Contains(inputNode))
                {
                    nodesToTell.Add(inputNode);
                }
            }

            if(GetInputNodesByPriority(i, true, false).Count == 0)
            {
                i--;
            }
        }
    }

    private void TakeLess()
    {
        int i = inputs.Count - 1;
        while (i > 0 && GetCurrentOutputAmount() < GetCurrentInputAmount())
        {
            List<InputNode> nodesToDemandLess = GetInputNodesByPriority(i, false, false);
            float surplus = GetCurrentInputAmount() - GetCurrentOutputAmount();

            foreach (InputNode inputNode in nodesToDemandLess)
            {
                float inputAmount = Mathf.Max(inputNode.GetTextAmount() - surplus / nodesToDemandLess.Count, offeredInputAmounts[inputs.IndexOf(inputNode)]);
                inputNode.SetAmountWithAmount(inputAmount);
            }

            if (GetInputNodesByPriority(i, false, false).Count == 0)
            {
                i--;
            }
        }

        while(GetCurrentOutputAmount() < GetCurrentInputAmount())
        {
            List<InputNode> nodesToDemandLess = GetInputNodesByPriority(inputs.Count, false, false);
            float surplus = GetCurrentInputAmount() - GetCurrentOutputAmount();

            foreach (InputNode inputNode in nodesToDemandLess)
            {
                float inputAmount = Mathf.Max(inputNode.GetTextAmount() - surplus / nodesToDemandLess.Count, offeredInputAmounts[inputs.IndexOf(inputNode)]);
                inputNode.SetAmountWithAmount(inputAmount);
            }
        }
    }

    private void GiveMoreToPrio1()
    {
        List<OutputNode> prio1OutputNodes = GetOutputNodesByPriority(0, false, true);
        float surplus = GetCurrentInputAmount() - GetCurrentOutputAmount();

        foreach (OutputNode outputNode in prio1OutputNodes)
        {
            outputNode.SetAmountWithAmount(outputNode.GetTextAmount() + surplus / prio1OutputNodes.Count);
        }
    }

    private List<OutputNode> GetOutputNodesByPriority(int priority, bool useWantedValues, bool allowZeroValue)
    {
        List<OutputNode> returnValue = new List<OutputNode>();

        for (int i = 0; i < outputs.Count; i++)
        {
            if(outputPriorities[i] == priority 
               && (!useWantedValues || wantedOutputValues[i] > outputs[i].GetTextAmount()) 
               && (allowZeroValue || Mathf.Abs(outputs[i].GetTextAmount()) < Mathf.Epsilon))
            {
                returnValue.Add(outputs[i]);
            }
        }

        return returnValue;
    }

    private List<InputNode> GetInputNodesByPriority(int priority, bool demandingOnly, bool allowZeroValue)
    {
        List<InputNode> returnValue = new List<InputNode>();

        for (int i = 0; i < inputs.Count; i++)
        {
            if (inputPriorities[i] == priority 
                && (!demandingOnly || inputs[i].GetTextAmount() > offeredInputAmounts[i]) 
                && (allowZeroValue || Mathf.Abs(inputs[i].GetTextAmount()) < Mathf.Epsilon))
            {
                returnValue.Add(inputs[i]);
            }
        }

        return returnValue;
    }

    private float GetCurrentOutputAmount()
    {
        float outputAmount = 0;
        foreach (OutputNode outputNode in outputs)
        {
            outputAmount += outputNode.GetTextAmount();
        }

        return outputAmount;
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
            offeredInputAmounts[i] = 0;
        }
        for (int i = 0; i < outputs.Count; i++)
        {
            outputs[i].Reset();
            wantedOutputValues[i] = 0;
        }
        dumpAmount.text = "X";
    }
}