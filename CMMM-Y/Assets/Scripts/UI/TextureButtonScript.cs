using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextureButtonScript : MonoBehaviour
{
	public TMP_Text pathText;
	public Image background;

	// Token: 0x060000C1 RID: 193 RVA: 0x00006714 File Offset: 0x00004914
	private bool isSelected;
	public bool IsSelected
	{
		get { return isSelected; }
		set
		{
			isSelected = value;
			background.gameObject.SetActive(isSelected);
			if (isSelected)
			{
				transform.SetAsFirstSibling();

				string texturePath = pathText.text.Split('/').First();
				PlayerPrefs.SetString("Texture", texturePath);
				TextureLoader.LoadTextureSet(texturePath);
			}
		}
	}

	public void OnClick()
	{
		transform.parent.GetChild(0).GetComponent<TextureButtonScript>().IsSelected = false;

		IsSelected = true;
	}
}
