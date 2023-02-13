using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
		throw new System.NotImplementedException();
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

		output.Append(";" + level.Name + ";");
		return output.ToString();
	}
}

public class ReadableFormat : SaveFormat
{
	public override string FormatName => "Readable";

	public override Level Decode(string code)
	{
		throw new System.NotImplementedException();
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

		output.Append(";" + level.Name + ";");
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

	public override Level Decode(string code)
	{
		throw new System.NotImplementedException();
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

			maxMatchLength = 0;
			dataIndex += 1;
		}
		output.Append(";" + level.Name + ";");
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

	public override Level Decode(string code)
	{
		throw new System.NotImplementedException();
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
		output.Append(";" + level.Name + ";");
		return output.ToString();
	}
}

public class V1Format : SaveFormat
{
	public override string FormatName => "V1";

	public override Level Decode(string code)
	{
		throw new System.NotImplementedException();
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
				if (level.Placeable[x+y*level.Size.x])
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
		output.Append(";" + level.Name + ";");
		return output.ToString();
	}
}