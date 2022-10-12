using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void OnChange()
    {
        if (slider.value < 30)
            slider.fillRect.GetComponent<Image>().color = Color.red;
        else if (slider.value < 70)
            slider.fillRect.GetComponent<Image>().color = Color.yellow;
        else
            slider.fillRect.GetComponent<Image>().color = Color.green;
    }
}
