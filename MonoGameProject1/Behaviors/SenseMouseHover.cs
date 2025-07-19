using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using IUpdateable = MonoGameProject1.Content.IUpdateable;

namespace MonoGameProject1;

/// <summary>
/// reports when the mouse starts and stops hovering over this object's collider. <br/>
/// requires the collider behavior
/// </summary>
public class SenseMouseHover : Behavior, IUpdateable
{
	public event Action OnStartHover;
	public event Action OnEndHover;
	public bool isHovering { get; private set; }
	
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
		if (newHovering && !isHovering)
		{
			OnStartHover?.Invoke();
		}
		else if (!newHovering && isHovering)
		{
			OnEndHover?.Invoke();
		}
		
		isHovering = newHovering;
	}
}