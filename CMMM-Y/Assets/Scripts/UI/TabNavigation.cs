using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
using System;

public class TabNavigation : MonoBehaviour
{
	private EventSystem system;

	private TabNavigationComponent[] allTabNavigators;
	private int currentIndex;
	private TabNavigationComponent currentComponent;

	private int cycleIndex;
	public Selectable[] cycle;
	public bool doCycle;

	public GameObject UIContainer;

	void Start()
	{
		system = EventSystem.current;
		allTabNavigators = UIContainer.GetComponentsInChildren<TabNavigationComponent>(false);
		currentComponent = allTabNavigators[0];
		currentIndex = currentComponent.GetFirstIndex();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (system.currentSelectedGameObject == null)
			{
				SetSelected();
				return;
			}

			var lastSelected = currentComponent.GetAtIndex(currentIndex).gameObject;
			if (system.currentSelectedGameObject != lastSelected)
			{
				var selectable = system.currentSelectedGameObject.GetComponent<Selectable>();

				bool found = false;

				// Find in list
				if (doCycle)
				{
					if (cycle.Any(s => s == selectable))
					{
						cycleIndex = Array.IndexOf(cycle, selectable);
						found = true;
					}
				}
				else
				{
					foreach (var tabNavigator in allTabNavigators)
					{
						var indexedSelecable = tabNavigator.selectables.FirstOrDefault(s => s.selectable == selectable);
						if (indexedSelecable != null)
						{
							currentComponent = tabNavigator;
							currentIndex = indexedSelecable.index;
							found = true;
							break;
						}
					}
				}

				if (!found)
				{
					SetSelected();
					return;
				}
			}

			if (doCycle)
			{
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) 
					cycleIndex = (cycleIndex - 1 + cycle.Length) % cycle.Length;
				else 
					cycleIndex = (cycleIndex + 1) % cycle.Length;
			}
			else
			{
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				{
					var newIndex = currentComponent.GetPreviousIndex(currentIndex);
					if (newIndex == currentIndex)
					{
						var newTabNavigatorIndex = Array.IndexOf(allTabNavigators, currentComponent) - 1;
						if (newTabNavigatorIndex >= 0)
						{
							currentComponent = allTabNavigators[newTabNavigatorIndex];
							newIndex = currentComponent.GetLastIndex();
						}
					}
					currentIndex = newIndex;
				}
				else
				{
					var newIndex = currentComponent.GetNextIndex(currentIndex);
					if (newIndex == currentIndex)
					{
						var newTabNavigatorIndex = Array.IndexOf(allTabNavigators, currentComponent) + 1;
						if (newTabNavigatorIndex < allTabNavigators.Length)
						{
							currentComponent = allTabNavigators[newTabNavigatorIndex];
							newIndex = currentComponent.GetFirstIndex();
						}
					}
					currentIndex = newIndex;
				}
			}
			SetSelected();
		}
	}

	private void SetSelected()
	{
		GameObject selected;
		if (doCycle)
		{
			selected = cycle[cycleIndex].gameObject;
		}
		else
		{
			selected = currentComponent.GetAtIndex(currentIndex).gameObject;
		}

		InputField inputfield = selected.GetComponent<InputField>();
		if (inputfield != null)
			inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

		system.SetSelectedGameObject(selected, new BaseEventData(system));
	}
}