using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

public class King(ChessBoard board, bool isWhite) : ChessPiece(board, isWhite, PieceType.King)
{
	public override List<Point> GetPossibleMoves()
	{
		List<Point> moves = new();
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (i == 0 && j == 0) continue;
				moves.Add(new Point(column + i, row + j));
			}
		}
		return moves;
	}
}