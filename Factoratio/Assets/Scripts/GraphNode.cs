using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    public List<InputNode> inputs;
    public List<OutputNode> outputs;

    void Start()
    {
        if(inputs == null)
        {
            inputs = new List<InputNode>();
        }

        if (outputs == null)
        {
            outputs = new List<OutputNode>();
        }
    }

    public void AddInputNode(GameObject input)
    {
        inputs.Add(input.GetComponent<InputNode>());
    }

    public void AddOutputNode(GameObject output)
    {
        outputs.Add(output.GetComponent<OutputNode>());
    }
}