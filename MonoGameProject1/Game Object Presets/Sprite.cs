using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Behaviors;
using IDrawable = MonoGameProject1.Content.IDrawable;
using IUpdateable = MonoGameProject1.Content.IUpdateable;

namespace MonoGameProject1;

/// <summary>
/// GameObject with SpriteRenderer behavior. 
/// </summary>
public class Sprite : GameObject
{
	public Transform transform;
	public SpriteRenderer spriteRenderer;

	public Sprite(string name, Texture2D texture) : base(name,
		new List<Behavior>
		{
			new Transform(),
			new SpriteRenderer(texture)
		})
	{
		GetBehaviorReferences();
	}

	public Sprite(string name, Texture2D texture, Rectangle sourceRectangle = default) : base(name,
		new List<Behavior>
		{
			new Transform(),
			new SpriteRenderer(texture, sourceRectangle)
		})
	{
		GetBehaviorReferences();
	}

	private void GetBehaviorReferences()
	{
		transform = TryGetBehavior<Transform>();
		spriteRenderer = TryGetBehavior<SpriteRenderer>();
	}
}