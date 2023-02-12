using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementManager : MonoBehaviour
{
    Direction_e oldDir;
    Direction_e dir;

    public static PlacementManager i;

    float animationTime = 0;
    readonly float animationDuration = .1f;

    bool backgroundTileToggle = false;
    bool backgroundTileDebounce = false;

    public Transform[] Buttons;

    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        if (GridManager.mode != Mode_e.EDITOR)
        {
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        animationTime += Time.deltaTime;

        #region Animation and rotating
        if (Input.GetKeyDown(KeyCode.Q) && GridManager.tool != Tool_e.SELECT) {
            animationTime = 0;
            if ((int)dir == 0)
            {
                dir = (Direction_e)3;
            }
            else {
                dir = (Direction_e)((int)dir - 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && GridManager.tool != Tool_e.SELECT) {
            animationTime = 0;
            if ((int)dir == 3)
            {
                dir = (Direction_e)0;
            }
            else
            {
                dir = (Direction_e)((int)dir + 1);
            }
        }
        

        foreach (Transform transform in Buttons) {
            if(transform.GetComponent<EditorButtons>().Animate)
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, (int)oldDir * -90), Quaternion.Euler(0, 0, (int)dir * -90), animationTime / animationDuration);
        }

        if (animationTime > animationDuration) {
            animationTime = 0;
            oldDir = dir;
        }
        #endregion

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        int x = Mathf.FloorToInt(worldPoint.x + .5f);
        int y = Mathf.FloorToInt(worldPoint.y + .5f);

        if (Input.GetMouseButton(0)) {

            if (!GridManager.clean)
                return;

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (x < 0 || y < 0)
                return;

            if (x >= CellFunctions.gridWidth || y >= CellFunctions.gridHeight)
                return;

            if (GridManager.tool == Tool_e.DRAG || GridManager.tool == Tool_e.SELECT)
                return;

            if (GridManager.tool == Tool_e.PLACEMENT)
            {
                if (!backgroundTileDebounce)
                    backgroundTileToggle = GridManager.instance.tilemap.GetTile(new Vector3Int(x,y,0)) == GridManager.instance.placebleTile;
                backgroundTileDebounce = true;
                GridManager.instance.tilemap.SetTile(new Vector3Int(x,y,0),
                    backgroundTileToggle ? GridManager.instance.backgroundTile : GridManager.instance.placebleTile
                    );
                return;
            }

            if (CellFunctions.cellGrid[x, y] != null)
            {
                if (CellFunctions.cellGrid[x, y].cellType != (CellType_e)GridManager.tool || CellFunctions.cellGrid[x, y].getDirection() != (Direction_e)dir) {
                    CellFunctions.cellGrid[x, y].Delete(true);
                }
                else return;
            }

            AudioManager.i.PlaySound(GameAssets.i.place);
            Cell cell = GridManager.instance.SpawnCell((CellType_e)GridManager.tool, new Vector2(x,y), dir, false);
            GridManager.hasSaved = false;
        }

        if (Input.GetMouseButton(1) && GridManager.tool != Tool_e.SELECT)
        {
            if (!GridManager.clean)
                return;
            if (x < 0 || y < 0)
                return;
            if (x >= CellFunctions.gridWidth || y >= CellFunctions.gridHeight)
                return;

            if (CellFunctions.cellGrid[x, y] != null)
            {
                AudioManager.i.PlaySound(GameAssets.i.destroy);
                CellFunctions.cellGrid[x, y].Delete(true);
                GridManager.hasSaved = false;
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            backgroundTileDebounce = false;
        }
    }
}
