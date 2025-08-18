using System.Collections.Generic;

namespace MonoGameProject1;

public static class ChessPieceFactory
{
	/// <summary>
	/// Gets an enumerator of all chess pieces of a certain color (determined by the owner player) and type
	/// </summary>
	/// <returns></returns>
	public static IEnumerable<ChessPiece> GetAllPieces(bool isWhite, PieceType type)
	{
		switch (type)
		{
			case PieceType.Pawn:
			{
				yield return new Pawn(isWhite, 1, 1); 
				yield break;
			}
			case PieceType.Knight:
				yield return new KnightRegainAp(isWhite);
				yield return new Knight(isWhite, 1 ,1);
				yield break;
			case PieceType.Bishop:
				yield return new BishopRegainAp(isWhite);
				yield return new Bishop(isWhite, 1 ,1);
				yield break;
			case PieceType.Rook:
				yield return new Rook(isWhite, 1 ,1);
				yield break;
			case PieceType.Queen:
				yield return new Queen(isWhite, 1 ,1);
				yield break;
			case PieceType.King:
				yield return new KingGainManaFromAdj(isWhite);
				yield return new King(isWhite, 1 ,1);
				yield break;
		}
	}
}