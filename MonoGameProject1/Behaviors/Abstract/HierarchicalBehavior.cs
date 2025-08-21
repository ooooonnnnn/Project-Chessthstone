using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameProject1.Behaviors.Abstract;

public abstract class HierarchicalBehavior : Behavior
{
	public abstract IReadOnlyList<GameObject> children { get; }
}

public abstract class HierarchicalBehavior<T> : HierarchicalBehavior where T : HierarchicalBehavior<T>
{
	public override IReadOnlyList<GameObject> children => _children.Select(child => child.gameObject).ToList();
	protected List<T> _children = new();
	protected T _parent;

	public T parent
	{
		get => _parent;
		set
		{
			if (_parent == value) 
				return;
			if (_parent == this)
				throw new ArgumentException($"Can't set {gameObject.name} parent to itself");
			_parent?._children.Remove((T)this);
			_parent = value;
			_parent?._children.Add((T)this);
			RefreshAfterParentChange();
		}
	}

	/// <summary>
	/// Implement this to update values when the parent changes
	/// </summary>
	protected virtual void RefreshAfterParentChange(){}

	public void AddChild(GameObject child)
	{
		AddChild(child.TryGetBehavior<T>());
	}

	public void AddChild(T behavior)
	{
		if (behavior == this)
			throw new ArgumentException($"Can't set {gameObject.name} child to itself");
		_children.Add(behavior);
		behavior._parent = (T)this;
	}

	public void RemoveChild(GameObject child)
	{
		RemoveChild(child.TryGetBehavior<T>());
	}

	public void RemoveChild(T behavior)
	{
		_children.Remove(behavior);
		behavior.parent = null;
	}
}