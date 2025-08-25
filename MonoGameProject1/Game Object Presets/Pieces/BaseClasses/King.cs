using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

public class King(bool isWhite, int baseHealth, int baseDamage, bool isSpecial = false) :
	ChessPiece(isWhite, PieceType.King, baseHealth, baseDamage, isSpecial)
{
	public override List<Point> GetMoveCoordList()
	{
		List<Point> moves = new();
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (i == 0 && j == 0) continue;
				bool alwaysTrue = true;
				Point nextCoord = new Point(column + i, row + j);
				if (ValidateMove(nextCoord, ref alwaysTrue)) moves.Add(nextCoord);
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
				if (i == 0 && j == 0) continue;
				bool alwaysTrue = true;
				Point nextCoord = new Point(column + i, row + j);
				if (ValidateAttackCoord(nextCoord, ref alwaysTrue)) attacks.Add(nextCoord);
			}
		}
		return attacks;
	}
}