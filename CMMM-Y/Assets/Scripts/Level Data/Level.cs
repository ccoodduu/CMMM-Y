using UnityEngine;

public class Level
{
	public string Name { get; private set; }
	public string Description { get; private set; }
	public Vector2 Size { get; private set; }
	public Cell[] Cells { get; private set; }
}
