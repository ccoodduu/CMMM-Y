public enum Mode_e { 
    LEVEL, EDITOR, VAULT_LEVEL
}

public static class Extensions
{
	public static bool IsEditor(this Mode_e mode)
	{
		return mode is Mode_e.LEVEL || mode is Mode_e.VAULT_LEVEL;
	}
}
