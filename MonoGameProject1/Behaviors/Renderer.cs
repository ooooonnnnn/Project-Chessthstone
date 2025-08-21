using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Behaviors.Abstract;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// Base class for renderer behaviors. Has parent and children to support groups of sprites sharing the same color etc.
/// </summary>
public abstract class Renderer : HierarchicalBehavior<Renderer>, IDrawable
{
	public Color color = Color.White;
	public SpriteEffects effects = SpriteEffects.None;
	public float layerDepth = LayerDepthManager.GameObjectDepth;
	public Transform transform;
	
	public override void Initialize()
	{
		transform = gameObject.TryGetBehavior<Transform>();
		//Can't have two renderers on one object
		gameObject.DontAllowBehaviorBesidesThis<Renderer>(this);
	}

	public abstract void Draw(SpriteBatch spriteBatch);
}