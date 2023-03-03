using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Linq;

namespace load
{
	public static class LoadString
	{
		public static bool Load(string str)
		{
			var formatName = str.Split(';')[0];
			var format = FormatManager.formats.First(f => f.FormatName.ToLower() == formatName.ToLower());
			var level = format.Decode(str);
			level.LoadToGrid();

			return true;
		}
	}
}
