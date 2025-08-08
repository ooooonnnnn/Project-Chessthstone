using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

public class Knight(ChessBoard board, bool isWhite) : ChessPiece(board, isWhite, PieceType.Knight)
{
	public override List<Point> GetPossibleMoves()
	{
		List<Point> moves = new();
		for (int i = -2; i <= 2; i++)
		{
			for (int j = -2; j <= 2; j++)
			{
				if ((Math.Abs(i) == 2 && Math.Abs(j) == 1) || (Math.Abs(i) == 1 && Math.Abs(j) == 2))
				{
					Point nextCoord = new Point(column + i, row + j);
					bool alwaysTrue = true;
					if (ValidatePossibleMove(nextCoord, ref alwaysTrue)) moves.Add(nextCoord);
				}
			}
		}
		return moves;
	}
}