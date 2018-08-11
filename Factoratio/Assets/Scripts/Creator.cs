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

    public void Create()
    {
        if (inputSlider != null && outputSlider != null)
        {
            CreateMiddleNode();
        }
        else
        {
            CreateGraphInOutput();
        }
    }

    private void CreateMiddleNode()
    {
        GameObject newObject = Instantiate(prefab, GameObject.Find("GraphArea").transform.Find("GraphNodes").transform);

        for (int i = 0; i < inputSlider.value; i++)
        {
            GameObject newInput = Instantiate(inputPrefab, newObject.transform.Find("Inputs"));
            newInput.GetComponent<RectTransform>().anchorMin = new Vector2(i / inputSlider.value, 0);
            newInput.GetComponent<RectTransform>().anchorMax = new Vector2((i + 1) / inputSlider.value, 1);

            newObject.GetComponent<GraphNode>().AddInputNode(newInput);
            newInput.GetComponent<InputNode>().SetNode(newObject.GetComponent<GraphNode>());
        }

        for (int i = 0; i < outputSlider.value; i++)
        {
            GameObject newOutput = Instantiate(outputPrefab, newObject.transform.Find("Outputs"));
            newOutput.GetComponent<RectTransform>().anchorMin = new Vector2(i / outputSlider.value, 0);
            newOutput.GetComponent<RectTransform>().anchorMax = new Vector2((i + 1) / outputSlider.value, 1);

            newObject.GetComponent<GraphNode>().AddOutputNode(newOutput);
            newOutput.GetComponent<OutputNode>().SetNode(newObject.GetComponent<GraphNode>());
        }

        newObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Max(inputSlider.value, outputSlider.value, 2) * 50, newObject.GetComponent<RectTransform>().sizeDelta.y);

        placer.GiveObjectToPlace(newObject);
    }

    private void CreateGraphInOutput()
    {
        GameObject newObject = Instantiate(prefab, GameObject.Find("GraphArea").transform.Find("GraphNodes").transform);

        placer.GiveObjectToPlace(newObject);
    }
}
