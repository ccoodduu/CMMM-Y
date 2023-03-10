public enum Mode_e
{
	LEVEL, EDITOR, VAULT_LEVEL
}

public static class Extensions
{
	public static bool IsEditor(this Mode_e mode)
	{
		return mode is Mode_e.EDITOR || mode is Mode_e.VAULT_LEVEL;
	}

	public static Tool_e ToTool(this CellType_e cell)
	{
		switch (cell)
		{
			case CellType_e.GENERATOR: return Tool_e.GENERATOR;
			case CellType_e.CWROTATER: return Tool_e.CWROTATER;
			case CellType_e.CCWROTATER: return Tool_e.CCWROTATER;
			case CellType_e.MOVER: return Tool_e.MOVER;
			case CellType_e.SLIDE: return Tool_e.SLIDE;
			case CellType_e.BLOCK: return Tool_e.BLOCK;
			case CellType_e.WALL: return Tool_e.WALL;
			case CellType_e.ENEMY: return Tool_e.ENEMY;
			case CellType_e.TRASH: return Tool_e.TRASH;
			default: return Tool_e.GENERATOR;
		}
	}
}
