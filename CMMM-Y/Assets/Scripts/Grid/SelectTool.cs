using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.UI;

public class SelectTool : MonoBehaviour
{
	public enum State_e
	{
		IDLE,
		SELECT,
		PREVIEW
	}

	static public List<Cell> selectedCells;
	static public List<Cell> clipboardCells;
	static public List<Cell> previewCells;

	public State_e state;

	public Material basicMat;
	public Material selectedMat;

	public SpriteRenderer sprRend;

	public GameObject selectButton;
	public GameObject moveButton;
	public GameObject toolbox;

	public Save save;

	Vector2Int initialPos;
	Vector2Int copyOffset;
	Vector2Int min;
	Vector2Int max;
	Vector2 toolboxPos;

	void Awake()
	{
		selectedCells = new List<Cell>();
		clipboardCells = new List<Cell>();
		previewCells = new List<Cell>();
		state = State_e.IDLE;
	}


	Vector2Int MousePos()
	{
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		return Vector2Int.RoundToInt((Vector2)worldPoint);
	}

	Vector2Int ClampedMousePos()
	{
		Vector2Int pos = MousePos();
		pos.Clamp(Vector2Int.zero, new Vector2Int(CellFunctions.gridWidth - 1, CellFunctions.gridHeight - 1));
		return pos;
	}

	void Select(Vector2Int cornerA, Vector2Int cornerB)
	{
		min = Vector2Int.Min(cornerA, cornerB);
		max = Vector2Int.Max(cornerA, cornerB);

		selectedCells = new List<Cell>();
		for (int x = min.x; x <= max.x; x++)
		{
			for (int y = min.y; y <= max.y; y++)
			{
				if (CellFunctions.cellGrid[x, y] != null)
				{
					selectedCells.Add(CellFunctions.cellGrid[x, y]);
				}
			}
		}
		//selection box
		transform.position = (Vector3)(Vector2)(max + min) / 2;
		sprRend.size = (Vector2)(Vector2Int.one + max - min);
	}

	void DeleteSelected()
	{
		// careful, even if the cells aren't on the cell grid, it will set elements of cellGrid to null
		foreach (Cell cell in selectedCells)
		{
			cell.Delete(true);
		}
		selectedCells = new List<Cell>();
	}

