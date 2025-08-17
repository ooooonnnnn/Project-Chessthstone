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
	private string _text;

	public string Text
	{
		get => _text;
		set
		{
			_text = value;
			if (MaxWidth > 0)
			{
				WrapText();
			}
		}
	}
	public SpriteFont Font = FontManager.defaultFont;
	private bool rightToLeft = false;
	
	private int _maxWidth = 0;
	public int MaxWidth
	{
		get => _maxWidth;
		set
		{
			_maxWidth = value;
			if (_maxWidth > 0)
			{
				WrapText();
			}
		}
	}

	private void WrapText()
	{
		if (string.IsNullOrEmpty(Text) || Font == null || MaxWidth <= 0) return;
		int lastSpaceIndex = 0;
		for (int i = 2; i < Text.Length; i++)
		{
			if (Text[i] == ' ') lastSpaceIndex = i;
			
			var width = Font.MeasureString(Text.Substring(lastSpaceIndex, i)).X;

			if (!(width > MaxWidth)) continue;
			if(lastSpaceIndex == 0) lastSpaceIndex = i - 1; // If no space found, break at current character
			Text = Text.Insert(lastSpaceIndex, "\n");
			i++;
		}
	}
	
	/// <summary>
	/// returns the size of the text in pixels, based on the current font and text.
	/// </summary>
	/// <returns></returns>
	public Vector2 GetTextSize()
	{
		if (string.IsNullOrEmpty(Text) || Font == null) return Vector2.Zero;
		return Font.MeasureString(Text);
	}

	/// <param name="text">Content.</param>
	/// <param name="maxWidth">If above 0, text will warp to its width in pixels.</param>
	public TextRenderer(string text = "", int maxWidth = 0)
	{
		this.Text = text;
		MaxWidth = maxWidth;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.DrawString(Font, Text, _transform.worldSpacePos, color, _transform.rotation,
			_transform.origin, _transform.worldSpaceScale, effects, layerDepth, rightToLeft);
	}
}