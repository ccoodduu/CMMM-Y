using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
	public static ActionManager instance;

	private readonly List<Action> actions = new List<Action>();
	private int currentActionIndex = -1;

	private void Start()
	{
		instance = this;
	}

	private void Update()
	{
		if (ControlsManager.GetControl("Undo").GetDown()) Undo();
		if (ControlsManager.GetControl("Redo").GetDown()) Redo();
	}

	public void DoAction(Action action)
	{
		action.Execute();

		if (actions.Count > 0) actions.RemoveRange(currentActionIndex + 1, actions.Count - (currentActionIndex + 1));

		actions.Add(action);
		currentActionIndex++;

		GridManager.hasSaved = false;
	}

	public void Undo()
	{
		if (currentActionIndex == -1) return;

		actions[currentActionIndex].Undo();
		currentActionIndex--;

		GridManager.hasSaved = false;
	}

	public void Redo()
	{
		if (currentActionIndex >= actions.Count) return;

		currentActionIndex++;
		actions[currentActionIndex].Execute();

		GridManager.hasSaved = false;
	}
}
