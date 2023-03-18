using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveSelection : Action
{
	private SavedCell[] cells;

	private SavedCell[] deletedCells;

	private Vector2Int offset;

	private bool stack;

	public MoveSelection(Vector2Int offset, SavedCell[] moveCells, bool stack)
	{
		this.offset = offset;
		this.cells = moveCells;
		this.stack = stack;
	}

	public override void Execute()
	{
		if (!stack)
			foreach (var cell in cells)
			{
				CellFunctions.cellGrid[cell.position.x, cell.position.y].Delete(true);
			}

		var deletedCellsList = new List<SavedCell>();

		foreach (var cell in cells)
		{
			var newPos = cell.position + offset;

			var currentCell = CellFunctions.cellGrid[newPos.x, newPos.y];
			if (currentCell != null)
			{
				deletedCellsList.Add(new SavedCell()
				{
					position = newPos,
					cellType = currentCell.cellType,
					rotation = currentCell.rotation,
				});
			}

			GridManager.instance.SpawnCell(cell.cellType, newPos, (Direction_e)cell.rotation, false);
		}

		deletedCells = deletedCellsList.ToArray();
	}

	public override void Undo()
	{
		foreach (var cell in cells)
		{
			CellFunctions.cellGrid[cell.position.x + offset.x, cell.position.y + offset.y].Delete(true);
		}

		foreach (var cell in cells)
		{
			GridManager.instance.SpawnCell(cell.cellType, cell.position, (Direction_e)cell.rotation, false);
		}

		foreach (var cell in deletedCells)
		{
			GridManager.instance.SpawnCell(cell.cellType, cell.position, (Direction_e)cell.rotation, false);
		}
	}
}
