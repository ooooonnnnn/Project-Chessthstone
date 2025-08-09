using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

/// <summary>
/// Base class for pawns. Check HLD to see how they move
/// </summary>
public class Pawn(ChessBoard board, bool isWhite, int baseHealth, int baseDamage) 
	: ChessPiece(board, isWhite, PieceType.Pawn, baseHealth, baseDamage)
{
	public override List<Point> GetMoveCoordList()
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
					if (ValidateMove(nextCoord, ref alwaysTrue)) moves.Add(nextCoord);
				}
			}
		}
		return moves;
	}

	public override List<Point> GetAttackCoordList()
	{
		List<Point> attacks = new();
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (Math.Abs(i) == 1 && Math.Abs(j) == 1)
				{
					Point nextCoord = new Point(column + i, row + j);
					bool alwaysTrue = true;
					if (ValidateAttackCoord(nextCoord, ref alwaysTrue)) attacks.Add(nextCoord);
				}
			}
		}
		return attacks;
	}
}