using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public static class ControlsManager
{
	public static string playerPrefsControls = "Controls";

	public static string GetDisplayName(KeyCode keyCode)
	{
		var name = keyCode.ToString();
		if (name.StartsWith("Alpha")) name = name.Substring(5);
		return name;
	}

	public static readonly Dictionary<string, KeyCode[]> defaultControls = new Dictionary<string, KeyCode[]>()
	{
		{ "Up", new KeyCode[]{ KeyCode.W} },
		{ "Down", new KeyCode[]{ KeyCode.S} },
		{ "Left", new KeyCode[]{ KeyCode.A} },
		{ "Right", new KeyCode[]{ KeyCode.D} },
		{ "FastPan", new KeyCode[]{ KeyCode.LeftControl} },
		{ "RotateCW", new KeyCode[]{ KeyCode.E} },
		{ "RotateCCW", new KeyCode[]{ KeyCode.Q} },
		{ "Rotate180", new KeyCode[]{ KeyCode.R} },
		{ "BeginSelect", new KeyCode[]{ KeyCode.LeftControl, KeyCode.Mouse0 } },
		{ "Select", new KeyCode[]{ KeyCode.Mouse0 } },
		{ "CancelSelection", new KeyCode[]{ KeyCode.Mouse1 } },
		{ "Paste", new KeyCode[]{ KeyCode.V} },
		{ "Copy", new KeyCode[]{ KeyCode.C} },
		{ "Cut", new KeyCode[]{ KeyCode.X} },
		{ "Delete", new KeyCode[]{ KeyCode.Delete} },
		{ "Crop", new KeyCode[]{ KeyCode.B} },
		{ "StackSelection", new KeyCode[]{ KeyCode.LeftControl} },
		{ "SelectionUp", new KeyCode[]{ KeyCode.UpArrow} },
		{ "SelectionDown", new KeyCode[]{ KeyCode.DownArrow} },
		{ "SelectionLeft", new KeyCode[]{ KeyCode.LeftArrow} },
		{ "SelectionRight", new KeyCode[]{ KeyCode.RightArrow} },
		{ "Pan", new KeyCode[]{ KeyCode.Mouse2} },
		{ "HideUI", new KeyCode[]{ KeyCode.F1} },
		{ "Debug", new KeyCode[]{ KeyCode.F3} },
		{ "HighlightMoveable", new KeyCode[]{ KeyCode.Space} },
		{ "Generator", new KeyCode[]{ KeyCode.Alpha1} },
		{ "Mover", new KeyCode[]{ KeyCode.Alpha2} },
		{ "RotatorCW", new KeyCode[]{ KeyCode.Alpha3} },
		{ "RotatorCCW", new KeyCode[]{ KeyCode.Alpha4} },
		{ "Push", new KeyCode[]{ KeyCode.Alpha5} },
		{ "Slide", new KeyCode[]{ KeyCode.Alpha6} },
		{ "Enemy", new KeyCode[]{ KeyCode.Alpha7} },
		{ "Trash", new KeyCode[]{ KeyCode.Alpha8} },
		{ "Immobile", new KeyCode[]{ KeyCode.Alpha9} },
		{ "Placeable", new KeyCode[]{ KeyCode.Alpha0} },
		{ "SelectTool", new KeyCode[]{ KeyCode.None} },
		{ "DragTool", new KeyCode[]{ KeyCode.None } },
		{ "PlaceCell", new KeyCode[]{ KeyCode.Mouse0 } },
		{ "DragCell", new KeyCode[]{ KeyCode.Mouse0 } },
		{ "DeleteCell", new KeyCode[]{ KeyCode.Mouse1 } },
		{ "PlayPause", new KeyCode[]{ KeyCode.C } },
		{ "Reset", new KeyCode[]{ KeyCode.X } },
		{ "Step", new KeyCode[]{ KeyCode.V } },
		{ "Save", new KeyCode[]{ KeyCode.F2 } },
		{ "SaveSelection", new KeyCode[]{ KeyCode.LeftShift, KeyCode.F2 } },
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
	}

	public static void SetKeyForControl(string controlName, int index, KeyCode key)
	{
		var control = GetControl(controlName);
		Control newControl;
		if (key is KeyCode.None && control.Keycodes.Length > 1)
		{
			var list = control.Keycodes.ToList();
			list.RemoveAt(index);
			newControl = new Control(list.ToArray());
		}
		else
		{
			var list = control.Keycodes.ToList();
			list[index] = key;
			newControl = new Control(list.ToArray());
		}

		SetControl(controlName, newControl);
	}

	public static void AddKeyToControl(string controlName, KeyCode key)
	{
		var control = GetControl(controlName);

		var list = control.Keycodes.ToList();
		list.Add(key);

		Control newControl = new Control(list.ToArray());

		SetControl(controlName, newControl);
	}

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

		if (!defaultControls.ContainsKey(controlName))
		{
			Debug.LogWarning("Control: " + controlName + " does not have a default control!");
			return new Control(KeyCode.None);
		}

		var control = new Control(defaultControls[controlName]);
		SetControl(controlName, control);
		return new Control(defaultControls[controlName]);
	}

	public static bool IsDefault(string controlName)
	{
		var control = GetControl(controlName);
		var defaultControl = new Control(defaultControls[controlName]);

		return control.ToSaveString() == defaultControl.ToSaveString();
	}

	public static void ResetAll()
	{
		PlayerPrefs.SetString(playerPrefsControls, "");
	}

	public static void Reset(string controlName)
	{
		var oldControls = PlayerPrefs.GetString(playerPrefsControls, "");
		var controlArray = oldControls.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

		var newControls = "";
		for (int i = 0; i < controlArray.Length; i++)
		{
			string c = controlArray[i];
			if (c.StartsWith(controlName + ":"))
			{
				var list = controlArray.ToList();
				list.RemoveAt(i);
				newControls = string.Join(",", list.ToArray());
				break;
			}
		}

		PlayerPrefs.SetString(playerPrefsControls, newControls);
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
		bool isReleased = false;
		foreach (KeyCode keyCode in Keycodes)
		{
			if (Input.GetKeyUp(keyCode)) isReleased = true;

			if (!(Input.GetKey(keyCode) || Input.GetKeyUp(keyCode)))
			{
				return false;
			}
		}
		if (!isReleased) return false;

		return true;
	}
}
