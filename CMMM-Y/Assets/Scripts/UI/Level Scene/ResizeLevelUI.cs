using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResizeLevelUI : MonoBehaviour
{
	public TMP_InputField upInput;
	public TMP_InputField downInput;
	public TMP_InputField leftInput;
	public TMP_InputField rightInput;

	public TMP_Text newSizeText;

	private void Update()
	{
		var newSize = GetNewSize();

		var text = "";

		if (newSize.x <= 0 || newSize.y <= 0) text = "<s>Invalid Size</s>";

		if (text == "") text = "<u>New size</u>\n" + newSize.x + "x" + newSize.y;

		newSizeText.text = text;
	}

	public void Resize()
	{
		var newSize = GetNewSize();

		if (newSize.x <= 0 || newSize.y <= 0) return;

		var (up, down, left, right) = GetInputs();

		if (up == 0 && down == 0 && left == 0 && right == 0) return;

		var topLeft = new Vector2Int(-left, CellFunctions.gridHeight + up - 1);
		var bottomRight = new Vector2Int(CellFunctions.gridWidth + right - 1, -down);

		ActionManager.instance.DoAction((Action)new ResizeLevel(topLeft, bottomRight));
	}

	private Vector2Int GetNewSize()
	{
		var (up, down, left, right) = GetInputs();

		var newSize = new Vector2Int(left + CellFunctions.gridWidth + right, up + CellFunctions.gridHeight + down);

		return newSize;
	}

	private (int, int, int, int) GetInputs()
	{
		int up = upInput.text == "-" ? 0 : int.Parse(upInput.text == "" ? "0" : upInput.text);
		int down = downInput.text == "-" ? 0 : int.Parse(downInput.text == "" ? "0" : downInput.text);
		int left = leftInput.text == "-" ? 0 : int.Parse(leftInput.text == "" ? "0" : leftInput.text);
		int right = rightInput.text == "-" ? 0 : int.Parse(rightInput.text == "" ? "0" : rightInput.text);

		return (up, down, left, right);
	}
}
