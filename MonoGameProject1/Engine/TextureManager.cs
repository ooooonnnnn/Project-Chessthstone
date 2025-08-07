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
	public static Texture2D KingBlackDeathSheet{ get; private set; }
	private static Texture2D[] _chessSquareTextures;
	private static Texture2D[,] _chessPieceTextures= new Texture2D[2,6];

	public static void LoadTextures()
	{
		_defaultButtonTexture = game.Content.Load<Texture2D>("Images/RoundedFilledSquare");
		_logoTexture = game.Content.Load<Texture2D>("Images/Logo");
		TestSpriteSheetTexture = game.Content.Load<Texture2D>("Images/SpriteSheets/TestSpriteSheet");
		KingBlackDeathSheet = game.Content.Load<Texture2D>("Images/SpriteSheets/kingDeathBlack2");
		_chessSquareTextures = new Texture2D[2];
		_chessSquareTextures[0] = game.Content.Load<Texture2D>("Images/tile1BlackWithCorners");
		_chessSquareTextures[1] = game.Content.Load<Texture2D>("Images/tile1WhiteWithCorners");
		
		_chessPieceTextures[0, (int)PieceType.King] = game.Content.Load<Texture2D>("Images/kingBlack");
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
		return _chessPieceTextures[isWhite ? 1 : 0, (int) type];
	}
}