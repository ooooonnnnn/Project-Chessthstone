using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// Shows a texture2d. Requires a Transform. Can't have two of these in one GameObject
/// </summary>
public class SpriteRenderer : Renderer
{
	/// <summary>
	/// The height of the source rectangle
	/// </summary>
	public int height => _sourceRectangle.Height;
	public int width => _sourceRectangle.Width;
	
	// texture
	protected Texture2D _texture;
	protected Rectangle _sourceRectangle;
	
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
	
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(_texture, _transform.worldSpacePos, _sourceRectangle, color, _transform.rotation,
			_transform.origin, _transform.scale, effects, layerDepth);
	}
}