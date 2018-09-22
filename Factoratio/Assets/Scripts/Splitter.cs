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
        if (wantedValue > inputNode.GetTextAmount())
        {
            offeredInputAmounts[inputs.IndexOf(inputNode)] = wantedValue;
        }
        else if(wantedValue < inputNode.GetTextAmount())
        {
            if (offeredInputAmounts[inputs.IndexOf(inputNode)] > wantedValue)
            {
                offeredInputAmounts[inputs.IndexOf(inputNode)] = wantedValue;
            }
            else
            {
                maxedOutInputs[inputs.IndexOf(inputNode)] = true;
            }
        }

        inputNode.SetAmountWithAmount(wantedValue);
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
        else
        {
            if (dumpExcess.isOn)
            {
                dumpAmount.text = (GetCurrentInputAmount() - GetCurrentOutputAmount()).ToString();
            }
            else
            {
                GiveMoreToPrio1();
            }
        }
    }

    private void HandleTooMuchOutput()
    {
        GiveLessExcessToOutputs();
        if (Mathf.Abs(GetCurrentInputAmount() - GetCurrentOutputAmount()) < Mathf.Epsilon)
        {
            return;
        }

        if (!intermediateProducts.isOn)
        {
            UseMoreOffered();
            if (Mathf.Abs(GetCurrentInputAmount() - GetCurrentOutputAmount()) < Mathf.Epsilon)
            {
                return;
            }
        }

        DemandMore();
        if (Mathf.Abs(GetCurrentInputAmount() - GetCurrentOutputAmount()) < Mathf.Epsilon)
        {
            return;
        }

        GiveLessToOutputs();
    }

    private void FullfillWantedValues()
    {
        int i = 0;
        while(i < outputs.Count && GetCurrentOutputAmount() < GetCurrentInputAmount())
        {
            List<OutputNode> nodesToGive = GetOutputNodesByPriority(i, true, false, true);
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

            if(GetOutputNodesByPriority(i, true, false, true).Count == 0)
            {
                i++;
            }
        }
    }

    private void DemandLess()
    {
        int i = inputs.Count - 1;
        while(i >= 0 && GetCurrentOutputAmount() < GetCurrentInputAmount())
        {
            List<InputNode> nodesToDemandLess = GetInputNodesByPriority(i, true, false, true, false);
            float surplus = GetCurrentInputAmount() - GetCurrentOutputAmount();

            foreach (InputNode inputNode in nodesToDemandLess)
            {
                float inputAmount = Mathf.Max(inputNode.GetTextAmount() - surplus / nodesToDemandLess.Count, offeredInputAmounts[inputs.IndexOf(inputNode)]);
                inputNode.SetAmountWithAmount(inputAmount);
                if(!nodesToTell.Contains(inputNode))
                {
                    nodesToTell.Add(inputNode);
                }

                maxedOutInputs[inputs.IndexOf(inputNode)] = false;
            }

            if(GetInputNodesByPriority(i, true, false, true, false).Count == 0)
            {
                i--;
            }
        }
    }

    private void TakeLess()
    {
        int i = inputs.Count - 1;
        while (i >= 0 && GetCurrentOutputAmount() < GetCurrentInputAmount())
        {
            List<InputNode> nodesToDemandLess = GetInputNodesByPriority(i, false, false, true, false);
            float surplus = GetCurrentInputAmount() - GetCurrentOutputAmount();

            foreach (InputNode inputNode in nodesToDemandLess)
            {
                float inputAmount = inputNode.GetTextAmount() - surplus / nodesToDemandLess.Count;
                inputNode.SetAmountWithAmount(inputAmount);
            }

            if (GetInputNodesByPriority(i, false, false, true, false).Count == 0)
            {
                i--;
            }
        }

        while(Mathf.Abs(GetCurrentOutputAmount() - GetCurrentInputAmount()) > Mathf.Epsilon)
        {
            List<InputNode> nodesToDemandLess = GetInputNodesByPriority(inputs.Count, false, false, true, false);
            float surplus = GetCurrentInputAmount() - GetCurrentOutputAmount();

            foreach (InputNode inputNode in nodesToDemandLess)
            {
                float inputAmount = inputNode.GetTextAmount() - surplus / nodesToDemandLess.Count;
                inputNode.SetAmountWithAmount(inputAmount);
            }
        }
    }

    private void GiveMoreToPrio1()
    {
        List<OutputNode> prio1OutputNodes = GetOutputNodesByPriority(0, false, false, true);
        float surplus = GetCurrentInputAmount() - GetCurrentOutputAmount();

        foreach (OutputNode outputNode in prio1OutputNodes)
        {
            outputNode.SetAmountWithAmount(outputNode.GetTextAmount() + surplus / prio1OutputNodes.Count);
            if(!nodesToTell.Contains(outputNode))
            {
                nodesToTell.Add(outputNode);
            }
        }
    }

    private void GiveLessExcessToOutputs()
    {
        int i = outputs.Count - 1;
        while (i >= 0 && GetCurrentOutputAmount() > GetCurrentInputAmount())
        {
            List<OutputNode> nodesToGiveLessTo = GetOutputNodesByPriority(i, false, true, false);
            float deficit = GetCurrentOutputAmount() - GetCurrentInputAmount();

            foreach(OutputNode outputNode in nodesToGiveLessTo)
            {
                float outputAmount = Mathf.Max(outputNode.GetTextAmount() - deficit / nodesToGiveLessTo.Count, wantedOutputValues[outputs.IndexOf(outputNode)]);
                outputNode.SetAmountWithAmount(outputAmount);
                if(!nodesToTell.Contains(outputNode))
                {
                    nodesToTell.Add(outputNode);
                }
            }

            if(GetOutputNodesByPriority(i, false, true, false).Count == 0)
            {
                i--;
            }
        }
    }

    private void UseMoreOffered()
    {
        int i = 0;
        while(i <= inputs.Count && GetCurrentOutputAmount() > GetCurrentInputAmount())
        {
            List<InputNode> nodesToUseMore = GetInputNodesByPriority(i, false, true, false, true);
            float deficit = GetCurrentOutputAmount() - GetCurrentInputAmount();

            foreach (InputNode inputNode in nodesToUseMore)
            {
                float inputAmount = Mathf.Min(inputNode.GetTextAmount() + deficit / nodesToUseMore.Count, offeredInputAmounts[inputs.IndexOf(inputNode)]);
                inputNode.SetAmountWithAmount(inputAmount);
            }

            if (GetInputNodesByPriority(i, false, true, false, true).Count == 0)
            {
                i++;
            }
        }
    }

    private void DemandMore()
    {
        int i = 0;
        while (i < inputs.Count && GetCurrentOutputAmount() > GetCurrentInputAmount())
        {
            List<InputNode> nodesToDemandMore = GetInputNodesByPriority(i, false, false, false, true);
            float deficit = GetCurrentOutputAmount() - GetCurrentInputAmount();

            foreach (InputNode inputNode in nodesToDemandMore)
            {
                float inputAmount = inputNode.GetTextAmount() + deficit / nodesToDemandMore.Count;
                inputNode.SetAmountWithAmount(inputAmount);
                if(!nodesToTell.Contains(inputNode))
                {
                    nodesToTell.Add(inputNode);
                }
            }

            if (GetInputNodesByPriority(i, false, false, false, true).Count == 0)
            {
                i++;
            }
        }
    }

    private void GiveLessToOutputs()
    {
        int i = outputs.Count - 1;
        while (i >= 0 && GetCurrentOutputAmount() > GetCurrentInputAmount())
        {
            List<OutputNode> nodesToGiveLessTo = GetOutputNodesByPriority(i, false, false, false);
            float deficit = GetCurrentOutputAmount() - GetCurrentInputAmount();

            foreach (OutputNode outputNode in nodesToGiveLessTo)
            {
                float outputAmount = outputNode.GetTextAmount() - deficit / nodesToGiveLessTo.Count;
                outputNode.SetAmountWithAmount(outputAmount);
                if (!nodesToTell.Contains(outputNode))
                {
                    nodesToTell.Add(outputNode);
                }
            }

            if (GetOutputNodesByPriority(i, false, false, false).Count == 0)
            {
                i--;
            }
        }
    }

    private List<InputNode> GetInputNodesByPriority(int priority, bool demandingOnly, bool offeredAmountLeftOnly, bool allowMaxedOutInputs, bool allowZeroValue)
    {
        List<InputNode> returnValue = new List<InputNode>();

        for (int i = 0; i < inputs.Count; i++)
        {
            if (inputPriorities[i] == priority
                && (!demandingOnly || inputs[i].GetTextAmount() > offeredInputAmounts[i])
                && (!offeredAmountLeftOnly || offeredInputAmounts[i] > inputs[i].GetTextAmount())
                && (allowMaxedOutInputs || !maxedOutInputs[i])
                && (allowZeroValue || Mathf.Abs(inputs[i].GetTextAmount()) > Mathf.Epsilon))
            {
                returnValue.Add(inputs[i]);
            }
        }

        return returnValue;
    }

    private List<OutputNode> GetOutputNodesByPriority(int priority, bool wantingOnly, bool oversaturatedOnly, bool allowZeroValue)
    {
        List<OutputNode> returnValue = new List<OutputNode>();

        for (int i = 0; i < outputs.Count; i++)
        {
            if(outputPriorities[i] == priority 
               && (!wantingOnly || wantedOutputValues[i] > outputs[i].GetTextAmount()) 
               && (!oversaturatedOnly || outputs[i].GetTextAmount() > wantedOutputValues[i])
               && (allowZeroValue || Mathf.Abs(outputs[i].GetTextAmount()) > Mathf.Epsilon))
            {
                returnValue.Add(outputs[i]);
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