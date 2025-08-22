using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

/// <summary>
/// Requires SpriteRenderer and SenseMouseHover. <br/>
/// </summary>
public class ChangeTintWhenHover : Behavior, IUpdatable, IActivatable
{
	public Color tintWhenHover;
	public Color tintWhenMouseDown;
	
	private Color _originalTint;
	private SpriteRenderer _spriteRenderer;
	private SenseMouseHover _senseMouseHover;
	public bool isActive => _isActive;
	private bool _isActive = true;

	public ChangeTintWhenHover() : this(Color.LightGray, Color.Gray) { }
	
	public ChangeTintWhenHover(Color tintWhenHover, Color tintWhenMouseDown)
	{
		this.tintWhenHover = tintWhenHover;
		this.tintWhenMouseDown = tintWhenMouseDown;
	}
	
	public override void Initialize()
	{
		_spriteRenderer = gameObject.TryGetBehavior<SpriteRenderer>();
		_senseMouseHover = gameObject.TryGetBehavior<SenseMouseHover>();
		_originalTint = _spriteRenderer.color;
	}

	public void Update(GameTime gameTime)
	{
		if (!isActive)
			return;
		_spriteRenderer.color = !_senseMouseHover.isHovering ? _originalTint :
			Mouse.GetState().LeftButton == ButtonState.Pressed ? tintWhenMouseDown : tintWhenHover;
	}

	public void SetActive(bool active) => _isActive = active;

}