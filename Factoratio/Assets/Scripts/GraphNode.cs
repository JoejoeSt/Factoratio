using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    public List<InputNode> inputs;
    public List<OutputNode> outputs;

    private bool mouseOver;

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

        mouseOver = false;
    }

    private void Update()
    {
        if (mouseOver && Input.GetMouseButtonDown(1))
        {
            Reposition();
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

    public void PositionInOutPuts()
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            inputs[i].GetComponent<RectTransform>().anchorMin = new Vector2((float) i / inputs.Count, 0);
            inputs[i].GetComponent<RectTransform>().anchorMax = new Vector2((float) (i + 1) / inputs.Count, 1);
        }

        for (int i = 0; i < outputs.Count; i++)
        {
            outputs[i].GetComponent<RectTransform>().anchorMin = new Vector2((float) i / outputs.Count, 0);
            outputs[i].GetComponent<RectTransform>().anchorMax = new Vector2((float) (i + 1) / outputs.Count, 1);
        }

        this.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Max(inputs.Count, outputs.Count, 2) * 50, this.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void Reconnect()
    {
        foreach (InputNode input in inputs)
        {
            input.Reconnect();
        }

        foreach (OutputNode output in outputs)
        {
            output.Reconnect();
        }
    }

    public void Reposition()
    {
        Camera.main.GetComponent<Placer>().GiveObjectToPlace(this.gameObject);
    }

    public void MouseEnter()
    {
        mouseOver = true;
    }

    public void MouseExit()
    {
        mouseOver = false;
    }
}