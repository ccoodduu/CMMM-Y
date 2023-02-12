using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImportLevel : MonoBehaviour
{
    string convertString(string oldFormat)
    {
        string[] components = oldFormat.Split(';');

        List<string> cells = new List<string>();
        foreach (string enemyCell in components[3].Split(','))
        {
            if(enemyCell != "")
                cells.Add("7.0." + enemyCell);
        }


        string[] newCell = { "0.0", "0.2", "0.3", "0.1", "2.0", "1.0", "3.0", "3.2", "3.3", "3.1", "5.0", "4.0", "4.1", "6.0" };
        foreach (string oldCell in components[4].Split(','))
        {
            cells.Add(newCell[int.Parse(oldCell.Split('.')[0])] + oldCell.Substring(oldCell.IndexOf('.')));
        }

        string[] newComponents = { "V1", components[0], components[1], components[2], string.Join(",", cells), components[5] };
        GUIUtility.systemCopyBuffer = string.Join(";", newComponents);
        return (string.Join(";", newComponents) + ";");
    }

    public void Play() {
        GridManager.currentLevel = 999;
        if (GUIUtility.systemCopyBuffer.StartsWith("V"))
        {
            GridManager.loadString = GUIUtility.systemCopyBuffer;
        }
        else {
            GridManager.loadString = convertString(GUIUtility.systemCopyBuffer);
        }

        GridManager.mode = Mode_e.LEVEL;
        SceneManager.LoadScene("LevelScreen");
    }

    public void Edit() {
        GridManager.currentLevel = 999;
        if (GUIUtility.systemCopyBuffer.StartsWith("V"))
        {
            GridManager.loadString = GUIUtility.systemCopyBuffer;
        }
        else
        {
            GridManager.loadString = convertString(GUIUtility.systemCopyBuffer);
        }
        GridManager.mode = Mode_e.EDITOR;
        SceneManager.LoadScene("LevelScreen");
    }
}
