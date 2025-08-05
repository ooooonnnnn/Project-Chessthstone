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
	public int height => sourceRectangle.Height;
	public int width => sourceRectangle.Width;
	
	// texture
	public Texture2D texture;
	public Rectangle sourceRectangle;
	
	public SpriteRenderer(Texture2D texture)
	{
		this.texture = texture;
		sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
	}

	public SpriteRenderer(Texture2D texture, Rectangle sourceRectangle)
	{
		this.texture = texture;
		this.sourceRectangle = sourceRectangle;
	}
	
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(texture, _transform.worldSpacePos, sourceRectangle, color, _transform.rotation,
			_transform.origin, _transform.scale, effects, layerDepth);
	}
}