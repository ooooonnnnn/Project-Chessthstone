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
	public Texture2D Texture => _texture;
	public Rectangle SourceRectangle => _sourceRectangle;

	// texture
	protected internal Texture2D _texture;
	protected Rectangle _sourceRectangle;
	
	protected Transform _transform;

	public SpriteRenderer(Texture2D texture)
	{
		_texture = texture;
		_sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
	}

	public SpriteRenderer(Texture2D texture, Rectangle sourceRectangle)
	{
		_texture = texture;
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
	public virtual void DrawAt(SpriteBatch spriteBatch, Vector2 position)
	{
		spriteBatch.Draw(_texture, position, _sourceRectangle, color, 0f,
			Vector2.Zero, Vector2.One, effects, layerDepth);
	}
}
