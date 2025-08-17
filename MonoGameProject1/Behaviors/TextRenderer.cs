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
			WrapText();
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
			WrapText();
		}
	}

	private void WrapText()
	{
		if (string.IsNullOrEmpty(_text) || Font == null || MaxWidth <= 0) return;

		int lineStart = 0;
		int lastSpaceIndex = -1;

		for (int i = 0; i < _text.Length; i++)
		{
			if (_text[i] == ' ')
				lastSpaceIndex = i;

			// measure from lineStart up to i
			var width = Font.MeasureString(_text.Substring(lineStart, i - lineStart + 1)).X;

			if (width > MaxWidth && lastSpaceIndex > lineStart)
			{
				// replace the space with a newline
				_text = _text.Remove(lastSpaceIndex, 1).Insert(lastSpaceIndex, "\n");

				// reset indices
				lineStart = lastSpaceIndex + 1;
				i = lineStart;
				lastSpaceIndex = -1;
			}
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
		Text = text;
		MaxWidth = maxWidth;
		WrapText();
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.DrawString(Font, Text, _transform.worldSpacePos, color, _transform.rotation,
			_transform.origin, _transform.worldSpaceScale, effects, layerDepth, rightToLeft);
	}
}