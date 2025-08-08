using System;
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
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if ((Math.Abs(i) == 1 && j == 0) || Math.Abs(j) == 1 && i == 0)
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