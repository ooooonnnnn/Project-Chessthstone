using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Engine;
using IDrawable = MonoGameProject1.IDrawable;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// Displays text. Requires Transform
/// </summary>
public class TextRenderer : Renderer
{
	public string text;
	public SpriteFont font = FontManager.defaultFont;
	public bool rightToLeft = false;

	public TextRenderer()
	{
		
	}
	
	public TextRenderer(string text)
	{
		this.text = text;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.DrawString(font, text, _transform.worldSpacePos, color, _transform.rotation,
			_transform.origin, _transform.worldSpaceScale, effects, layerDepth, rightToLeft);
	}
}