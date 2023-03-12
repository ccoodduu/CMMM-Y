using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class DragManager : MonoBehaviour
{
    Vector3 mousePos;
    Cell selectedCell;
    bool inDrag = false;

    public void EndDrag() {
        if (mousePos.x < 0 || mousePos.y < 0 || mousePos.x >= CellFunctions.gridWidth || mousePos.y >= CellFunctions.gridHeight) 
        {
            CancelDrag();
            return;
        }

		var isPlaceable = GridManager.instance.tilemap.GetTile(new Vector3Int((int)mousePos.x, (int)mousePos.y, 0)) == GridManager.instance.placebleTile;

		if (!isPlaceable && GridManager.mode == Mode_e.LEVEL ||
			isPlaceable && GridManager.mode == Mode_e.VAULT_LEVEL) {
            CancelDrag();
            return;
        }

        AudioManager.instance.PlaySound(GameAssets.instance.place);
        if (CellFunctions.cellGrid[(int)mousePos.x, (int)mousePos.y] == null)
        {
            selectedCell.SetPosition((int)mousePos.x, (int)mousePos.y);
            inDrag = false;
            selectedCell.animate = true;
            selectedCell.spawnPosition = selectedCell.position;
        }
        else {
            Cell cell = CellFunctions.cellGrid[(int)mousePos.x, (int)mousePos.y];
            CellFunctions.cellGrid[(int)mousePos.x, (int)mousePos.y].SetPosition(selectedCell.position);
            selectedCell.position = new Vector2((int)mousePos.x, (int)mousePos.y);
            selectedCell.SetPosition((int)mousePos.x, (int)mousePos.y);
            selectedCell.animate = true;
            inDrag = false;
            cell.spawnPosition = cell.position;
            selectedCell.spawnPosition = selectedCell.position;
        }
        GridManager.hasSaved = false;
    }

    public void CancelDrag() {
        inDrag = false;
        if (selectedCell != null)
            selectedCell.animate = true;
    }

    public void StartDrag() {
        if (mousePos.x < 0 || mousePos.y < 0)
            return;
        if (mousePos.x >= CellFunctions.gridWidth || mousePos.y >= CellFunctions.gridHeight)
            return;

        var isPlaceable = GridManager.instance.tilemap.GetTile(new Vector3Int((int)mousePos.x, (int)mousePos.y, 0)) == GridManager.instance.placebleTile;

		if (!isPlaceable && GridManager.mode == Mode_e.LEVEL ||
			isPlaceable && GridManager.mode == Mode_e.VAULT_LEVEL)
        {
            CancelDrag();
            return;
        }

        if (CellFunctions.cellGrid[(int)mousePos.x, (int)mousePos.y] != null)
        {
            selectedCell = CellFunctions.cellGrid[(int)mousePos.x, (int)mousePos.y];
            selectedCell.animate = false;
            inDrag = true;
        }
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)) + new Vector3(.5f,.5f,0);
		Control control = ControlsManager.GetControl("DragCell");

		if (inDrag) {
            selectedCell.transform.position = new Vector3(mousePos.x - .5f, mousePos.y - .5f, -5);
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (control.GetUp() && inDrag) CancelDrag();
			return;
		}
        if ((GridManager.tool != Tool_e.DRAG && GridManager.mode.IsEditor()))
        {
			if (inDrag) CancelDrag();
            return;
		}
        if (!GridManager.clean)
        {
			if (inDrag) CancelDrag();
			return;
        }

        if (control.GetDown()) {
            StartDrag();
        }
        if (control.GetUp() && inDrag && selectedCell != null)
        {
            EndDrag();
        }
    }
}
