using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameProject1;

public class ClickableSprite : Sprite, ICallback
{
	private Clickable _clickable;

	public ClickableSprite(string name, Texture2D texture, Rectangle sourceRectangle = default) : base(name, texture, sourceRectangle)
	{
		AddClickBehaviors();
	}
	
	private void AddClickBehaviors()
	{
		AddBehaviors(new List<Behavior>
		{
			new Clickable(),
			new SenseMouseHover(),
			new SpriteRectCollider()
		});
		
		_clickable = TryGetBehavior<Clickable>();
	}
	
	public void AddLeftClickListener(Action listener)
	{
		_clickable.OnLeftClick += listener;
	}
	
	public void AddRightClickListener(Action listener)
	{
		_clickable.OnRightClick += listener;
	}

	public void RemoveLeftClickListener(Action listener)
	{
		_clickable.OnLeftClick -= listener;
	}
	
	public void RemoveRightClickListener(Action listener)
	{
		_clickable.OnRightClick -= listener;
	}
}