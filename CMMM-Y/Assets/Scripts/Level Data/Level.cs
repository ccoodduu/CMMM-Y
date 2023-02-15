using System;
using System.Linq;
using UnityEngine;

public class Level
{
	public string Name { get; private set; }
	public Vector2Int Size { get; private set; }
	public SavedCell[] Cells { get; private set; }
	public bool[] Placeable { get; private set; }


	public Level(string name, Vector2Int size, SavedCell[] cells, bool[] placeable)
	{
		Name = name;
		Size = size;
		Cells = cells;
		Placeable = placeable;
	}

	public static Level FromSelection()
	{
		throw new NotImplementedException();
	}

	public static Level FromCurrent()
	{
		var gridCells = CellFunctions.cellList;
		var cells = gridCells.Select(cell =>
		{
			var newCell = new SavedCell
			{
				position = Vector2Int.RoundToInt(cell.spawnPosition),
				rotation = cell.spawnRotation,
				cellType = cell.cellType
			};
			return newCell;
		});

		int width = CellFunctions.gridWidth;
		int height = CellFunctions.gridHeight;
			
		var placable = new bool[width * height];

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				placable[x + y * width] = GridManager.instance.tilemap.GetTile(new Vector3Int(x, y, 0)) == GridManager.instance.placebleTile;
			}
		}

		return new Level("", new Vector2Int(width, height), cells.ToArray(), placable);
	}

	public void LoadToGrid()
	{
		throw new NotImplementedException();
	}
}

public struct SavedCell
{
	public CellType_e cellType;
	public Vector2Int position;
	public int rotation;
}
