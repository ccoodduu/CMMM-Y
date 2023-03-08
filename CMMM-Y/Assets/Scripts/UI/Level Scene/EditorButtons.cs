using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorButtons : MonoBehaviour
{
    public Tool_e tool;
    public bool animate;
    public string controlName;

	private void Start()
	{
		if (tool == Tool_e.PLACEMENT && GridManager.mode == Mode_e.VAULT_LEVEL)
        {
            this.gameObject.SetActive(false);
        }
	}
	private void Update()
    {
        if (ControlsManager.GetControl(controlName).GetDown())
            SwitchTool();
    }

    public void SwitchTool() {
        GridManager.tool = tool;
        foreach (Transform button in PlacementManager.instance.buttons) {
            button.GetComponent<Image>().color = new Color(1, 1, 1, 1 - PlayerPrefs.GetFloat("FadeStrength", 0.5f));
        }
        this.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }
}
