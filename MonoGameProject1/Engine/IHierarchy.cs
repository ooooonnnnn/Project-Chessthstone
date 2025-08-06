using System.Collections.Generic;

namespace MonoGameProject1;

/// <summary>
/// For behaviors that are part of a hierarchy. Changing the value of a behavior will affect the values of its children 
/// </summary>
public interface IHierarchy<T> : IHierarchy where T : Behavior
{
	/// <summary>
	/// Adds the behavior as a child
	/// </summary>
	/// <param name="behavior"></param>
	public void AddChild(T behavior);
	
	/// <summary>
	/// Removes the behavior from children
	/// </summary>
	/// <param name="behavior"></param>
	public void RemoveChild(T behavior);
}

/// <summary>
/// Base interface for generic IHierarchy
/// </summary>
public interface IHierarchy
{
	public IReadOnlyList<GameObject> children { get; }
	
	/// <summary>
	/// Adds the relevant behavior of the GameObject as a child. Possibly changes its value
	/// </summary>
	public void AddChild(GameObject gameObject);
	
	/// <summary>
	/// Removes the relevant behavior of the GameObject from the hierarchy.
	/// </summary>
	public void RemoveChild(GameObject gameObject);
}