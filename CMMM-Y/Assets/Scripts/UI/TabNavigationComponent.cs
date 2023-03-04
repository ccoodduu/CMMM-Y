using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TabNavigationComponent : MonoBehaviour
{
	public List<IndexedSelectable> selectables = new List<IndexedSelectable>();

	public void Clear()
	{
		selectables.Clear();
	}

	public void RemoveDestroyed()
	{
		selectables.RemoveAll(s => s.selectable == null);
	}

	public void Remove(Selectable selectable)
	{
		selectables.Remove(selectables.FirstOrDefault(s => s.selectable == selectable));
	}

	public void AddStart(Selectable selectable)
	{
		var index = selectables.Min(s => s.index) - 1;
		Add(selectable, index);
	}

	public void AddEnd(Selectable selectable)
	{
		var index = selectables.Max(s => s.index) + 1;
		Add(selectable, index);
	}

	public void Add(Selectable selectable, int index)
	{
		selectables.Add(new IndexedSelectable(index, selectable));
	}

	public Selectable GetAtIndex(int index)
	{
		var indexedSelectable = selectables.FirstOrDefault(s => s.index == index);
		return indexedSelectable == null ? GetAtIndex(GetFirstIndex()) : indexedSelectable.selectable;
	}

	public int GetFirstIndex()
	{
		return selectables.Where(s => s.selectable.IsInteractable()).Min(s => s.index);
	}
	public int GetLastIndex()
	{
		return selectables.Where(s => s.selectable.IsInteractable()).Max(s => s.index);
	}
	public int GetNextIndex(int index)
	{
		var allAfter = selectables.Where(s => s.index > index && s.selectable.IsInteractable());
		if (!allAfter.Any()) return index;
		return allAfter.Min(s => s.index);
	}
	public int GetPreviousIndex(int index)
	{
		var allBefore = selectables.Where(s => s.index < index && s.selectable.IsInteractable());
		if (!allBefore.Any()) return index;
		return allBefore.Max(s => s.index);
	}
}

[System.Serializable]
public class IndexedSelectable
{
	public int index;
	public Selectable selectable;

	public IndexedSelectable(int index, Selectable selectable)
	{
		this.index = index;
		this.selectable = selectable;
	}
}
