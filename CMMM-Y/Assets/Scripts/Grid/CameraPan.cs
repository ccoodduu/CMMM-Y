using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
	private float orthographicSize;
	private Vector2 position;

	private Vector2 previousMousePosition;

	private void Start()
	{
		instance = this;
	}

	public void Update()
	{
		Zoom(-Input.mouseScrollDelta.y * PlayerPrefs.GetFloat("ZoomSensitivity", 1f) * Time.deltaTime);
		Pan();

		ApplyPosition();

		previousMousePosition = Input.mousePosition;
	}

	public void PositionCamera()
	{
		position = new Vector2(CellFunctions.gridWidth * 0.5f - 0.5f, CellFunctions.gridHeight * 0.5f - 0.5f);
		orthographicSize = CellFunctions.gridHeight * 0.5f + 2f;

		ApplyPosition();
	}

	private void ApplyPosition()
	{
		transform.position = new Vector3(position.x, position.y, -10f);
		Camera.main.orthographicSize = orthographicSize;
	}

	private void Zoom(float amount)
	{
		orthographicSize = Camera.main.orthographicSize;

		var camSize = new Vector2(orthographicSize * 2 * Camera.main.aspect, orthographicSize * 2);

		var addSize = camSize * amount;

		orthographicSize += addSize.y;
		if (orthographicSize < 0.5)
			orthographicSize = 0.5f;
		else
		{
			if (amount != 0)
			{
				Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				var diffrence = position - mousePos;

				position += 2 * amount * diffrence;
			}
		}
	}

	private void Pan()
	{
		var speed = (Input.GetKey(ControlsManager.GetKeyForControl("FastPan")) ? 2f : 1f) * PlayerPrefs.GetFloat("MovementSpeed", 1f) * 2f * Time.deltaTime;

		if (Input.GetKey(ControlsManager.GetKeyForControl("Up"))) position += Vector2.up * speed;
		if (Input.GetKey(ControlsManager.GetKeyForControl("Down"))) position += Vector2.down * speed;
		if (Input.GetKey(ControlsManager.GetKeyForControl("Left"))) position += Vector2.left * speed;
		if (Input.GetKey(ControlsManager.GetKeyForControl("Right"))) position += Vector2.right * speed;

		if (Input.GetKey(ControlsManager.GetKeyForControl("Pan")))
		{
			Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 previousWorldPos = Camera.main.ScreenToWorldPoint(previousMousePosition);

			position += previousWorldPos - worldPos;
		}
	}

	public static CameraPan instance;
}
