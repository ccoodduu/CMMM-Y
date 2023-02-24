using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
	private float orthographicSize;
	private float x;
	private float y;

	private void Start()
	{
		instance = this;
	}

	public void PositionCamera()
	{
		this.x = (float)CellFunctions.gridWidth * 0.5f - 0.5f;
		this.y = (float)CellFunctions.gridHeight * 0.5f - 0.5f;
		this.orthographicSize = (float)CellFunctions.gridHeight * 0.5f + 2f;
		base.transform.position = new Vector3(this.x, this.y, -10f);
		base.GetComponent<Camera>().orthographicSize = this.orthographicSize;
	}

	public void Update()
	{
		Zoom(Input.mouseScrollDelta.y * Time.deltaTime);

		if (Input.GetKey("left ctrl"))
		{
			this.x += Input.GetAxis("Horizontal") * 0.5f * PlayerPrefs.GetFloat("MovementSpeed", 1f);
			this.y += Input.GetAxis("Vertical") * 0.5f * PlayerPrefs.GetFloat("MovementSpeed", 1f);
		}
		else
		{
			this.x += Input.GetAxis("Horizontal") * 0.2f * PlayerPrefs.GetFloat("MovementSpeed", 1f);
			this.y += Input.GetAxis("Vertical") * 0.2f * PlayerPrefs.GetFloat("MovementSpeed", 1f);
		}
		base.transform.position = new Vector3(this.x, this.y, -10f);
		base.GetComponent<Camera>().orthographicSize = this.orthographicSize;
	}

	private void Zoom(float amount)
	{
		var camera = this.GetComponent<Camera>();

		this.orthographicSize = camera.orthographicSize;

		var camSize = new Vector2(this.orthographicSize * 2 * camera.aspect, this.orthographicSize * 2);

		var addSize = camSize*amount;

		this.orthographicSize += addSize.y;
		if (this.orthographicSize < 0.5)
			this.orthographicSize = 0.5f;
		else
		{
			if (amount != 0)
			{

			}
		}
	}

	public static CameraPan instance;
}
