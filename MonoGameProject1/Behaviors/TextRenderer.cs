using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Engine;
using IDrawable = MonoGameProject1.Content.IDrawable;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// Displays text. Requires Transform
/// </summary>
public class TextRenderer : Renderer
{
	private string text;
	public string Text{
		get => text;
		set
		{
			text = value;
			FixText();
		}
	}
	public SpriteFont Font = FontManager.defaultFont;
	public bool rightToLeft = false;
	public int maxPixelWidth = 0;
	
	public TextRenderer(string text = "", int maxPixelWidth = 0, SpriteFont font = null, bool rightToLeft = false)
	{
		this.Text = text;
		this.maxPixelWidth = maxPixelWidth;
		this.Font = font ?? FontManager.defaultFont;
		this.rightToLeft = rightToLeft;
		
		FixText();
	}
	
	private void FixText()
	{
		if (maxPixelWidth <= 0) return;
		var lastBreakIndex = 0;
		var lastSpaceIndex = 0;
		for (int i = 0; i < Text.Length; i++)
		{
			switch (Text[i])
			{
				case ' ':
					lastSpaceIndex = i;
					break;
				case '\n':
					lastBreakIndex = i;
					break;
			}
			if (Font.MeasureString(Text.Substring(lastBreakIndex, i - lastBreakIndex + 1)).X > maxPixelWidth)
			{
				Text = Text.Insert(lastSpaceIndex, "\n");
				lastBreakIndex = i;
				lastSpaceIndex = i;
			}
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.DrawString(Font, Text, _transform.worldSpacePos, color, _transform.rotation,
			_transform.origin, _transform.worldSpaceScale, effects, layerDepth, rightToLeft);
	}
}