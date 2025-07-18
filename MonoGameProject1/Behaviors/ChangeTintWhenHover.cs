using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using IUpdateable = MonoGameProject1.Content.IUpdateable;

namespace MonoGameProject1;

/// <summary>
/// Requires Sprite and SenseMouseHover. <br/>
/// </summary>
public class ChangeTintWhenHover : Behavior, IUpdateable
{
	public Color tintWhenHover;
	public Color tintWhenMouseDown;
	
	private Color _originalTint;
	private SpriteRenderer _spriteRenderer;
	private SenseMouseHover _senseMouseHover;

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
		_spriteRenderer.color = !_senseMouseHover.isHovering ? _originalTint:
				Mouse.GetState().LeftButton == ButtonState.Pressed ? tintWhenMouseDown : tintWhenHover;
	}
}