using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveFormat
{
	public abstract string FormatName { get; }
	public abstract string Encode(Level level);
	public abstract Level Decode(string code);
}

public class V3Format : SaveFormat
{
	public override string FormatName => "V3";

	public override Level Decode(string code)
	{
		throw new System.NotImplementedException();
	}

	public override string Encode(Level level)
	{
		throw new System.NotImplementedException();
	}
} 