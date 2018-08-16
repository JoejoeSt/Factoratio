using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Creator : MonoBehaviour
{
    public GameObject prefab;
    public GameObject inputPrefab;
    public GameObject outputPrefab;
    public Slider inputSlider;
    public Slider outputSlider;

    private Placer placer;

    private void Start()
    {
        placer = Camera.main.GetComponent<Placer>();
    }

    public void CreateMiddleNode()
    {
        GameObject newObject = Instantiate(prefab, GameObject.Find("GraphArea").transform.Find("GraphNodes").transform);

        for (int i = 0; i < inputSlider.value; i++)
        {
            GameObject newInput = Instantiate(inputPrefab, newObject.transform.Find("Inputs"));

            newObject.GetComponent<GraphNode>().AddInputNode(newInput);
            newInput.GetComponent<InputNode>().SetNode(newObject.GetComponent<GraphNode>());
        }

        for (int i = 0; i < outputSlider.value; i++)
        {
            GameObject newOutput = Instantiate(outputPrefab, newObject.transform.Find("Outputs"));

            newObject.GetComponent<GraphNode>().AddOutputNode(newOutput);
            newOutput.GetComponent<OutputNode>().SetNode(newObject.GetComponent<GraphNode>());
        }

        newObject.GetComponent<GraphNode>().PositionInOutPuts();

        placer.GiveObjectToPlace(newObject);
    }

    public void CreateGraphInOutput()
    {
        GameObject newObject = Instantiate(prefab, GameObject.Find("GraphArea").transform.Find("GraphNodes").transform);

        placer.GiveObjectToPlace(newObject);
    }
}
