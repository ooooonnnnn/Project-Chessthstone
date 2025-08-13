using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameProject1;

/// <summary>
/// reports when the mouse starts and stops hovering over this object's collider. <br/>
/// requires the collider behavior
/// </summary>
public class SenseMouseHover : Behavior, IUpdatable, IDisposable
{
	public event Action OnStartHover;
	public event Action OnEndHover;
	
	public bool isHovering => _collider.PointOnCollider(Mouse.GetState().Position.ToVector2()); //hovering query

	private bool _currentlyHovering; //hovering in this frame
	
	private Collider _collider;
	
	public override void Initialize()
	{
		//check gameobject has collider
		_collider = gameObject.TryGetBehavior<Collider>();
	}

	public void Update(GameTime gameTime)
	{
		//check mouse position and trigger events accordingly
		bool newHovering = _collider.PointOnCollider(Mouse.GetState().Position.ToVector2());
		if (newHovering && !_currentlyHovering)
		{
			OnStartHover?.Invoke();
		}
		else if (!newHovering && _currentlyHovering)
		{
			OnEndHover?.Invoke();
		}
		
		_currentlyHovering = newHovering;
	}

	public void Dispose()
	{
		OnStartHover = null;
		OnEndHover = null;
	}
}