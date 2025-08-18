using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

/// <summary>
/// Records a state of the game.
/// </summary>
public readonly struct GameState
{
	public GameState(bool isWhiteTurn, Scene gameScene)
	{
		this.isWhiteTurn = isWhiteTurn;
		_blackPieces = new List<Point>();
		_whitePieces = new List<Point>();
		foreach (GameObject gameObject in gameScene.gameObjects)
		{
			// if (gameObject is ChessBoard board)
			// {
			// 	foreach (ChessSquare square in board.squares)
			// 	{
			// 		if (square.occupyingPiece?.isWhite is true)
			// 			_whitePieces.Add(new Point(square.column, square.row));
			// 		else if (square.occupyingPiece?.isWhite is false)
			// 			_blackPieces.Add(new Point(square.column, square.row));
			// 	}
			// }
			
			if (gameObject is ChessPiece chessPiece)
			{
				Point coords = new Point(chessPiece.column, chessPiece.row);
				if (chessPiece.isWhite)
					_whitePieces.Add(coords);
				else
					_blackPieces.Add(coords);
			}
		}
	}

	public bool isWhiteTurn { get; init; }
	private readonly List<Point> _blackPieces;
	private readonly List<Point> _whitePieces;
	public IReadOnlyList<Point> blackPieces => _blackPieces;
	public IReadOnlyList<Point> whitePieces => _whitePieces;
}