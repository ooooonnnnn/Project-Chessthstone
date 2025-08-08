using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

public class Pawn : ChessPiece
{
	public Pawn(bool isWhite) : base(isWhite, PieceType.Pawn)
	{
	}

	public override List<Point> GetPossibleMoves()
	{
		List<Point> moves = new();
		if (column > 0)
			moves.Add(new Point(column - 1, row));
		if (column < ChessProperties.boardSize - 1) 
			moves.Add(new Point(column + 1, row));
		if (row > 0)
			moves.Add(new Point(column, row - 1));
		if(row < ChessProperties.boardSize - 1)
			moves.Add(new Point(column, row + 1));
		return moves;
	}
}