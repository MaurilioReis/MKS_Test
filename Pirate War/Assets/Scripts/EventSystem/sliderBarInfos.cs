using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sliderBarInfos : MonoBehaviour
{
    public Slider slider;
    public TMP_Text text;
    public string amountNumber = "00";

    void Update()
    {
        text.text = slider.value.ToString(amountNumber);
    }
}
