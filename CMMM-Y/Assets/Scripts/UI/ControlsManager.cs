using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ControlsManager
{
	public static string playerPrefsControls = "Controls";

	public static string GetDisplayName(KeyCode keyCode)
	{
		return keyCode.ToString();
	}

	public static Dictionary<string, KeyCode> deafultValues = new Dictionary<string, KeyCode>()
	{
		{"Up", KeyCode.W },
		{"Down", KeyCode.S },
		{"Left", KeyCode.A },
		{"Right", KeyCode.D },
		{"RotateCW", KeyCode.E },
		{"RotateCCW", KeyCode.Q },
		{"Select", KeyCode.LeftControl },
		{"Paste", KeyCode.V },
		{"Copy", KeyCode.C },
		{"Cut", KeyCode.X },
		{"Delete", KeyCode.Delete },
		{"StackSelection", KeyCode.LeftControl },
		{"SelectionUp", KeyCode.UpArrow },
		{"SelectionDown", KeyCode.DownArrow },
		{"SelectionLeft", KeyCode.LeftArrow },
		{"SelectionRight", KeyCode.RightArrow },

	};

	public static void SetKeyForControl(string control, KeyCode key)
	{
		var oldControls = PlayerPrefs.GetString(playerPrefsControls, "");
		var controlArray = oldControls.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

		var newControls = "";
		for (int i = 0; i < controlArray.Length; i++)
		{
			string c = controlArray[i];
			if (c.StartsWith(control + ":"))
			{
				controlArray[i] = control + ":" + (int)key;
				newControls = string.Join(",", controlArray);
				break;
			}
		}
		if (newControls == "")
		{
			var controlList = controlArray.ToList();
			controlList.Add(control + ":" + (int)key);
			newControls = string.Join(",", controlList);
		}
		PlayerPrefs.SetString(playerPrefsControls, newControls);
		Debug.Log(newControls);
	}

	public static KeyCode GetKeyForControl(string control)
	{
		var controlArray = PlayerPrefs.GetString(playerPrefsControls).Split(',');

		for (int i = 0; i < controlArray.Length; i++)
		{
			string c = controlArray[i];
			if (c.StartsWith(control + ":"))
			{
				KeyCode keyCode = (KeyCode)int.Parse(c.Split(':')[1]);
				return keyCode;
			}
		}

		SetKeyForControl(control, deafultValues[control]);
		return deafultValues[control];
	}
}
