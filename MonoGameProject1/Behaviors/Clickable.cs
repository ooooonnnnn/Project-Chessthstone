using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameProject1;

/// <summary>
/// calls a callback when clicked. requires the SenseMouseHover behavior
/// </summary>>
public class Clickable : Behavior
{
	public event Action OnLeftClick;
	public event Action OnRightClick;
	
	private SenseMouseHover senseMouseHover;

	public override void Initialize()
	{
		senseMouseHover = gameObject.TryGetBehavior<SenseMouseHover>();
		MouseInput.OnLeftClick += AttemptClick;
		MouseInput.OnRightClick += AttemptRightClick;
	}

	private void AttemptClick()
	{
		if (senseMouseHover.isHovering) OnLeftClick?.Invoke();
	}
	
	private void AttemptRightClick()
	{
		if (senseMouseHover.isHovering) OnRightClick?.Invoke();
	}
}