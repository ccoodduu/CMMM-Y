using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResizeLevel : Action
{
	private Level levelBefore;
	private Level levelAfter;

	public ResizeLevel(Vector2Int topLeft, Vector2Int bottomRight)
	{
		levelBefore = Level.FromCurrent();
		levelAfter = levelBefore.Resize(topLeft, bottomRight);
	}

	public override void Execute()
	{
		GridManager.loadString = new V3Format().Encode(levelAfter);
		GridManager.instance.Reload();
	}

	public override void Undo()
	{
		GridManager.loadString = new V3Format().Encode(levelBefore);
		GridManager.instance.Reload();
	}
}
