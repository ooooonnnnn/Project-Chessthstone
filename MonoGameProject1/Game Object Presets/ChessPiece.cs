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
	/// Takes the board size and existing pieces into account
	/// </summary>
	public abstract List<Point> GetPossibleMoves();	
	
	/// <summary>
	/// Used for constructing the possible moves of a piece. Given a possible move and a true bool that signifies this
	/// move is part of a valid direction, this function will check if the direction is still valid, update the bool, and
	/// add the move accordingly.
	/// </summary>
	/// <param name="nextCoord">The move to consider</param>
	/// <param name="directionValid">If the move is part of a direction, true means the direction is not blocked and
	/// within the board. The function checks and updates its value</param>
	protected bool ValidatePossibleMove(Point nextCoord, ref bool directionValid)
	{
		if (directionValid)
		{
			if (!ChessProperties.IsPointInBoard(nextCoord))
			{
				directionValid = false;
			}
			else if (board.squares[nextCoord.X, nextCoord.Y].occupyingPiece != null)
			{
				directionValid = false;
			}
		}
		return directionValid;
	}

	private static string ChessPieceName(bool isWhite, PieceType type)
	{
		string color = isWhite ? "White" : "Black";
		return $"{color} {type}";
	}
}