using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

public class Bishop(ChessBoard board, bool isWhite) : ChessPiece(board, isWhite, PieceType.Bishop)
{
	public override List<Point> GetPossibleMoves()
	{
		List<Point> moves = new();
		bool dir1 = true, dir2 = true, dir3 = true, dir4 = true; //Free directions
		int boardSize = ChessProperties.boardSize;
		for (int i = 1; i < boardSize && (dir1 || dir2 || dir3 || dir4); i++)
		{
			Point nextCoord = new Point(column + i, row + i); //potential coordinate to add
			if(ValidatePossibleMove(nextCoord, ref dir1))
				moves.Add(nextCoord);
			nextCoord = new Point(column - i, row + i);
			if(ValidatePossibleMove(nextCoord, ref dir2))
				moves.Add(nextCoord);
			nextCoord = new Point(column + i, row - i);
			if(ValidatePossibleMove(nextCoord, ref dir3))
				moves.Add(nextCoord);
			nextCoord = new Point(column - i, row - i);
			if(ValidatePossibleMove(nextCoord, ref dir4))
				moves.Add(nextCoord);
		}
		return moves;
	}
}