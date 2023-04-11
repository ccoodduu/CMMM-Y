using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeleteCell : Action
{
	private Vector2Int position;
	private SavedCell deletedCell;

	public DeleteCell(Vector2Int position)
	{
		this.position = position;
	}

	public override void Execute()
	{
		var cell = CellFunctions.cellGrid[position.x, position.y];

		deletedCell = new SavedCell()
		{
			cellType = cell.cellType,
			position = position,
			rotation = cell.rotation,
		};

		cell.Delete(true);
	}

	public override void Undo()
	{
		GridManager.instance.SpawnCell(deletedCell.cellType, deletedCell.position, (Direction_e)deletedCell.rotation, false);
	}
}
