using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connector : MonoBehaviour
{
    private GameObject start;
    private Vector2 startConnectionPoint;
    public GameObject connectingLinePrefab;

    public void Connect(GameObject node, Vector3 connectionPoint)
    {
        node.GetComponent<InOutNode>().ClearConnection();

        if(start == null || start == node)
        {
            start = node;
            startConnectionPoint = connectionPoint;
        }
        else
        {
            if(start.GetComponent<InputNode>() == node.GetComponent<InputNode>() || start.GetComponent<OutputNode>() == node.GetComponent<OutputNode>() || start.GetComponent<InOutNode>().graphNode == node.GetComponent<InOutNode>().graphNode)
            {
                start.GetComponent<InOutNode>().ColorItemField(Color.white);
                if(start.GetComponent<InOutNode>().GetCounterpart() != null)
                {
                    start.GetComponent<InOutNode>().GetCounterpart().ColorItemField(Color.white);
                }

                start = node;
                startConnectionPoint = connectionPoint;
            }
            else
            {
                ConnectNodes(node, connectionPoint);
                start = null;
            }
        }
    }

    private void ConnectNodes(GameObject secondNode, Vector2 secondConnectionPoint)
    {
        start.GetComponent<InOutNode>().SetCounterpart(secondNode.GetComponent<InOutNode>());
        secondNode.GetComponent<InOutNode>().SetCounterpart(start.GetComponent<InOutNode>());

        GameObject newConnectingLine = CreateConnector(secondNode, secondConnectionPoint);

        start.GetComponent<InOutNode>().SetConnectingLine(newConnectingLine);
        secondNode.GetComponent<InOutNode>().SetConnectingLine(newConnectingLine);

        start.GetComponent<InOutNode>().CheckItemName();
    }

    public void Reconnect(GameObject firstNode, Vector2 firstConnectionPoint, GameObject secondNode, Vector2 secondConnectionPoint)
    {
        start = firstNode;
        startConnectionPoint = firstConnectionPoint;
        GameObject newConnectingLine = CreateConnector(secondNode, secondConnectionPoint);

        start.GetComponent<InOutNode>().SetConnectingLine(newConnectingLine);
        secondNode.GetComponent<InOutNode>().SetConnectingLine(newConnectingLine);
    }

    private GameObject CreateConnector(GameObject secondNode, Vector2 secondConnectionPoint)
    {
        if(start.GetComponent<InputNode>() != null)
        {
            if(startConnectionPoint.y < secondConnectionPoint.y)
            {
                return CreateStraightConnector(startConnectionPoint, secondConnectionPoint);
            }
            else
            {
                return CreateZigzagConnector(start, startConnectionPoint, secondNode, secondConnectionPoint);
            }
        }
        else
        {
            if (startConnectionPoint.y > secondConnectionPoint.y)
            {
                return CreateStraightConnector(startConnectionPoint, secondConnectionPoint);
            }
            else
            {
                return CreateZigzagConnector(secondNode, secondConnectionPoint, start, startConnectionPoint);
            }
        }
    }

    private GameObject CreateStraightConnector(Vector2 firstConnectionPoint, Vector2 secondConnectionPoint)
    {
        GameObject newConnectingLine = Instantiate(connectingLinePrefab, GameObject.Find("Connectors").transform);

        Vector2 differenceVector = secondConnectionPoint - firstConnectionPoint;

        newConnectingLine.GetComponent<RectTransform>().sizeDelta = new Vector2(differenceVector.magnitude, 10);
        newConnectingLine.GetComponent<RectTransform>().position = firstConnectionPoint;
        float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
        newConnectingLine.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, angle);

        return newConnectingLine;
    }

    private GameObject CreateZigzagConnector(GameObject input, Vector2 inputConnectionPoint, GameObject output, Vector2 outputConnectionPoint)
    {
        GameObject newConnectingLineContainer = new GameObject("ConnectingLineContainer (Clone)");
        newConnectingLineContainer.transform.parent = GameObject.Find("Connectors").transform;

        List<Vector2> zigzagConnectionPoints = SetZigzagConnectionPoints(input, inputConnectionPoint, output, outputConnectionPoint);
        
        for (int i = 0; i < zigzagConnectionPoints.Count - 1; i++)
        {
            GameObject newConnectingLine;
            if (i != zigzagConnectionPoints.Count - 2)
            {
                Vector2 differenceVector = zigzagConnectionPoints[i + 1] - zigzagConnectionPoints[i];
                newConnectingLine = CreateStraightConnector(zigzagConnectionPoints[i], zigzagConnectionPoints[i + 1] + 5 * differenceVector.normalized);
            }
            else
            {
                newConnectingLine = CreateStraightConnector(zigzagConnectionPoints[i], zigzagConnectionPoints[i + 1]);
            }
            newConnectingLine.transform.SetParent(newConnectingLineContainer.transform);
        }

        return newConnectingLineContainer;
    }

    private List<Vector2> SetZigzagConnectionPoints(GameObject input, Vector2 inputConnectionPoint, GameObject output, Vector2 outputConnectionPoint)
    {
        GameObject graphNodeOfInput;
        int graphNodeOfInputCount;
        int indexOfInputInGraphNode;

        GameObject graphNodeOfOutput;
        int graphNodeOfOutputCount;
        int indexOfOutputInGraphNode;

        if (input.transform.parent.GetComponent<GraphNode>() != null)
        {
            graphNodeOfInput = input.transform.parent.gameObject;
            graphNodeOfInputCount = graphNodeOfInput.GetComponent<GraphNode>().inputs.Count;
            indexOfInputInGraphNode = graphNodeOfInput.GetComponent<GraphNode>().inputs.IndexOf(input.GetComponent<InputNode>());
        }
        else
        {
            graphNodeOfInput = input.transform.parent.parent.gameObject;
            graphNodeOfInputCount = graphNodeOfInput.GetComponent<GraphNode>().inputs.Count;
            indexOfInputInGraphNode = graphNodeOfInput.GetComponent<GraphNode>().inputs.IndexOf(input.GetComponent<InputNode>());
        }

        if(output.transform.parent.GetComponent<GraphNode>() != null)
        {
            graphNodeOfOutput = output.transform.parent.gameObject;
            graphNodeOfOutputCount = graphNodeOfOutput.GetComponent<GraphNode>().outputs.Count;
            indexOfOutputInGraphNode = graphNodeOfOutput.GetComponent<GraphNode>().outputs.IndexOf(output.GetComponent<OutputNode>());
        }
        else
        {
            graphNodeOfOutput = output.transform.parent.parent.gameObject;
            graphNodeOfOutputCount = graphNodeOfOutput.GetComponent<GraphNode>().outputs.Count;
            indexOfOutputInGraphNode = graphNodeOfOutput.GetComponent<GraphNode>().outputs.IndexOf(output.GetComponent<OutputNode>());
        }

        List<Vector2> zigzagConnectionPoints = new List<Vector2>();
        zigzagConnectionPoints.Add(inputConnectionPoint);

        if (inputConnectionPoint.x < outputConnectionPoint.x)
        {        
            if(graphNodeOfInputCount - 1 - indexOfInputInGraphNode != 0)
            {
                zigzagConnectionPoints.Add(inputConnectionPoint + new Vector2(0, 10 * (graphNodeOfInputCount - 1 - indexOfInputInGraphNode)));
                zigzagConnectionPoints.Add(zigzagConnectionPoints[zigzagConnectionPoints.Count - 1] + new Vector2(graphNodeOfInput.GetComponent<RectTransform>().rect.width * (graphNodeOfInputCount - indexOfInputInGraphNode) / graphNodeOfInputCount, 0));
            }
            else
            {
                zigzagConnectionPoints.Add(inputConnectionPoint + new Vector2(graphNodeOfInput.GetComponent<RectTransform>().rect.width * (graphNodeOfInputCount - indexOfInputInGraphNode) / graphNodeOfInputCount, 0));
            }

            if(indexOfOutputInGraphNode != 0)
            {
                Vector2 pointStraightUnderOutput = outputConnectionPoint - new Vector2(0, 10 * indexOfOutputInGraphNode);
                zigzagConnectionPoints.Add(pointStraightUnderOutput - new Vector2(graphNodeOfOutput.GetComponent<RectTransform>().rect.width * (indexOfOutputInGraphNode + 1) / graphNodeOfOutputCount, 0));
                zigzagConnectionPoints.Add(pointStraightUnderOutput);
            }
            else
            {
                zigzagConnectionPoints.Add(outputConnectionPoint - new Vector2(graphNodeOfOutput.GetComponent<RectTransform>().rect.width * (indexOfOutputInGraphNode + 1) / graphNodeOfOutputCount, 0));
            }
        }
        else
        {
            if(indexOfInputInGraphNode != 0)
            {
                zigzagConnectionPoints.Add(inputConnectionPoint + new Vector2(0, 10 * indexOfInputInGraphNode));
                zigzagConnectionPoints.Add(zigzagConnectionPoints[zigzagConnectionPoints.Count - 1] - new Vector2(graphNodeOfInput.GetComponent<RectTransform>().rect.width * (indexOfInputInGraphNode + 1) / graphNodeOfInputCount, 0));
            }
            else
            {
                zigzagConnectionPoints.Add(inputConnectionPoint - new Vector2(graphNodeOfInput.GetComponent<RectTransform>().rect.width * (indexOfInputInGraphNode + 1) / graphNodeOfInputCount, 0));
            }
            
            if(graphNodeOfOutputCount - 1 - indexOfOutputInGraphNode != 0)
            {
                Vector2 pointStraightUnderOutput = outputConnectionPoint - new Vector2(0, 10 * (graphNodeOfOutputCount - 1 - indexOfOutputInGraphNode));
                zigzagConnectionPoints.Add(zigzagConnectionPoints[4] + new Vector2(graphNodeOfOutput.GetComponent<RectTransform>().rect.width * (graphNodeOfOutputCount - indexOfOutputInGraphNode) / graphNodeOfOutputCount, 0));
                zigzagConnectionPoints.Add(pointStraightUnderOutput);
            }
            else
            {
                zigzagConnectionPoints.Add(outputConnectionPoint + new Vector2(graphNodeOfOutput.GetComponent<RectTransform>().rect.width * (graphNodeOfOutputCount - indexOfOutputInGraphNode) / graphNodeOfOutputCount, 0));
            }
        }

        zigzagConnectionPoints.Add(outputConnectionPoint);

        return zigzagConnectionPoints;
    }
}