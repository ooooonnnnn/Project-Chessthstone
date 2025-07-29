using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = MonoGameProject1.Content.IDrawable;
using IUpdateable = MonoGameProject1.Content.IUpdateable;

namespace MonoGameProject1;

/// <summary>
/// GameObject with SpriteRenderer behavior. 
/// </summary>
public class Sprite : GameObject
{
	public Transform transform;

	public Sprite(string name, Texture2D texture) : base(name,
		new List<Behavior>
		{
			new Transform(),
			new SpriteRenderer(texture)
		})
	{
		GetTransform();
	}

	public Sprite(string name, Texture2D texture, Rectangle sourceRectangle) : base(name,
		new List<Behavior>
		{
			new Transform(),
			new SpriteRenderer(texture, sourceRectangle)
		})
	{
		GetTransform();
	}

	private void GetTransform()
	{
		transform = TryGetBehavior<Transform>();
	}
}