using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeleteSelection : Action
{
	private SavedCell[] cells;

	public DeleteSelection(SavedCell[] cells)
	{
		this.cells = cells;
	}

	public override void Execute()
	{
		foreach (var cell in cells)
		{
			CellFunctions.cellGrid[cell.position.x, cell.position.y].Delete(true);
		}
	}

	public override void Undo()
	{
		foreach (var cell in cells)
		{
			GridManager.instance.SpawnCell(cell.cellType, cell.position, (Direction_e)cell.rotation, false);
		}
	}
}
