using System.Collections.Generic;
using System.Net.Mime;
using MonoGameProject1.Behaviors;
using MonoGameProject1.Settings;

namespace MonoGameProject1;

/// <summary>
/// Shows text
/// </summary>
public class TextBox : GameObject
{
	public readonly TextRenderer textRenderer;
	public readonly Transform transform;
	public string text
	{
		get => textRenderer.Text;
		set => textRenderer.Text = value;
	}
	public int maxWidth
	{
		get => textRenderer.MaxWidth;
		set => textRenderer.MaxWidth = value;
	}
	
	/// <summary>
	/// Creates a TextBox with no text. Left aligned, no wrapping
	/// </summary>
	public TextBox(string name) : base(name, [new Transform(), new TextRenderer()])
	{
		textRenderer = TryGetBehavior<TextRenderer>();
		transform = TryGetBehavior<Transform>();
	}
	
	/// <summary>
	/// TextBox with initial text, optional max width (setting this causes wrapping), and optional centering
	/// </summary>
	public TextBox(string name, string text, int maxWidth = 0, bool isCentered = false) 
		: base(name, [new Transform(), new TextRenderer(text, maxWidth, isCentered)])
	{
		textRenderer = TryGetBehavior<TextRenderer>();
		transform = TryGetBehavior<Transform>();
		textRenderer.color = GraphicsSettings.textColor;
	}
}