using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FormatManager : MonoBehaviour
{
    [SerializeField] private string selectedFormatName;
    public SaveFormat selectedFormat;
    public SaveFormat[] formats;

    public static FormatManager instance;

	private void Start()
	{
        if (instance == null) return;
		instance = this; 
    }
	
	public string SelectedFormatName
    {
        get => selectedFormatName; set
        {
            selectedFormatName = value;
            selectedFormat = formats.First(f => f.FormatName == value);
        } 
    }
    public string[] FormatNames { get => formats.Select(format => format.FormatName).ToArray(); }
}
