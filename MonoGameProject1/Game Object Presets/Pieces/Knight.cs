using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

public class Knight(ChessBoard board, Player owner, int baseHealth, int baseDamage)
	: ChessPiece(board, owner, PieceType.Knight, baseHealth, baseDamage)
{
	public override List<Point> GetMoveCoordList()
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
					if (ValidateMove(nextCoord, ref alwaysTrue)) moves.Add(nextCoord);
				}
			}
		}
		return moves;
	}

	public override List<Point> GetAttackCoordList()
	{
		List<Point> attacks = new();
		for (int i = -2; i <= 2; i++)
		{
			for (int j = -2; j <= 2; j++)
			{
				if ((Math.Abs(i) == 2 && Math.Abs(j) == 1) || (Math.Abs(i) == 1 && Math.Abs(j) == 2))
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