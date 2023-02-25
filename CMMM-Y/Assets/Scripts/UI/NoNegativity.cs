using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//input validation for level width and height settings
//unity input field already has a setting to limit imputs to integers, but it also includes the '-' char

public class NoNegativity : MonoBehaviour
{
	private TMP_InputField numberInput;
    void Start()
    {
        numberInput = GetComponent<TMP_InputField>();
        numberInput.onValueChanged.AddListener(ValueChanged);
    }

    public void ValueChanged(string txt)
    {
        if (txt == "-")
        {
            numberInput.text = "";
        }
    }
}
