using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameProject1.Engine;

/// <summary>
/// Holds all SpriteFonts
/// </summary>
public static class FontManager
{
	public static Game game;
	public static SpriteFont defaultFont { get; private set; }
	public static SpriteFont arialFont { get; private set; }
	
	public static void LoadFonts()
	{
		//defaultFont = game.Content.Load<SpriteFont>("Fonts/Retro Gaming");
		defaultFont = game.Content.Load<SpriteFont>("Fonts/Arial");
	}
}