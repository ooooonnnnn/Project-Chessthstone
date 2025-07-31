using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class ClickableSprite : Sprite, ICallback
{
	private Clickable _clickable;
	
	public ClickableSprite(string name, Texture2D texture) : base(name, texture)
	{
		AddClickBehaviors();
	}

	public ClickableSprite(string name, Texture2D texture, Rectangle sourceRectangle) : base(name, texture, sourceRectangle)
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
	
	public void AddListener(Action listener)
	{
		_clickable.OnClick += listener;
	}

	public void RemoveListener(Action listener)
	{
		_clickable.OnClick -= listener;
	}
}