	void CopySelected(Vector2Int reference)
	{
		foreach (Cell cell in clipboardCells)
		{
			cell.gameObject.SetActive(false);
			Destroy(cell.gameObject);
		}
		clipboardCells = new List<Cell>();
		foreach (Cell cell in selectedCells)
		{
			Cell newCell = Instantiate(cell);
			newCell.position = cell.position + (Vector2)reference;
			newCell.oldPosition = newCell.position;
			newCell.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .5f);
			clipboardCells.Add(newCell);
		}
	}

	void PasteClipboard()
	{
		foreach (Cell cell in clipboardCells)
		{
			if (cell.position.x >= CellFunctions.gridWidth || cell.position.x < 0)
				continue;
			if (cell.position.y >= CellFunctions.gridHeight || cell.position.y < 0)
				continue;
			GridManager.instance.SpawnCell(cell.cellType, cell.position, (Direction_e)cell.rotation, false);
		}
		GridManager.hasSaved = false;
	}

	void Stack(Vector2Int offset, bool cut)
	{
		CopySelected(offset);
		if (cut)
			DeleteSelected();
		PasteClipboard();
		Select(min + offset, max + offset);
		CopySelected(MousePos() - (min + max) / 2);
		copyOffset = MousePos();
	}

	void RotateClipboard(Vector2Int pivot, bool counterClockwise)
	{
		int rotDir = counterClockwise ? 1 : -1;
		foreach (Cell cell in clipboardCells)
		{
			cell.rotation = (cell.rotation - rotDir) % 4;
			if (cell.rotation < 0)
				cell.rotation = 3;

			cell.transform.rotation = Quaternion.Euler(0, 0, cell.rotation * -90);
			cell.oldPosition = Vector2.Perpendicular((cell.oldPosition - (Vector2)pivot) * rotDir) + (Vector2)pivot;
		}
	}

	// functions that are ment to be called with hotkeys and buttons

	public void Delete()
	{
		AudioManager.i.PlaySound(GameAssets.i.destroy);
		DeleteSelected();
		state = State_e.IDLE;
	}
	public void Copy()
	{
		CopySelected(MousePos() - (min + max) / 2);
		copyOffset = MousePos();
		state = State_e.PREVIEW;
	}
	public void Cut()
	{
		CopySelected(MousePos() - (min + max) / 2);
		DeleteSelected();
		copyOffset = MousePos();
		state = State_e.PREVIEW;
	}
	public void SaveSelection()
	{
		save.SaveLevel(new Vector2Int(min.x, max.y), new Vector2Int(max.x, min.y));
	}
	public void Crop()
	{
		var level = Level.FromCurrent().Crop(new Vector2Int(min.x, max.y), new Vector2Int(max.x, min.y)); ;
		GridManager.loadString = new V3Format().Encode(level);
		GridManager.instance.Reload();
	}

	void Update()
	{
		// if you select an area or if you use the paste hotkey
		if (ControlsManager.GetControl("BeginSelect").GetDown() || ControlsManager.GetControl("Paste").GetDown())
			if (GridManager.mode.IsEditor())
				selectButton.GetComponent<EditorButtons>().SwitchTool();

		//State Management
		if (GridManager.tool == Tool_e.SELECT)
		{
			if (ControlsManager.GetControl("Paste").GetDown())
				state = State_e.PREVIEW;
			if (ControlsManager.GetControl("Select").GetDown())
			{
				if (state == State_e.IDLE && !EventSystem.current.IsPointerOverGameObject())
					state = State_e.SELECT;
			}
		}
		else
			state = State_e.IDLE;

		if (GridManager.tool == Tool_e.SELECT)
		{
			//if (state != State_e.PREVIEW && ("q") || "e")))
			//    moveButton.GetComponent<EditorButtons>().switchTool();
			if (state == State_e.IDLE && ControlsManager.GetControl("CancelSelection").GetDown())
				moveButton.GetComponent<EditorButtons>().SwitchTool();
		}
		if (!GridManager.clean || ControlsManager.GetControl("CancelSelection").Get())
			state = State_e.IDLE;

		//End of State Management


		if (state != State_e.SELECT)
		{
			sprRend.enabled = false;
			toolbox.SetActive(false);
		}

		foreach (Cell cell in selectedCells)
		{
			if (cell != null)
				cell.GetComponent<SpriteRenderer>().material = basicMat;
		}

		if (state == State_e.SELECT)
		{
			if (ControlsManager.GetControl("Select").Get())
			{
				if (!EventSystem.current.IsPointerOverGameObject())
				{
					if (ControlsManager.GetControl("Select").GetDown())
					{
						initialPos = ClampedMousePos();
						sprRend.enabled = true;
					}
					toolbox.SetActive(false);
					Select(initialPos, ClampedMousePos());
					if (PlayerPrefs.GetInt("Selection Toolbox", 1) == 1)
						toolboxPos = (Vector3)((Vector2)min - new Vector2(0.5f, 0.5f));
					else if (PlayerPrefs.GetInt("Selection Toolbox", 1) == 2)
						toolboxPos = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
				}
			}
			else
			{
				//area has been 
				var doStackSelection = ControlsManager.GetControl("StackSelection").Get();

				if (PlayerPrefs.GetInt("Selection Toolbox", 1) != 0)
				{
					toolbox.SetActive(true);
					toolbox.transform.position = Camera.main.WorldToScreenPoint(toolboxPos);
				}

				if (ControlsManager.GetControl("Delete").GetDown())
				{
					Delete();
				}
				else if (ControlsManager.GetControl("Copy").GetDown())
				{
					Copy();
				}
				else if (ControlsManager.GetControl("Cut").GetDown())
				{
					Cut();
				}
				else if (ControlsManager.GetControl("SaveSelection").GetDown())
				{
					SaveSelection();
				}
				else if (ControlsManager.GetControl("Crop").GetDown())
				{
					Crop();
				}

				else if (ControlsManager.GetControl("SelectionUp").GetDown())
				{
					Vector2Int offset = new Vector2Int(0, doStackSelection ? (max.y + 1) - min.y : 1);
					if (max.y + offset.y < CellFunctions.gridHeight)
						Stack(offset, !doStackSelection);
				}

				else if (ControlsManager.GetControl("SelectionDown").GetDown())
				{
					Vector2Int offset = new Vector2Int(0, doStackSelection ? min.y - (max.y + 1) : -1);
					if (min.y + offset.y >= 0)
						Stack(offset, !doStackSelection);
				}

				else if (ControlsManager.GetControl("SelectionRight").GetDown())
				{
					Vector2Int offset = new Vector2Int(doStackSelection ? (max.x + 1) - min.x : 1, 0);
					if (max.x + offset.x < CellFunctions.gridHeight)
						Stack(offset, !doStackSelection);
				}

				else if (ControlsManager.GetControl("SelectionLeft").GetDown())
				{
					Vector2Int offset = new Vector2Int(doStackSelection ? min.x - (max.x + 1) : -1, 0);
					if (min.x + offset.x >= 0)
						Stack(offset, !doStackSelection);
				}
			}

			foreach (Cell cell in selectedCells)
			{
				cell.GetComponent<SpriteRenderer>().material = selectedMat;
			}

		}

		if (state == State_e.PREVIEW)
		{
			Vector2 offset = (Vector2)(MousePos() - copyOffset);
			foreach (Cell cell in clipboardCells)
			{
				cell.gameObject.SetActive(true);
				cell.position = cell.oldPosition + offset;
				cell.transform.position = cell.position;
			}

			if (ControlsManager.GetControl("PlaceCell").GetDown() && !EventSystem.current.IsPointerOverGameObject())
			{
				AudioManager.i.PlaySound(GameAssets.i.place);
				PasteClipboard();
				state = State_e.IDLE;
			}

			if (ControlsManager.GetControl("RotateCCW").GetDown())
				RotateClipboard(copyOffset, true);
			if (ControlsManager.GetControl("RotateCW").GetDown())
				RotateClipboard(copyOffset, false);
			if (ControlsManager.GetControl("Rotate180").GetDown())
			{
				RotateClipboard(copyOffset, false);
				RotateClipboard(copyOffset, false);
			}
		}
		else
		{
			foreach (Cell cell in clipboardCells)
				cell.gameObject.SetActive(false);
		}
	}
}
