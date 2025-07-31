namespace MonoGameProject1.Engine;

/// <summary>
/// For behaviors that are part of a hierarchy. Changing the value of a behavior will affect the values of its children 
/// </summary>
public interface IHierarchy
{
	/// <summary>
	/// Adds the relevant behavior of the GameObject as a child. Possibly changes its value
	/// </summary>
	public void AddChild(GameObject gameObject);

	/// <summary>
	/// Removes the relevant behavior of the GameObject from the hierarchy.
	/// </summary>
	public void RemoveChild(GameObject gameObject);
}