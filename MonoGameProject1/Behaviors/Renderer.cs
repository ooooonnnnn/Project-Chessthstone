using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Engine;
using IDrawable = MonoGameProject1.Content.IDrawable;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// Base class for renderer behaviors. Has parent and children to support groups of sprites sharing the same color etc.
/// </summary>
public abstract class Renderer : Behavior, IDrawable
{
	public Color color = Color.White;
	public SpriteEffects effects = SpriteEffects.None;
	public float layerDepth = 0;
	protected Transform _transform;
	
	public override void Initialize()
	{
		_transform = gameObject.TryGetBehavior<Transform>();
		//Can't have two renderers on one object TODO: Why?
		gameObject.DontAllowBehaviorBesidesThis<SpriteRenderer>(this);
	}

	public abstract void Draw(SpriteBatch spriteBatch);
}