using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateLevel : MonoBehaviour
{
    public TMP_InputField width;
    public TMP_InputField height;
    public void Create() {
        GridManager.currentLevel = 999;
        if (width.text != "" && height.text != "")
        {
            GridManager.mode = Mode_e.EDITOR;
            CellFunctions.gridWidth = int.Parse(width.text);
            CellFunctions.gridHeight = int.Parse(height.text);
            GridManager.loadString = "";
            SceneManager.LoadScene("LevelScreen");
        }
    }
}
