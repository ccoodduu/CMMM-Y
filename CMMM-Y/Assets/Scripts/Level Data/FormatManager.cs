using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FormatManager
{
	public static SaveFormat[] formats = { new V1Format(), new V2Format(), new V3Format(), new EmojiFormat(), new ReadableFormat() };

    public static string SelectedFormatName
    {
        get => SelectedFormat.FormatName; 
    }
	public static SaveFormat SelectedFormat
	{
		get => formats[PlayerPrefs.GetInt("ExportFormat", 2)];

	}
	public static string[] FormatNames { get => formats.Select(format => format.FormatName).ToArray(); }
}
