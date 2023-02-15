using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public abstract class SaveFormat
{
	public abstract string FormatName { get; }
	public abstract string Encode(Level level);
	public abstract Level Decode(string code);
}

public class EmojiFormat : SaveFormat
{
	public override string FormatName => "Emoji";

	public override Level Decode(string code)
	{
		string[] arguments = code.Split(';');

		var cellString = arguments[1];
		var lines = cellString.Split(new[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
		var emojis = lines.Select(x => string.Join(":,:", x.Split(new[] { "::" }, StringSplitOptions.None)).Split(',')).ToArray();

		var size = new Vector2Int(emojis[0].Length, emojis.Length);
		var placeable = new bool[size.x * size.y];
		var cells = new List<SavedCell>();
		string tutorialText = arguments[2];
		string name = arguments[3];

		for (int y = 0; y < size.x; y++)
		{
			for (int x = 0; x < size.y; x++)
			{
				var position = new Vector2Int(x, y);
				var emoji = emojis[y][x];

				if (emoji == ":bg_placeable:")
				{
					placeable[x + y * size.y] = true;
				}
				else if (emoji != ":bg_default:")
				{
					SavedCell cell;
					switch (emoji)
					{
						case ":generator_right:": cell = new SavedCell() { cellType = CellType_e.GENERATOR, rotation = 0, position = position }; break;
						case ":generator_down:": cell = new SavedCell() { cellType = CellType_e.GENERATOR, rotation = 1, position = position }; break;
						case ":generator_left:": cell = new SavedCell() { cellType = CellType_e.GENERATOR, rotation = 2, position = position }; break;
						case ":generator_up:": cell = new SavedCell() { cellType = CellType_e.GENERATOR, rotation = 3, position = position }; break;
						case ":mover_right:": cell = new SavedCell() { cellType = CellType_e.MOVER, rotation = 0, position = position }; break;
						case ":mover_down:": cell = new SavedCell() { cellType = CellType_e.MOVER, rotation = 1, position = position }; break;
						case ":mover_left:": cell = new SavedCell() { cellType = CellType_e.MOVER, rotation = 2, position = position }; break;
						case ":mover_up:": cell = new SavedCell() { cellType = CellType_e.MOVER, rotation = 3, position = position }; break;
						case ":slide_horizontal:": cell = new SavedCell() { cellType = CellType_e.SLIDE, rotation = 0, position = position }; break;
						case ":slide_vertical:": cell = new SavedCell() { cellType = CellType_e.SLIDE, rotation = 1, position = position }; break;
						case ":wall:": cell = new SavedCell() { cellType = CellType_e.WALL, rotation = 0, position = position }; break;
						case ":push:": cell = new SavedCell() { cellType = CellType_e.BLOCK, rotation = 0, position = position }; break;
						case ":enemy:": cell = new SavedCell() { cellType = CellType_e.ENEMY, rotation = 0, position = position }; break;
						case ":trash:": cell = new SavedCell() { cellType = CellType_e.TRASH, rotation = 0, position = position }; break;
						case ":rotator_cw:": cell = new SavedCell() { cellType = CellType_e.CWROTATER, rotation = 0, position = position }; break;
						case ":rotator_ccw:": cell = new SavedCell() { cellType = CellType_e.CCWROTATER, rotation = 0, position = position }; break;

						default: throw new ArgumentException("Not a Cell Machine emoji");
					}
					cells.Add(cell);
				}
			}
		}

		return new Level(name, size, cells.ToArray(), placeable, tutorialText);
	}

	public override string Encode(Level level)
	{
		StringBuilder output = new StringBuilder();
		output.Append(FormatName + ";\n");

		var generatorEmojis = new string[] { ":generator_right:", ":generator_down:", ":generator_left:", ":generator_up:" };
		var moverEmojis = new string[] { ":mover_right:", ":mover_down:", ":mover_left:", ":mover_up:" };
		var slideEmojis = new string[] { ":slide_horizontal:", ":slide_vertical:", ":slide_horizontal:", ":slide_vertical:" };

		for (int y = level.Size.y - 1; y >= 0; y--)
		{
			for (int x = 0; x < level.Size.x; x++)
			{
				if (level.Cells.Any(c => c.position == new Vector2Int(x, y)))
				{
					var cell = level.Cells.FirstOrDefault(c => c.position == new Vector2Int(x, y));
					switch (cell.cellType)
					{
						case CellType_e.GENERATOR:
							output.Append(generatorEmojis[cell.rotation]);
							break;
						case CellType_e.CWROTATER:
							output.Append(":rotator_cw:");
							break;
						case CellType_e.CCWROTATER:
							output.Append(":rotator_ccw:");
							break;
						case CellType_e.MOVER:
							output.Append(moverEmojis[cell.rotation]);
							break;
						case CellType_e.SLIDE:
							output.Append(slideEmojis[cell.rotation]);
							break;
						case CellType_e.BLOCK:
							output.Append(":push:");
							break;
						case CellType_e.WALL:
							output.Append(":wall:");
							break;
						case CellType_e.ENEMY:
							output.Append(":enemy:");
							break;
						case CellType_e.TRASH:
							output.Append(":trash:");
							break;
					}
				}
				else
				{
					output.Append(level.Placeable[x + y * level.Size.x] ? ":bg_placeable:" : ":bg_default:");
				}
			}
			output.Append("\n");
		}

		output.Append(";" + level.TutorialText + ";" + level.Name);
		return output.ToString();
	}
}

public class ReadableFormat : SaveFormat
{
	public override string FormatName => "Readable";

	public override Level Decode(string code)
	{
		throw new NotImplementedException();
	}

	public override string Encode(Level level)
	{
		StringBuilder output = new StringBuilder();
		output.Append(FormatName + ";\n");


		for (int y = level.Size.y - 1; y >= 0; y--)
		{
			for (int x = 0; x < level.Size.x; x++)
			{
				if (level.Cells.Any(c => c.position == new Vector2Int(x, y)))
				{
					var cell = level.Cells.FirstOrDefault(c => c.position == new Vector2Int(x, y));
					switch (cell.cellType)
					{
						case CellType_e.GENERATOR:
							output.Append("1234"[cell.rotation]);
							break;
						case CellType_e.CWROTATER:
							output.Append("r");
							break;
						case CellType_e.CCWROTATER:
							output.Append("l");
							break;
						case CellType_e.MOVER:
							output.Append(">v<^"[cell.rotation]);
							break;
						case CellType_e.SLIDE:
							output.Append("-|-|"[cell.rotation]);
							break;
						case CellType_e.BLOCK:
							output.Append("+");
							break;
						case CellType_e.WALL:
							output.Append("#");
							break;
						case CellType_e.ENEMY:
							output.Append("e");
							break;
						case CellType_e.TRASH:
							output.Append("x");
							break;
					}
				}
				else
				{
					output.Append(" ");
				}
			}
			output.Append("\n");
		}

		output.Append(";" + level.TutorialText + ";" + level.Name);
		return output.ToString();
	}
}

public class V3Format : SaveFormat
{
	public override string FormatName => "V3";

	private static readonly string cellKey = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!$%&+-.=?^{}";
	private static string EncodeInt(int num)
	{
		if (num < 74)
			return cellKey[num % 74].ToString();

		string output = "";
		while (num > 0)
		{
			output = cellKey[num % 74] + output;
			num /= 74;
		}
		return output;
	}

	private static int DecodeString(string str)
	{
		int output = 0;
		foreach (char c in str)
		{
			output *= 74;
			output += cellKey.IndexOf(c);
		}
		return output;
	}

	public override Level Decode(string code)
	{
		string[] arguments = code.Split(';');

		var size = new Vector2Int(DecodeString(arguments[1]), DecodeString(arguments[2]));
		var cells = new List<SavedCell>();
		var placeable = new bool[size.x * size.y];
		string tutorialText = arguments[4];
		string name = arguments[5];

		int[] cellDataHistory = new int[size.x * size.y];
		int offset;
		int length;
		int dataIndex = 0;
		int gridIndex = 0;
		string temp;

		string cellString = arguments[3];
		while (dataIndex < cellString.Length)
		{
			if (cellString[dataIndex] == ')' || cellString[dataIndex] == '(')
			{
				if (cellString[dataIndex] == ')')
				{
					dataIndex += 2;
					offset = cellKey.IndexOf(cellString[dataIndex - 1]);
					length = cellKey.IndexOf(cellString[dataIndex]);

				}
				else
				{
					dataIndex++;
					temp = "";
					while (cellString[dataIndex] != ')' && cellString[dataIndex] != '(')
					{
						temp += cellString[dataIndex];
						dataIndex++;
					}
					offset = DecodeString(temp);
					if (cellString[dataIndex] == ')')
					{
						dataIndex++;
						length = cellKey.IndexOf(cellString[dataIndex]);
					}
					else
					{
						dataIndex++;
						temp = "";
						while (cellString[dataIndex] != ')')
						{
							temp += cellString[dataIndex];
							dataIndex++;
						}
						length = DecodeString(temp);
					}
				}
				for (int i = 0; i < length; i++)
				{
					var c = cellDataHistory[gridIndex - offset - 1];
					placeable[gridIndex] = c % 2 == 1;
					if (c < 72)
					{
						cells.Add(new SavedCell()
						{
							cellType = (CellType_e)((c / 2) % 9),
							position = new Vector2Int(gridIndex % size.x, gridIndex / size.x),
							rotation = (c / 18),
						});
					}

					cellDataHistory[gridIndex] = c;
					gridIndex++;
				}
			}
			else
			{
				var c = cellKey.IndexOf(cellString[dataIndex]);
				placeable[gridIndex] = c % 2 == 1;
				if (c < 72)
				{
					cells.Add(new SavedCell()
					{
						cellType = (CellType_e)((c / 2) % 9),
						position = new Vector2Int(gridIndex % size.x, gridIndex / size.x),
						rotation = (c / 18),
					});
				}

				cellDataHistory[gridIndex] = c;
				gridIndex++;
			}

			dataIndex++;
		}

		return new Level(name, size, cells.ToArray(), placeable, tutorialText);
	}

	public override string Encode(Level level)
	{
		StringBuilder output = new StringBuilder();
		output.Append(FormatName + ";");
		output.Append(EncodeInt(level.Size.x) + ";" + EncodeInt(level.Size.y) + ";");
		int dataIndex = 0;
		int[] cellData = new int[level.Size.x * level.Size.y];

		for (int y = 0; y < level.Size.y; y++)
		{
			for (int x = 0; x < level.Size.x; x++)
			{
				cellData[x + y * level.Size.x] = level.Placeable[x + y * level.Size.x] ? 73 : 72;
			}
		}
		foreach (SavedCell cell in level.Cells)
		{
			cellData[(cell.position.x + cell.position.y * level.Size.x)] += (2 * (int)cell.cellType) + (18 * cell.rotation) - 72;
		}

		int matchLength;
		int maxMatchLength;
		int maxMatchOffset = 0;

		while (dataIndex < cellData.Length)
		{
			maxMatchLength = 0;
			for (int matchOffset = 1; matchOffset <= dataIndex; matchOffset++)
			{
				matchLength = 0;
				while (dataIndex + matchLength < cellData.Length && cellData[dataIndex + matchLength] == cellData[dataIndex + matchLength - matchOffset])
				{
					matchLength++;
					if (matchLength > maxMatchLength)
					{
						maxMatchLength = matchLength;
						maxMatchOffset = matchOffset - 1;
					}
				}
			}
			if (maxMatchLength > 3)
			{
				if (EncodeInt(maxMatchLength).Length == 1)
				{
					if (EncodeInt(maxMatchOffset).Length == 1)
					{
						if (maxMatchLength > 3)
						{
							output.Append(")" + EncodeInt(maxMatchOffset) + EncodeInt(maxMatchLength));
							dataIndex += maxMatchLength - 1;
						}
						else
							output.Append(cellKey[cellData[dataIndex]]);
					}
					else
					{
						if (maxMatchLength > 3 + EncodeInt(maxMatchOffset).Length)
						{
							output.Append("(" + EncodeInt(maxMatchOffset) + ")" + EncodeInt(maxMatchLength));
							dataIndex += maxMatchLength - 1;
						}
						else
							output.Append(cellKey[cellData[dataIndex]]);
					}
				}
				else
				{
					output.Append("(" + EncodeInt(maxMatchOffset) + "(" + EncodeInt(maxMatchLength) + ")");
					dataIndex += maxMatchLength - 1;
				}
			}
			else
				output.Append(cellKey[cellData[dataIndex]]);

			dataIndex += 1;
		}
		output.Append(";" + level.TutorialText + ";" + level.Name);
		return output.ToString();
	}
}

public class V2Format : SaveFormat
{
	public override string FormatName => "V2";

	private static readonly string cellKey = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!$%&+-.=?^{}";

	private static string EncodeInt(int num)
	{
		if (num < 74)
			return cellKey[num % 74].ToString();

		string output = "";
		while (num > 0)
		{
			output = cellKey[num % 74] + output;
			num /= 74;
		}
		return output;
	}
	private static int DecodeString(string str)
	{
		int output = 0;
		foreach (char c in str)
		{
			output *= 74;
			output += cellKey.IndexOf(c);
		}
		return output;
	}

	public override Level Decode(string code)
	{
		string[] arguments = code.Split(';');

		var size = new Vector2Int(DecodeString(arguments[1]), DecodeString(arguments[2]));
		var cells = new List<SavedCell>();
		var placeable = new bool[size.x * size.y];
		string tutorialText = arguments[4];
		string name = arguments[5];

		int length;
		int dataIndex = 0;
		int gridIndex = 0;
		string temp;

		string cellString = arguments[3];

		int cellData;
		while (dataIndex < arguments[3].Length)
		{
			if (arguments[3][dataIndex] == ')' || arguments[3][dataIndex] == '(')
			{

				cellData = cellKey.IndexOf(cellString[dataIndex - 1]);
				if (arguments[3][dataIndex] == ')')
				{
					dataIndex++;
					length = cellKey.IndexOf(cellString[dataIndex]);
				}
				else
				{
					dataIndex++;
					temp = "";
					while (arguments[3][dataIndex] != ')')
					{
						temp += arguments[3][dataIndex];
						dataIndex++;
					}
					length = DecodeString(temp);
				}

				if (cellData != 72)
				{
					for (int i = 0; i < length; i++)
					{
						placeable[gridIndex + i] = cellData % 2 == 1;
						if (cellData < 72)
						{
							cells.Add(new SavedCell()
							{
								cellType = (CellType_e)((cellData / 2) % 9),
								position = new Vector2Int((gridIndex + i) % size.x, (gridIndex + i) / size.x),
								rotation = (cellData / 18),
							});
						}
					}
				}
				gridIndex += length;
			}
			else
			{
				var c = cellKey.IndexOf(cellString[dataIndex]);
				placeable[gridIndex] = c % 2 == 1;
				if (c < 72)
				{
					cells.Add(new SavedCell()
					{
						cellType = (CellType_e)((c / 2) % 9),
						position = new Vector2Int(gridIndex % size.x, gridIndex / size.x),
						rotation = (c / 18),
					});
				}
				gridIndex++;
			}
			dataIndex++;

		}

		return new Level(name, size, cells.ToArray(), placeable, tutorialText);
	}

	public override string Encode(Level level)
	{
		var output = new StringBuilder();
		output.Append(FormatName + ";");
		output.Append(level.Size.x + ";" + level.Size.y + ";");

		int dataIndex = 0;
		int[] cellData = new int[level.Size.x * level.Size.y];

		for (int y = 0; y < level.Size.y; y++)
		{
			for (int x = 0; x < level.Size.x; x++)
			{
				cellData[x + y * level.Size.x] = level.Placeable[x + y * level.Size.x] ? 73 : 72;
			}
		}
		foreach (SavedCell cell in level.Cells)
		{
			cellData[(cell.position.x + cell.position.y * level.Size.x)] += (2 * (int)cell.cellType) + (18 * cell.rotation) - 72;
		}

		int runLength = 1;

		while (dataIndex < cellData.Length)
		{

			if (dataIndex + 1 < cellData.Length && cellData[dataIndex] == cellData[dataIndex + 1])
				runLength++;
			else
			{
				if (runLength > 3)
				{
					if (EncodeInt(runLength - 1).Length > 1)
						output.Append(cellKey[cellData[dataIndex]] + "(" + EncodeInt(runLength - 1) + ")");
					else
						output.Append(cellKey[cellData[dataIndex]] + ")" + EncodeInt(runLength - 1));
				}
				else
					output.Append(new string(cellKey[cellData[dataIndex]], runLength));
				runLength = 1;
			}
			dataIndex++;
		}
		output.Append(";" + level.TutorialText + ";" + level.Name);
		return output.ToString();
	}
}

public class V1Format : SaveFormat
{
	public override string FormatName => "V1";

	public override Level Decode(string code)
	{
		string[] arguments = code.Split(';');

		var size = new Vector2Int(int.Parse(arguments[1]), int.Parse(arguments[2]));
		var cells = new List<SavedCell>();
		var placeable = new bool[size.x * size.y];
		string tutorialText = arguments[5];
		string name = arguments[6];

		string[] placementCellLocationsStr = arguments[3].Split(',');
		if (placementCellLocationsStr[0] != "")
		{
			foreach (string str in placementCellLocationsStr)
			{
				int x = int.Parse(str.Split('.')[0]);
				int y = int.Parse(str.Split('.')[1]);
				placeable[x + y * size.x] = true;
			}
		}

		string[] cellStr = arguments[4].Split(',');
		if (cellStr[0] != "")
		{
			foreach (string str in cellStr)
			{
				string[] data = str.Split('.');
				cells.Add(new SavedCell()
				{
					cellType = (CellType_e)int.Parse(data[0]),
					position = new Vector2Int(int.Parse(data[2]), int.Parse(data[3])),
					rotation = int.Parse(data[1]),
				});
			}
		}
		return new Level(name, size, cells.ToArray(), placeable, tutorialText);
	}

	public override string Encode(Level level)
	{
		var output = new StringBuilder();
		output.Append(FormatName + ";");
		output.Append(level.Size.x + ";" + level.Size.y + ";");

		bool debounce = false;
		for (int y = 0; y < level.Size.y; y++)
		{
			for (int x = 0; x < level.Size.x; x++)
			{
				if (level.Placeable[x + y * level.Size.x])
				{
					if (debounce)
						output.Append(",");
					debounce = true;
					output.Append(x + "." + y);
				}
			}
		}
		output.Append(";");

		debounce = false;
		foreach (var cell in level.Cells)
		{
			if (debounce)
				output.Append(",");
			debounce = true;
			output.Append((int)cell.cellType + "." + cell.rotation + "." + cell.position.x + "." + cell.position.y);
		}
		output.Append(";" + level.TutorialText + ";" + level.Name);
		return output.ToString();
	}
}