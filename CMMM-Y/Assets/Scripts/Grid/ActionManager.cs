using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionManager : MonoBehaviour
{
	public static ActionManager instance;

	private readonly List<Action> actions = new List<Action>();

	private int cai;
	private int CurrentActionIndex
	{
		get => cai;
		set
		{
			if (value <= -1) cai = -1;
			else if (value >= actions.Count) cai = actions.Count - 1;
			else cai = value;
		}
	}

	private void Start()
	{
		if (instance != null)
		{
			Destroy(this.gameObject);
			return;
		}
		DontDestroyOnLoad(this);
		instance = this;
	}

	private void Update()
	{
		if (SceneManager.GetActiveScene().name != "LevelScreen") Destroy(this.gameObject);

		if (ControlsManager.GetControl("Undo").GetDown()) Undo();
		if (ControlsManager.GetControl("Redo").GetDown()) Redo();
	}

	public void DoAction(Action action)
	{
		action.Execute();

		if (actions.Count > 0) actions.RemoveRange(CurrentActionIndex + 1, actions.Count - CurrentActionIndex - 1);

		actions.Add(action);
		CurrentActionIndex++;

		GridManager.hasSaved = false;
	}

	public void Undo()
	{
		if (actions.Count == 0) return;
		if (CurrentActionIndex == -1) return;

		actions[CurrentActionIndex].Undo();
		CurrentActionIndex--;

		GridManager.hasSaved = false;
	}

	public void Redo()
	{
		if (actions.Count == 0) return;
		if (CurrentActionIndex == actions.Count - 1) return;

		CurrentActionIndex++;
		actions[CurrentActionIndex].Execute();

		GridManager.hasSaved = false;
	}
}
