using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

public class Rook(ChessBoard board, bool isWhite) : ChessPiece(board, isWhite, PieceType.Rook)
{
	public override List<Point> GetMoveCoordList()
	{
		List<Point> moves = new();
		bool dir1 = true, dir2 = true, dir3 = true, dir4 = true; //Free directions
		for (int i = 1; i < ChessProperties.boardSize && (dir1 || dir2 || dir3 || dir4); i++)
		{
			Point nextCoord = new Point(column + i, row);
			if(ValidateMove(nextCoord,ref dir1))
				moves.Add(nextCoord);
			nextCoord = new Point(column - i, row);
			if(ValidateMove(nextCoord,ref dir2))
				moves.Add(nextCoord);
			nextCoord = new Point(column, row + i);
			if(ValidateMove(nextCoord,ref dir3))
				moves.Add(nextCoord);
			nextCoord = new Point(column, row - i);
			if(ValidateMove(nextCoord,ref dir4))
				moves.Add(nextCoord);
		}
		return moves;
	}

	public override List<Point> GetAttackCoordList()
	{
		List<Point> attacks = new();
		bool dir1 = true, dir2 = true, dir3 = true, dir4 = true; //Free directions
		for (int i = 1; i < ChessProperties.boardSize && (dir1 || dir2 || dir3 || dir4); i++)
		{
			Point nextCoord = new Point(column + i, row);
			if(ValidateAttack(nextCoord,ref dir1))
				attacks.Add(nextCoord);
			nextCoord = new Point(column - i, row);
			if(ValidateAttack(nextCoord,ref dir2))
				attacks.Add(nextCoord);
			nextCoord = new Point(column, row + i);
			if(ValidateAttack(nextCoord,ref dir3))
				attacks.Add(nextCoord);
			nextCoord = new Point(column, row - i);
			if(ValidateAttack(nextCoord,ref dir4))
				attacks.Add(nextCoord);
		}
		return attacks;
	}
}