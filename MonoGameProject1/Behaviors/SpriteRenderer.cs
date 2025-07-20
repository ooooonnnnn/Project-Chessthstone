using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = MonoGameProject1.Content.IDrawable;

namespace MonoGameProject1;

/// <summary>
/// Shows a texture2d. Requires a Transform
/// </summary>
public class SpriteRenderer : Behavior, IDrawable
{
	public Color color = Color.White;
	public SpriteEffects effects = SpriteEffects.None;
	public float layerDepth = 0;
	
	/// <summary>
	/// The height of the source rectangle
	/// </summary>
	public int height => _sourceRectangle.Height;
	public int width => _sourceRectangle.Width;
	
	// texture
	protected Texture2D _texture;
	protected Rectangle _sourceRectangle;
	
	protected Transform _transform;

	public SpriteRenderer(Texture2D texture, Rectangle sourceRectangle = default)
	{
		_texture = texture;
		if (sourceRectangle == default)
			sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
		_sourceRectangle = sourceRectangle;
	}
	
	public override void Initialize()
	{
		_transform = gameObject.TryGetBehavior<Transform>();
	}

	public virtual void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(_texture, _transform.position, _sourceRectangle, color, _transform.rotation,
			_transform.origin, _transform.scale, effects, layerDepth);
	}
}