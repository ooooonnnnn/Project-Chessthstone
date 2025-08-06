using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameProject1;

/// <summary>
/// Holds all textures for the game
/// </summary>
public static class TextureManager
{
	public static Game game;
	private static Texture2D _defaultButtonTexture;
	private static Texture2D _logoTexture;
	public static Texture2D TestSpriteSheetTexture{ get; private set; }

	public static void LoadTextures()
	{
		_defaultButtonTexture = game.Content.Load<Texture2D>("Images/RoundedFilledSquare");
		_logoTexture = game.Content.Load<Texture2D>("Images/Logo");
		TestSpriteSheetTexture = game.Content.Load<Texture2D>("Images/SpriteSheets/TestSpriteSheet");
	}

	public static Texture2D GetDefaultButtonTexture()
	{
		return _defaultButtonTexture;
	}

	public static Texture2D GetLogoTexture()
	{
		return _logoTexture;
	}
}