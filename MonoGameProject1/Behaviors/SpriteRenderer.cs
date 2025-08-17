using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// Shows a texture2d. Requires a Transform. Can't have two of these in one GameObject
/// </summary>
public class SpriteRenderer : Renderer
{
    /// <summary>
    /// Texture
    /// </summary>
    public Texture2D texture;
    public Rectangle sourceRectangle;

    /// <summary>
    /// Size in pixels. Changes the transform scale accordingly
    /// </summary>
    public Point sizePx
    {
        get => new((int)(sourceWidth * _transform.worldSpaceScale.X), (int)(sourceHeight * _transform.worldSpaceScale.Y));
        set
        {
            Vector2 worldScaleOfParent = _transform.parent?.worldSpaceScale ?? Vector2.One;
            _transform.parentSpaceScale = new(
                value.X / (float)sourceWidth / worldScaleOfParent.X,
                value.Y / (float)sourceHeight / worldScaleOfParent.Y);
        }
    }

    /// <summary>
    /// Size of the source rectangle
    /// </summary>
    public int sourceHeight => sourceRectangle.Height;
    public int sourceWidth => sourceRectangle.Width;

    public SpriteRenderer(Texture2D texture, Rectangle sourceRectangle = default)
    {
        this.texture = texture;
        this.sourceRectangle = sourceRectangle == default ? new Rectangle(0, 0, texture.Width, texture.Height) : sourceRectangle;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, _transform.worldSpacePos, sourceRectangle, color, _transform.rotation,
            _transform.origin, _transform.worldSpaceScale, effects, layerDepth);
    }
}