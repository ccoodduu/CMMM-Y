using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class Save : MonoBehaviour
{
    public TMP_Text saveText;

    public void Awake()
    {
		saveText.gameObject.SetActive(false);
    }

	void Update()
	{
		if (ControlsManager.GetControl("Save").GetDown()) SaveLevel();
	}

	public void SaveLevel() {
		saveText.text = "Level copied to clipboard!";
		SaveLevel(Level.FromCurrent());
    }

	public void SaveLevel(Vector2Int topLeft, Vector2Int bottomRight)
	{
		saveText.text = "Selection copied to clipboard!";
		SaveLevel(Level.FromCurrent().Crop(topLeft, bottomRight));
	}

	private void SaveLevel(Level level)
    {
        var save = FormatManager.SelectedFormat.Encode(level);    
        
        GridManager.hasSaved = true;
        GUIUtility.systemCopyBuffer = save;

		StartCoroutine(ShowThenFadeOut());
	}

	public IEnumerator ShowThenFadeOut()
	{
		saveText.gameObject.SetActive(true);

		float counter = 0f;
		while (counter < 3f)
		{
			counter += Time.deltaTime;
			if (counter > 2f) saveText.alpha = Mathf.Lerp(1f, 0f, (counter - 2f) / 1f);
			yield return null;
		}

		saveText.gameObject.SetActive(false);
		saveText.alpha = 1f;
	}
}
