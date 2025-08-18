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
				yield return new PawnBasic(isWhite);
				yield return new PawnAttackNearby(isWhite);
				yield break;
			}
			case PieceType.Knight:
				yield return new KnightBasic(isWhite);
				yield return new KnightRegainAp(isWhite);
				yield break;
			case PieceType.Bishop:
				yield return new BishopBasic(isWhite);
				yield return new BishopRegainAp(isWhite);
				yield break;
			case PieceType.Rook:
				yield return new RookBasic(isWhite);
				yield return new RookAttackInPlusRange(isWhite);
				yield break;
			case PieceType.Queen:
				yield return new QueenBasic(isWhite);
				yield return new QueenGainManaOnAttack(isWhite);
				yield break;
			case PieceType.King:
				yield return new KingBasic(isWhite);
				yield return new KingGainManaFromAdj(isWhite);
				yield break;
		}
	}
}