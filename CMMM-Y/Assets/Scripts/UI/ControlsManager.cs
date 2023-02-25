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

	public static Dictionary<string, KeyCode[]> deafultValues = new Dictionary<string, KeyCode[]>()
	{
		{"Up", new KeyCode[]{ KeyCode.W} },
		{"Down", new KeyCode[]{ KeyCode.S} },
		{"Left", new KeyCode[]{ KeyCode.A} },
		{"Right", new KeyCode[]{ KeyCode.D} },
		{"FastPan", new KeyCode[]{ KeyCode.LeftControl} },
		{"RotateCW", new KeyCode[]{ KeyCode.E} },
		{"RotateCCW", new KeyCode[]{ KeyCode.Q} },
		{"Select", new KeyCode[]{ KeyCode.LeftControl} },
		{"Paste", new KeyCode[]{ KeyCode.V} },
		{"Copy", new KeyCode[]{ KeyCode.C} },
		{"Cut", new KeyCode[]{ KeyCode.X} },
		{"Delete", new KeyCode[]{ KeyCode.Delete} },
		{"StackSelection", new KeyCode[]{ KeyCode.LeftControl, KeyCode.Mouse0} },
		{"SelectionUp", new KeyCode[]{ KeyCode.UpArrow} },
		{"SelectionDown", new KeyCode[]{ KeyCode.DownArrow} },
		{"SelectionLeft", new KeyCode[]{ KeyCode.LeftArrow} },
		{"SelectionRight", new KeyCode[]{ KeyCode.RightArrow} },
		{"Pan", new KeyCode[]{ KeyCode.Mouse2} },
	};

	public static void SetControl(string controlName, Control control)
	{
		var oldControls = PlayerPrefs.GetString(playerPrefsControls, "");
		var controlArray = oldControls.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

		var newControls = "";
		for (int i = 0; i < controlArray.Length; i++)
		{
			string c = controlArray[i];
			if (c.StartsWith(controlName + ":"))
			{
				controlArray[i] = controlName + ":" + control.ToSaveString();
				newControls = string.Join(",", controlArray);
				break;
			}
		}
		if (newControls == "")
		{
			var controlList = controlArray.ToList();
			controlList.Add(controlName + ":" + control.ToSaveString());
			newControls = string.Join(",", controlList);
		}
		PlayerPrefs.SetString(playerPrefsControls, newControls);
		Debug.Log(newControls);
	}

	public static void SetKeyForControl(string controlName, int index, KeyCode key)
	{
		var control = GetControl(controlName);
		control.Keycodes[index] = key;
		SetControl(controlName, control);
	}

	//public static KeyCode GetKeyForControl(string controlName)
	//{
	//	var controlArray = PlayerPrefs.GetString(playerPrefsControls).Split(',');

	//	for (int i = 0; i < controlArray.Length; i++)
	//	{
	//		string c = controlArray[i];
	//		if (c.StartsWith(controlName + ":"))
	//		{
	//			KeyCode keyCode = (KeyCode)int.Parse(c.Split(':')[1]);
	//			return keyCode;
	//		}
	//	}

	//	SetKeyForControl(controlName, deafultValues[controlName]);
	//	return deafultValues[controlName];
	//}

	public static Control GetControl(string controlName)
	{
		var controlArray = PlayerPrefs.GetString(playerPrefsControls).Split(',');

		for (int i = 0; i < controlArray.Length; i++)
		{
			string c = controlArray[i];
			if (c.StartsWith(controlName + ":"))
			{
				return new Control(c.Split(':')[1]);
			}
		}

		var control = new Control(deafultValues[controlName]);
		SetControl(controlName, control);
		return new Control(deafultValues[controlName]);
	}
}

public class Control
{
	public KeyCode[] Keycodes { get; }

	public Control(KeyCode[] keycodes)
	{
		this.Keycodes = keycodes;
	}

	public Control(KeyCode keycodes)
	{
		this.Keycodes = new KeyCode[] { keycodes };
	}

	public Control(string control)
	{
		var keys = control.Split('&');
		this.Keycodes = keys.Select((key) => (KeyCode)int.Parse(key)).ToArray();
	}

	public string ToSaveString()
	{
		return string.Join("&", Keycodes.Select(key => ((int)key).ToString()));
	}

	public bool Get()
	{
		foreach (KeyCode keyCode in Keycodes)
		{
			if (!Input.GetKey(keyCode)) return false;
		}
		return true;
	}

	public bool GetDown()
	{
		foreach (KeyCode keyCode in Keycodes)
		{
			if (!Input.GetKey(keyCode)) return false;
		}
		foreach (KeyCode keyCode in Keycodes)
		{
			if (Input.GetKeyDown(keyCode)) return true;
		}
		return false;
	}

	public bool GetUp()
	{
		foreach (KeyCode keyCode in Keycodes)
		{
			if (!Input.GetKey(keyCode))
			{
				if (!Input.GetKeyUp(keyCode)) return false;
			}
		}
		return true;
	}
}
