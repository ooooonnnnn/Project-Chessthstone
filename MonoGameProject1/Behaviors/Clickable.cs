using System;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// calls a callback when clicked. requires the SenseMouseHover behavior
/// </summary>>
public class Clickable : Behavior, IDisposable
{
	public event Action OnClick;
	
	private SenseMouseHover senseMouseHover;

	public override void Initialize()
	{
		senseMouseHover = gameObject.TryGetBehavior<SenseMouseHover>();
		MouseInput.OnLeftClick += AttemptClick;
	}

	private void AttemptClick()
	{
		if (senseMouseHover.isHovering) OnClick?.Invoke();
	}

	public void Dispose()
	{
		OnClick = null;
		MouseInput.OnLeftClick -= AttemptClick;
	}
}