using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePlaceable : Action
{
	private readonly Vector2Int position;
	private readonly bool setPlaceable;
	private bool previousPlaceable;

	public PlacePlaceable(Vector2Int position, bool setPlaceable)
	{
		this.position = position;
		this.setPlaceable = setPlaceable;
	}

	public override void Execute()
	{
		var pos = new Vector3Int(position.x, position.y, 0);

		previousPlaceable = GridManager.instance.tilemap.GetTile(pos) == GridManager.instance.placebleTile;

		GridManager.instance.tilemap.SetTile(pos, setPlaceable ? GridManager.instance.placebleTile : GridManager.instance.backgroundTile);
	}

	public override void Undo()
	{
		var pos = new Vector3Int(position.x, position.y, 0);

		GridManager.instance.tilemap.SetTile(pos, previousPlaceable ? GridManager.instance.placebleTile : GridManager.instance.backgroundTile);
	}
}
