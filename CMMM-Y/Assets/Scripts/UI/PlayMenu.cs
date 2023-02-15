using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
    public GameObject errorCard;

    string ConvertString(string oldFormat)
    {
        string[] components = oldFormat.Split(';');

        List<string> cells = new List<string>();
        foreach (string enemyCell in components[3].Split(','))
        {
            if (enemyCell != "")
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

    public void LoadLevel()
    {
        GridManager.currentLevel = 999;

        var str = GUIUtility.systemCopyBuffer;
		var formatName = str.Split(';')[0];
		var format = FormatManager.formats.FirstOrDefault(f => f.FormatName.ToLower() == formatName.ToLower());

		if (format != null)
        {
            GridManager.loadString = str;
        }
        else
        {
            try
            {
                GridManager.loadString = ConvertString(str);
            }
            catch
            {
                errorCard.GetComponent<CanvasGroup>().alpha = 1;
                errorCard.GetComponentInChildren<Text>().text = "Your clipboard doesn't contain a valid level!";

                CanvasGroup canvGroup = errorCard.GetComponent<CanvasGroup>();

                StartCoroutine(PauseThenFadeOut(canvGroup, canvGroup.alpha, 0));
                return;
            }
        }
        GridManager.mode = Mode_e.EDITOR;
        SceneManager.LoadScene("LevelScreen");
    }

    public IEnumerator PauseThenFadeOut(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;
        while (counter < 3f)
        {
            counter += Time.deltaTime;
            if (counter > 2f) canvGroup.alpha = Mathf.Lerp(start, end, (counter - 2f) / 1f);
            yield return null;
        }
    }
}
