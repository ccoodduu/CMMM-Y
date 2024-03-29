﻿using System.Collections;
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

	bool placePlaceable = false;
	bool backgroundTileDebounce = false;

	public Transform[] buttons;

	private readonly List<Vector2Int> justPlacedAt = new List<Vector2Int>();

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

		var position = new Vector2Int(x, y);

		if (ControlsManager.GetControl("PlaceCell").Get())
		{
			if (justPlacedAt.Contains(position))
				return;

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
					placePlaceable = !isPlaceable;
				backgroundTileDebounce = true;

				if (placePlaceable != isPlaceable) ActionManager.instance.DoAction(new PlacePlaceable(position, placePlaceable));

				return;
			}

			justPlacedAt.Add(position);

			AudioManager.instance.PlaySound(GameAssets.instance.place);

			ActionManager.instance.DoAction(new PlaceCell(position, (CellType_e)GridManager.tool, dir));
		}
		else if (ControlsManager.GetControl("DeleteCell").Get())
		{
			if (justPlacedAt.Contains(position))
				return;

			if (GridManager.tool == Tool_e.SELECT)
				return;

			if (!GridManager.clean)
				return;

			if (x < 0 || y < 0)
				return;

			if (x >= CellFunctions.gridWidth || y >= CellFunctions.gridHeight)
				return;

			var isPlaceable = GridManager.instance.tilemap.GetTile(new Vector3Int(x, y, 0)) == GridManager.instance.placebleTile;

			if (GridManager.mode == Mode_e.VAULT_LEVEL && isPlaceable)
				return;

			if (CellFunctions.cellGrid[x, y] == null)
				return;

			justPlacedAt.Add(position);

			AudioManager.instance.PlaySound(GameAssets.instance.destroy);

			ActionManager.instance.DoAction(new DeleteCell(position));
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

		if (ControlsManager.GetControl("PlaceCell").GetUp() || ControlsManager.GetControl("DeleteCell").GetUp())
		{
			justPlacedAt.Clear();
		}
	}
}
