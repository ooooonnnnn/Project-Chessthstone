using System;

namespace MonoGameProject1;

/// <summary>
/// base class for all behaviors. should be added to gameobjects via composition
/// </summary>
/// <param name="gameObject"> Reference to the containing gameobject</param>
/// <param name="enabled"> Will the behavior be updated on Update</param>
public abstract class Behavior
{
	public GameObject gameObject;

	/// <summary>
	/// When set with false, the behavior will no longer be updated. It starts as active by default
	/// </summary>
	public void SetActive(bool active)
	{
		gameObject.ActivateBehavior(active, this);
	}
	
	/// <summary>
	/// for validation and stuff. if you're inheriting from a behavior that isn't Behavior, call Initialize on the base first
	/// </summary>
	public abstract void Initialize(); 
}