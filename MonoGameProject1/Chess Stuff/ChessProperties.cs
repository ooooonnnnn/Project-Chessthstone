using Microsoft.Xna.Framework;

namespace MonoGameProject1;

/// <summary>
/// General properties of the chess game
/// </summary>
public static class ChessProperties
{
	public const int boardSize = 8;
	
	public static bool IsWhiteSquare(int row, int column) => (row + column) % 2 == 0;
	
	/// <summary>
	/// Checks if the given coordinate is in the board
	/// </summary>
	public static bool IsPointInBoard(Point coord)
	{
		return coord.X < boardSize && coord.X >= 0
		                               && coord.Y < boardSize && coord.Y >= 0;
	}
}