using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

public class Bishop(ChessBoard board, bool isWhite) : ChessPiece(board, isWhite, PieceType.Bishop)
{
	public override List<Point> GetPossibleMoves()
	{
		List<Point> moves = new();
		for (int i = 1; i < ChessProperties.boardSize; i++)
		{
			moves.Add(new Point(column + i, row + i));
			moves.Add(new Point(column - i, row + i));
			moves.Add(new Point(column + i, row - i));
			moves.Add(new Point(column - i, row - i));
		}
		return moves;
	}
}