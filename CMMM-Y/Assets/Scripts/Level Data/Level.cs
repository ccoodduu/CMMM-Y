using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class Level
{
	public string Name { get; private set; }
	public Vector2Int Size { get; private set; }
	public SavedCell[] Cells { get; private set; }
	public bool[] Placeable { get; private set; }
	public string TutorialText { get; private set; }

	public Level(string name, Vector2Int size, SavedCell[] cells, bool[] placeable, string tutorialText)
	{
		Name = name;
		Size = size;
		Cells = cells;
		Placeable = placeable;
		TutorialText = tutorialText;
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

		var tutorialText = GameObject.Find("TutorialText").GetComponent<TextMeshProUGUI>().text;

		return new Level("Unnamed", new Vector2Int(width, height), cells.ToArray(), placable, tutorialText);
	}

	public void LoadToGrid()
	{
		CellFunctions.gridWidth = Size.x;
		CellFunctions.gridHeight = Size.y;
		GridManager.instance.InitGridSize();

		for (int i = 0; i < Placeable.Length; i++)
		{
			var place = Placeable[i];
			if (!place) continue;
			var x = i % Size.x;
			var y = i / Size.x;

			GridManager.instance.tilemap.SetTile(new Vector3Int(x, y, 0), GridManager.instance.placebleTile);
		}

		for (int i = 0; i < Cells.Length; i++)
		{
			var cell = Cells[i];

			GridManager.instance.SpawnCell(cell.cellType, cell.position, (Direction_e)cell.rotation, false);
		}

		GameObject.Find("TutorialText").GetComponent<TextMeshProUGUI>().text = TutorialText;
	}
}

public struct SavedCell
{
	public CellType_e cellType;
	public Vector2Int position;
	public int rotation;
}

