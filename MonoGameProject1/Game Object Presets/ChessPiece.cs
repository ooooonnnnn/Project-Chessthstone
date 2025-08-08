using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

/// <summary>
/// Base class for chess pieces
/// </summary>
public abstract class ChessPiece : Sprite
{
	public PieceType type { get; init; }
	public bool isWhite { get; init; }
	public ChessBoard board { get; init; }
	protected int row, column;

	public ChessPiece(ChessBoard board, bool isWhite, PieceType type) : 
		base(ChessPieceName(isWhite, type), TextureManager.GetChessPieceTexture(isWhite, type))
	{
		this.type = type;
		this.isWhite = isWhite;
		this.board = board;
	}

	/// <summary>
	/// Moves the piece to the target square
	/// </summary>
	public void GoToSquare(ChessSquare square)
	{
		row = square.row;
		column = square.column;
		transform.parentSpacePos = square.transform.worldSpacePos;
	}
	
	/// <summary>
	/// List of int coordinates of the squares this piece can move to. <br/>
	/// Doesn't trim moves outside of the board. This is handled by the board
	/// </summary>
	public abstract List<Point> GetPossibleMoves();	

	private static string ChessPieceName(bool isWhite, PieceType type)
	{
		string color = isWhite ? "White" : "Black";
		return $"{color} {type}";
	}
}