using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlacementManager : MonoBehaviour
{
	Direction_e oldDir;
	Direction_e dir;

	public static PlacementManager instance;

	float animationTime = 0;
	readonly float animationDuration = .1f;

	bool backgroundTileToggle = false;
	bool backgroundTileDebounce = false;

	public Transform[] buttons;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		if (!GridManager.mode.IsEditor())
		{
			this.gameObject.SetActive(false);
		}
	}

	void Update()
	{
		animationTime += Time.deltaTime;

		#region Animation and rotating
		if (ControlsManager.GetControl("RotateCCW").GetDown() && GridManager.tool != Tool_e.SELECT)
		{
			animationTime = 0;
			if ((int)dir == 0)
				dir = (Direction_e)3;
			else
				dir = (Direction_e)((int)dir - 1);
		}
		if (ControlsManager.GetControl("RotateCW").GetDown() && GridManager.tool != Tool_e.SELECT)
		{
			animationTime = 0;
			if ((int)dir == 3)
				dir = (Direction_e)0;
			else
				dir = (Direction_e)((int)dir + 1);
		}
		if (ControlsManager.GetControl("Rotate180").GetDown() && GridManager.tool != Tool_e.SELECT)
		{
			animationTime = 0;
			dir = (Direction_e)(((int)dir + 2) % 4);
		}

		foreach (Transform transform in buttons)
		{
			if (transform.GetComponent<EditorButtons>().animate)
				transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, (int)oldDir * -90), Quaternion.Euler(0, 0, (int)dir * -90), animationTime / animationDuration);
		}

		if (animationTime > animationDuration)
		{
			animationTime = 0;
			oldDir = dir;
		}
		#endregion

		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		int x = Mathf.FloorToInt(worldPoint.x + .5f);
		int y = Mathf.FloorToInt(worldPoint.y + .5f);

		if (ControlsManager.GetControl("PlaceCell").Get())
		{

			if (!GridManager.clean)
				return;

			if (EventSystem.current.IsPointerOverGameObject())
				return;

			if (x < 0 || y < 0)
				return;

			if (x >= CellFunctions.gridWidth || y >= CellFunctions.gridHeight)
				return;

			if (GridManager.tool == Tool_e.DRAG || GridManager.tool == Tool_e.SELECT)
				return;

			var isPlaceable = GridManager.instance.tilemap.GetTile(new Vector3Int(x, y, 0)) == GridManager.instance.placebleTile;


			if (GridManager.mode == Mode_e.VAULT_LEVEL && isPlaceable)
				return;

			if (GridManager.tool == Tool_e.PLACEMENT)
			{
				if (GridManager.mode == Mode_e.VAULT_LEVEL) return;

				if (!backgroundTileDebounce)
					backgroundTileToggle = isPlaceable;
				backgroundTileDebounce = true;
				GridManager.instance.tilemap.SetTile(new Vector3Int(x, y, 0),
					backgroundTileToggle ? GridManager.instance.backgroundTile : GridManager.instance.placebleTile
					);
				return;
			}

			if (CellFunctions.cellGrid[x, y] != null)
			{
				if (CellFunctions.cellGrid[x, y].cellType != (CellType_e)GridManager.tool || CellFunctions.cellGrid[x, y].GetDirection() != (Direction_e)dir)
				{
					CellFunctions.cellGrid[x, y].Delete(true);
				}
				else return;
			}

			AudioManager.instance.PlaySound(GameAssets.instance.place);
			Cell cell = GridManager.instance.SpawnCell((CellType_e)GridManager.tool, new Vector2(x, y), dir, false);
			GridManager.hasSaved = false;
		}

		if (ControlsManager.GetControl("DeleteCell").Get() && GridManager.tool != Tool_e.SELECT)
		{
			if (!GridManager.clean)
				return;

			if (x < 0 || y < 0)
				return;

			if (x >= CellFunctions.gridWidth || y >= CellFunctions.gridHeight)
				return;

			var isPlaceable = GridManager.instance.tilemap.GetTile(new Vector3Int(x, y, 0)) == GridManager.instance.placebleTile;

			if (GridManager.mode == Mode_e.VAULT_LEVEL && isPlaceable)
				return;

			if (CellFunctions.cellGrid[x, y] != null)
			{
				AudioManager.instance.PlaySound(GameAssets.instance.destroy);
				CellFunctions.cellGrid[x, y].Delete(true);
				GridManager.hasSaved = false;
			}
		}

		if (ControlsManager.GetControl("PickCell").GetDown())
		{
			if (!GridManager.clean)
				return;

			if (x < 0 || y < 0)
				return;

			if (x >= CellFunctions.gridWidth || y >= CellFunctions.gridHeight)
				return;

			var cell = CellFunctions.cellGrid[x, y];

			if (cell != null)
			{
				var tool = cell.cellType.ToTool();

				GridManager.tool = tool;

				foreach (Transform button in PlacementManager.instance.buttons)
				{
					var editorButton = button.GetComponent<EditorButtons>();
					if (editorButton.tool == tool)
					{
						editorButton.SwitchTool();
						dir = cell.GetDirection();
						break;
					}
				}
			}
		}

		if (ControlsManager.GetControl("PlaceCell").GetUp())
		{
			backgroundTileDebounce = false;
		}
	}
}
