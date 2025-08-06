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

	public static void LoadTextures()
	{
		_defaultButtonTexture = game.Content.Load<Texture2D>("Images/RoundedFilledSquare");
		_logoTexture = game.Content.Load<Texture2D>("Images/Logo");
	}

	public static Texture2D GetDefaultButtonTexture()
	{
		return _defaultButtonTexture;
	}

	public static Texture2D GetLogoTexture()
	{
		return _logoTexture;
	}

	public static Texture2D GetChessSquareTexture(bool isWhite)
	{
		
	}

	public static Texture2D GetChessPieceTexture(bool isWhite, PieceType type)
	{
		
	}
}