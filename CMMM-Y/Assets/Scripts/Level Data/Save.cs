using System.Text;
using UnityEngine;

public class Save : MonoBehaviour
{
    public GameObject saveText;
    private const string cellKey = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!$%&+-.=?^{}";
    public GameObject canvas;

    public void Awake()
    {
        saveText.SetActive(false);
    }

    private string EncodeInt(int num)
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

    public void SaveString() {
        SaveString(new Vector2Int(0, CellFunctions.gridHeight - 1), new Vector2Int(CellFunctions.gridWidth - 1, 0));
    }

    public void SaveString(Vector2Int topLeft, Vector2Int bottomRight)
    {
        var level = Level.FromCurrent();
        var save = FormatManager.SelectedFormat.Encode(level);    
        
        GridManager.hasSaved = true;
        GUIUtility.systemCopyBuffer = save;
        GameObject go = Instantiate(saveText, canvas.GetComponent<Transform>(), true);
        go.SetActive(true);
        Destroy(go, 3);
    }
}
