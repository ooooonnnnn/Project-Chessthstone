namespace MonoGameProject1;

/// <summary>
/// General properties of the chess game
/// </summary>
public static class ChessProperties
{
	public const int boardSize = 8;
	
	public static bool IsWhiteSquare(int row, int column) => (row + column) % 2 == 0;
}