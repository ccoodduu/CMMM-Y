using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlaceCell : Action
{
	private readonly Vector2Int position;
	private readonly CellType_e cellType;
	private readonly Direction_e rotation;

	private SavedCell? previousCell = null;

	public PlaceCell(Vector2Int position, CellType_e cellType, Direction_e rotation) 
	{
		this.position = position;
		this.cellType = cellType;
		this.rotation = rotation;
	}

	public override void Execute()
	{
		var currentCell = CellFunctions.cellGrid[position.x, position.y];

		if (currentCell != null)
		{
			previousCell = new SavedCell()
			{
				cellType = currentCell.cellType,
				position = new Vector2Int((int)currentCell.position.x, (int)currentCell.position.y),
				rotation = currentCell.rotation,
			};
		}

		GridManager.instance.SpawnCell(cellType, new Vector2(position.x, position.y), rotation, false);
	}

	public override void Undo()
	{
		CellFunctions.cellGrid[position.x, position.y].Delete(true);

		if (previousCell != null) GridManager.instance.SpawnCell(previousCell.Value.cellType, previousCell.Value.position, (Direction_e)previousCell.Value.rotation, false);
	}
}
