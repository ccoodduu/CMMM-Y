using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ActionManager
{
	private static List<Action> actions = new List<Action>();
	private static int currentActionIndex = -1;
	public static void DoAction(Action action)
	{
		action.Execute();
		if (actions.Count > 0) actions.RemoveRange(currentActionIndex, actions.Count - currentActionIndex);
		actions.Add(action);
		currentActionIndex++;

		GridManager.hasSaved = false;
	}

	public static void Undo()
	{
		actions[currentActionIndex].Undo();
		currentActionIndex--;

		GridManager.hasSaved = false;
	}

	public static void Redo()
	{
		currentActionIndex++;
		actions[currentActionIndex].Execute();

		GridManager.hasSaved = false;
	}
}
