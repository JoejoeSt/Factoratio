using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    private List<TargetOutput> targets;

    public void Calculate()
    { 
        FillList();

        foreach (TargetOutput target in targets)
        {
            target.Calculate();
        }
    }

    private void FillList()
    {
        targets = new List<TargetOutput>();

        foreach (GameObject targetOutput in GameObject.FindGameObjectsWithTag("TargetOutput"))
        {
            targets.Add(targetOutput.GetComponent<TargetOutput>());
        }
    }
}
