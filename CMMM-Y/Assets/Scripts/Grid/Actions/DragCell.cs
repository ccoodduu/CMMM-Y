using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DragCell : Action
{
	private Vector2Int from;
	private Vector2Int to;

	public DragCell(Vector2Int from, Vector2Int to)
	{
		this.from = from;
		this.to = to;
	}

	public override void Execute()
	{
		var selectedCell = CellFunctions.cellGrid[from.x, from.y];
		var cellAtPosition = CellFunctions.cellGrid[to.x, to.y];

		if (cellAtPosition == null)
		{
			selectedCell.SetPosition(to);
			selectedCell.spawnPosition = selectedCell.position;
		}
		else
		{
			cellAtPosition.SetPosition(from);
			selectedCell.position = to; // Set position already so SetPosition() doesn't overwrite the other cell
			selectedCell.SetPosition(to);

			cellAtPosition.spawnPosition = cellAtPosition.position;
			selectedCell.spawnPosition = selectedCell.position;
		}
	}

	public override void Undo()
	{
		var selectedCell = CellFunctions.cellGrid[to.x, to.y];
		var cellAtPosition = CellFunctions.cellGrid[from.x, from.y];

		if (cellAtPosition == null)
		{
			selectedCell.SetPosition(from);
			selectedCell.spawnPosition = selectedCell.position;
		}
		else
		{
			cellAtPosition.SetPosition(to);
			selectedCell.position = from; // Set position already so SetPosition() doesn't overwrite the other cell
			selectedCell.SetPosition(from);

			cellAtPosition.spawnPosition = cellAtPosition.position;
			selectedCell.spawnPosition = selectedCell.position;
		}
	}
}
