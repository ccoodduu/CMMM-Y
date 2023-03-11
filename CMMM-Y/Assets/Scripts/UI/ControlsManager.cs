using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using UnityEngine;

public class ControlSetting
{
	public bool canOverlap;
	public KeyCode[] defaultKeys;

	public ControlSetting(KeyCode[] defaultKeys, bool canOverlap)
	{
		this.defaultKeys = defaultKeys;
		this.canOverlap = canOverlap;
	}
}

public static class ControlsManager
{
	public static string playerPrefsControls = "Controls";

	private static Dictionary<string, Control> cache = new Dictionary<string, Control>();

	private static void RegenerateCache()
	{
		cache.Clear();

		var controlStringArray = PlayerPrefs.GetString(playerPrefsControls).Split(new char[]{ ',' }, StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < controlStringArray.Length; i++)
		{
			var control = controlStringArray[i].Split(':');

			var name = control[0];

			cache.Add(name, new Control(controlStringArray[i], allControls[name].canOverlap));
		}

		foreach (var name in allControls.Keys)
		{
			if (cache.ContainsKey(name)) continue;

			cache.Add(name, new Control(allControls[name].defaultKeys, allControls[name].canOverlap, name));
		}
	}

	public static string GetDisplayName(KeyCode keyCode)
	{
		var name = keyCode.ToString();

		if (name.StartsWith("Alpha")) name = name.Substring(5);
		if (name == "Mouse0") name = "LMB";
		if (name == "Mouse1") name = "RMB";
		if (name == "Mouse2") name = "MMB";

		// Add spaces
		name = Regex.Replace(Regex.Replace(name, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");

		return name;
	}

	public static readonly Dictionary<string, ControlSetting> allControls = new Dictionary<string, ControlSetting>()
	{
		{ "Up", new ControlSetting(new KeyCode[]{ KeyCode.W}, false)},
		{ "Down", new ControlSetting(new KeyCode[]{ KeyCode.S}, false)},
		{ "Left", new ControlSetting(new KeyCode[]{ KeyCode.A}, false)},
		{ "Right", new ControlSetting(new KeyCode[]{ KeyCode.D}, false)},
		{ "FastPan", new ControlSetting(new KeyCode[]{ KeyCode.LeftControl}, false)},
		{ "RotateCW", new ControlSetting(new KeyCode[]{ KeyCode.E}, false)},
		{ "RotateCCW", new ControlSetting(new KeyCode[]{ KeyCode.Q}, false)},
		{ "Rotate180", new ControlSetting(new KeyCode[]{ KeyCode.R}, false)},
		{ "BeginSelect", new ControlSetting(new KeyCode[]{ KeyCode.LeftControl, KeyCode.Mouse0 }, false)},
		{ "Select", new ControlSetting(new KeyCode[]{ KeyCode.Mouse0 }, true)},
		{ "CancelSelection", new ControlSetting(new KeyCode[]{ KeyCode.Mouse1 }, false)},
		{ "Paste", new ControlSetting(new KeyCode[]{ KeyCode.V}, false)},
		{ "Copy", new ControlSetting(new KeyCode[]{ KeyCode.C}, false)},
		{ "Cut", new ControlSetting(new KeyCode[]{ KeyCode.X}, false)},
		{ "Delete", new ControlSetting(new KeyCode[]{ KeyCode.Delete}, false)},
		{ "Crop", new ControlSetting(new KeyCode[]{ KeyCode.B}, false)},
		{ "StackSelection", new ControlSetting(new KeyCode[]{ KeyCode.LeftControl}, false)},
		{ "SelectionUp", new ControlSetting(new KeyCode[]{ KeyCode.UpArrow}, false)},
		{ "SelectionDown", new ControlSetting(new KeyCode[]{ KeyCode.DownArrow}, false)},
		{ "SelectionLeft", new ControlSetting(new KeyCode[]{ KeyCode.LeftArrow}, false)},
		{ "SelectionRight", new ControlSetting(new KeyCode[]{ KeyCode.RightArrow}, false)},
		{ "Pan", new ControlSetting(new KeyCode[]{ KeyCode.Mouse2}, false)},
		{ "HideUI", new ControlSetting(new KeyCode[]{ KeyCode.F1}, false)},
		{ "Debug", new ControlSetting(new KeyCode[]{ KeyCode.F3}, false)},
		{ "HighlightMoveable", new ControlSetting(new KeyCode[]{ KeyCode.Space}, false)},
		{ "Generator", new ControlSetting(new KeyCode[]{ KeyCode.Alpha1}, false)},
		{ "Mover", new ControlSetting(new KeyCode[]{ KeyCode.Alpha2}, false)},
		{ "RotatorCW", new ControlSetting(new KeyCode[]{ KeyCode.Alpha3}, false)},
		{ "RotatorCCW", new ControlSetting(new KeyCode[]{ KeyCode.Alpha4}, false)},
		{ "Push", new ControlSetting(new KeyCode[]{ KeyCode.Alpha5}, false)},
		{ "Slide", new ControlSetting(new KeyCode[]{ KeyCode.Alpha6}, false)},
		{ "Enemy", new ControlSetting(new KeyCode[]{ KeyCode.Alpha7}, false)},
		{ "Trash", new ControlSetting(new KeyCode[]{ KeyCode.Alpha8}, false)},
		{ "Immobile", new ControlSetting(new KeyCode[]{ KeyCode.Alpha9}, false)},
		{ "Placeable", new ControlSetting(new KeyCode[]{ KeyCode.Alpha0}, false)},
		{ "SelectTool", new ControlSetting(new KeyCode[]{ KeyCode.None}, false)},
		{ "DragTool", new ControlSetting(new KeyCode[]{ KeyCode.None }, false)},
		{ "PlaceCell", new ControlSetting(new KeyCode[]{ KeyCode.Mouse0 }, false)},
		{ "DragCell", new ControlSetting(new KeyCode[]{ KeyCode.Mouse0 }, false)},
		{ "DeleteCell", new ControlSetting(new KeyCode[]{ KeyCode.Mouse1 }, false)},
		{ "PickCell", new ControlSetting(new KeyCode[]{ KeyCode.LeftControl, KeyCode.Mouse2 }, false)},
		{ "PlayPause", new ControlSetting(new KeyCode[]{ KeyCode.F5 }, false)},
		{ "Reset", new ControlSetting(new KeyCode[]{ KeyCode.F6 }, false)},
		{ "Step", new ControlSetting(new KeyCode[]{ KeyCode.F7 }, false)},
		{ "Save", new ControlSetting(new KeyCode[]{ KeyCode.F2 }, false)},
		{ "SaveSelection", new ControlSetting(new KeyCode[]{ KeyCode.LeftShift, KeyCode.F2 }, false)},
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

		RegenerateCache();
	}

	public static void SetKeyForControl(string controlName, int index, KeyCode key)
	{
		var control = GetControl(controlName);
		Control newControl;
		if (key is KeyCode.None && control.Keycodes.Length > 1)
		{
			var list = control.Keycodes.ToList();
			list.RemoveAt(index);
			newControl = new Control(list.ToArray(), allControls[controlName].canOverlap, controlName);
		}
		else
		{
			var list = control.Keycodes.ToList();
			list[index] = key;
			newControl = new Control(list.ToArray(), allControls[controlName].canOverlap, controlName);
		}

		SetControl(controlName, newControl);
	}

	public static void AddKeyToControl(string controlName, KeyCode key)
	{
		var control = GetControl(controlName);

		var list = control.Keycodes.ToList();
		list.Add(key);

		Control newControl = new Control(list.ToArray(), allControls[controlName].canOverlap, controlName);

		SetControl(controlName, newControl);
	}

	public static Control GetControl(string controlName)
	{
		if (cache.Count == 0) RegenerateCache();
		return cache[controlName];
	}

	public static Control[] GetAllControlsContainingKey(KeyCode key)
	{
		if (cache.Count == 0) RegenerateCache();

		var controls = new List<Control>();

		foreach (var control in cache.Values)
		{
			if (control.Keycodes.Contains(key))
			{
				controls.Add(control);
			}
		}

		return controls.ToArray();
	}

	public static bool IsDefault(string controlName)
	{
		var control = GetControl(controlName);
		var defaultControl = new Control(allControls[controlName].defaultKeys, allControls[controlName].canOverlap, controlName);

		return control.ToSaveString() == defaultControl.ToSaveString();
	}

	public static void ResetAll()
	{
		PlayerPrefs.SetString(playerPrefsControls, "");

		RegenerateCache();
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

		RegenerateCache();
	}
}

public class Control
{
	public KeyCode[] Keycodes { get; }
	public bool CanOverlap { get; }
	public string Name { get; }

	public Control(KeyCode[] keycodes, bool canOverlap, string name)
	{
		this.Keycodes = keycodes;
		this.CanOverlap = canOverlap;
		Name = name;
	}

	public Control(KeyCode keycode, bool canOverlap, string name)
	{
		this.Keycodes = new KeyCode[] { keycode };
		this.CanOverlap = canOverlap;
		this.Name = name;
	}

	public Control(string control, bool canOverlap)
	{
		var s = control.Split(':');
		var keys = s[1].Split('&');

		this.Keycodes = keys.Select((key) => (KeyCode)int.Parse(key)).ToArray();
		this.CanOverlap = canOverlap;
		this.Name = s[0];
	}

	public string ToSaveString()
	{
		return string.Join("&", Keycodes.Select(key => ((int)key).ToString()));
	}

	private bool Overlaps()
	{
		if (CanOverlap) return false;

		foreach (var key in this.Keycodes)
		{
			foreach (var control in ControlsManager.GetAllControlsContainingKey(key))
			{
				if (control.Name == Name) continue;
				if (control.Keycodes.Length > this.Keycodes.Length && !control.CanOverlap && control.Get(false)) return true;
			}
		}

		return false;
	}

	public bool Get()
	{
		return Get(true);
	}

	public bool Get(bool checkForOverlap)
	{
		if (checkForOverlap && Overlaps()) return false;

		foreach (KeyCode keyCode in Keycodes)
		{
			if (!Input.GetKey(keyCode)) return false;
		}
		return true;
	}

	public bool GetDown()
	{
		if (Overlaps()) return false;

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
		if (Overlaps()) return false;

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
