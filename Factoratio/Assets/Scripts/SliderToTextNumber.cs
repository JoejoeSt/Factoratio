using UnityEngine;
using UnityEngine.UI;

public class SliderToTextNumber : MonoBehaviour
{
    public string outOrInput;
    public Text text;

    public void ChangeText()
    {
        text.text = outOrInput + ": " + this.GetComponent<Slider>().value;
    }
}
