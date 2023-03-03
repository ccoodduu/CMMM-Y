using System.Text;
using UnityEngine;

public class Save : MonoBehaviour
{
    public GameObject saveText;

    public void Awake()
    {
        saveText.SetActive(false);
    }

    public void SaveLevel() {
		SaveLevel(Level.FromCurrent());
    }

	public void SaveSelection()
	{
		SaveLevel(Level.FromSelection());
	}
	public void SaveLevel(Vector2Int topLeft, Vector2Int bottomRight)
	{
		SaveLevel(Level.FromCurrent().Crop(topLeft, bottomRight));
	}

	private void SaveLevel(Level level)
    {
        var save = FormatManager.SelectedFormat.Encode(level);    
        
        GridManager.hasSaved = true;
        GUIUtility.systemCopyBuffer = save;
    }
}
