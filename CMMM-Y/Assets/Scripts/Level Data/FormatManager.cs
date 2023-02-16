using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public static class FormatManager
{
    private static string selectedFormatName = "Emoji";
	private static SaveFormat selectedFormat;
	public static SaveFormat[] formats = { new V1Format(), new V2Format(), new V3Format(), new EmojiFormat(), new ReadableFormat() };

    public static string SelectedFormatName
    {
        get => selectedFormatName ?? selectedFormat.FormatName; 
        set
        {
            selectedFormatName = value;
            selectedFormat = formats.First(f => f.FormatName == value);
        } 
    }
	public static SaveFormat SelectedFormat
	{
		get => selectedFormat ?? formats.First(f => f.FormatName == selectedFormatName);
		set
		{
			selectedFormat = value;
			selectedFormatName = value.FormatName;
		}
	}
	public static string[] FormatNames { get => formats.Select(format => format.FormatName).ToArray(); }
}
