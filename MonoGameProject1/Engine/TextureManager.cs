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
	private static Texture2D[] _chessSquareTextures;
	private static Texture2D[,] _chessPieceTextures;

	public static void LoadTextures()
	{
		_defaultButtonTexture = game.Content.Load<Texture2D>("Images/RoundedFilledSquare");
		_logoTexture = game.Content.Load<Texture2D>("Images/Logo");
		_chessSquareTextures = new Texture2D[2];
		_chessSquareTextures[0] = game.Content.Load<Texture2D>("Images/tile1BlackWithCorners");
		_chessSquareTextures[1] = game.Content.Load<Texture2D>("Images/tile1WhiteWithCorners");
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
		return _chessSquareTextures[isWhite ? 1 : 0];
	}

	public static Texture2D GetChessPieceTexture(bool isWhite, PieceType type)
	{
		//TODO: Temporary
		return _defaultButtonTexture;
		
		return _chessPieceTextures[isWhite ? 1 : 0, (int) type];
	}
}