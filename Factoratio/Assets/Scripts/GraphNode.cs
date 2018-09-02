using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphNode : MonoBehaviour
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
        if ((mouseOver && Input.GetMouseButtonDown(1) && Input.touchCount == 0))
        {
            Reposition();
        }
        else if(Input.touchCount == 1 && Input.GetTouch(0).radius > 30 &&TouchOnThis())
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

    public void ClearConnections()
    {
        foreach (InputNode input in inputs)
        {
            input.ClearConnection();
        }

        foreach (OutputNode output in outputs)
        {
            output.ClearConnection();
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

    private bool TouchOnThis()
    {
        Vector2 camPosition = Camera.main.transform.position;

        Vector2 positionOfTouchRelativeToCenter = Input.GetTouch(0).position - new Vector2(Screen.width / 2, Screen.height / 2);
        float zoomFactor = (float)Screen.height / 2 / Camera.main.orthographicSize;
        Vector2 touchPositionRelativeToCam = positionOfTouchRelativeToCenter / zoomFactor;

        Vector2 touchPosition = camPosition + touchPositionRelativeToCam;

        float x = this.GetComponent<RectTransform>().position.x;
        float width = this.GetComponent<RectTransform>().rect.width;
        float y = this.GetComponent<RectTransform>().position.y;
        float height = this.GetComponent<RectTransform>().rect.height;

        return x - width/2 <= touchPosition.x && x + width/2 >= touchPosition.x && y - height/2 <= touchPosition.y && y + height/2 >= touchPosition.y;
    }

    public abstract void Calculate(InOutNode changeingNode, float wantedValue);
}