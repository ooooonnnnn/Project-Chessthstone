using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

/// <summary>
/// Base class for pawns. Check HLD to see how they move
/// </summary>
public class Pawn(ChessBoard board, bool isWhite) : ChessPiece(board, isWhite, PieceType.Pawn)
{
	public override List<Point> GetPossibleMoves()
	{
		List<Point> moves = new();
		moves.Add(new Point(column - 1, row));
		moves.Add(new Point(column + 1, row));
		moves.Add(new Point(column, row - 1));
		moves.Add(new Point(column, row + 1));
		return moves;
	}
}