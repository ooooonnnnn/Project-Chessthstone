namespace MonoGameProject1;

/// <summary>
/// Creates different pieces <br/>
/// "Many chess piece in my facotry"
/// </summary>
public static class PieceFactory
{
	public static ChessPiece CreatePiece(ChessBoard board, Player owner, PieceType type)
	{
		switch (type)
		{
			case PieceType.Pawn:
				return new Pawn(board, owner, 20, 5);
			case PieceType.Knight:
				return new Knight(board, owner, 20, 5);
			case PieceType.Bishop:
				return new Bishop(board, owner, 20, 5);
			case PieceType.Rook:
				return new Rook(board, owner, 20, 5);
			case PieceType.Queen:
				return new Queen(board, owner, 20, 5);
			case PieceType.King:
				return new King(board, owner, 20, 5);
			default:
				return null;
		}
	}

	public static ChessPiece CreateRandomPiece(ChessBoard board, Player owner)
	{
		return CreatePiece(board, owner, (PieceType)QuickRandom.NextInt(0, 6));
	}
}