using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabNavigation : MonoBehaviour
{
	private EventSystem system;

	void Start()
	{
		system = EventSystem.current;

	}
	// TODO: Fix in LevelSelect and TextureSelect
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			Selectable next = null;

			if (system.currentSelectedGameObject == null)
			{
				system.SetSelectedGameObject(system.firstSelectedGameObject, new BaseEventData(system));
				return;
			}

			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				if (next == null) next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnLeft();
				if (next == null) next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
			} 
			else
			{
				if (next == null) next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight();
				if (next == null) next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
			}

			if (next != null)
			{
				InputField inputfield = next.GetComponent<InputField>();
				if (inputfield != null)
					inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

				system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
			}
		}
	}
}