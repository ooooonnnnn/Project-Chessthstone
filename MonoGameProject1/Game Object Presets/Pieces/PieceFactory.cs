namespace MonoGameProject1;

/// <summary>
/// Creates different pieces <br/>
/// "Many chess piece in my facotry"
/// </summary>
public static class PieceFactory
{
	public static ChessPiece CreatePiece(ChessBoard board, bool isWhite, PieceType type)
	{
		switch (type)
		{
			case PieceType.Pawn:
				return new Pawn(board, isWhite, 20, 5);
			case PieceType.Knight:
				return new Knight(board, isWhite, 20, 5);
			case PieceType.Bishop:
				return new Bishop(board, isWhite, 20, 5);
			case PieceType.Rook:
				return new Rook(board, isWhite, 20, 5);
			case PieceType.Queen:
				return new Queen(board, isWhite, 20, 5);
			case PieceType.King:
				return new King(board, isWhite, 20, 5);
			default:
				return null;
		}
	}

	public static ChessPiece CreateRandomPiece(ChessBoard board)
	{
		return CreatePiece(board, QuickRandom.NextInt(0, 2) == 0, (PieceType)QuickRandom.NextInt(0, 6));
	}
}