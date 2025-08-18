using System;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// calls a callback when clicked. requires the SenseMouseHover behavior
/// </summary>>
public class Clickable : Behavior, IDisposable, IActivatable
{
	public event Action OnClick;
	public bool isActive => _isActive;
	private bool _isActive;
	
	private SenseMouseHover senseMouseHover;

	public override void Initialize()
	{
		senseMouseHover = gameObject.TryGetBehavior<SenseMouseHover>();
		SetActive(true);
	}

	private void AttemptClick()
	{
		if (senseMouseHover.isHovering)
		{
			OnClick?.Invoke();
			// Console.WriteLine($"{gameObject.name} clicked");
		}
	}

	public void Dispose()
	{
		OnClick = null;
		MouseInput.OnLeftClick -= AttemptClick;
	}

	public void SetActive(bool active)
	{
		if (active && !_isActive)
			MouseInput.OnLeftClick += AttemptClick;
		else if (!active && _isActive)
			MouseInput.OnLeftClick -= AttemptClick;
		
		_isActive = active;
	}

}