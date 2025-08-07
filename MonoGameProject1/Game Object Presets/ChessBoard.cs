using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

/// <summary>
/// The chess board, has ChessSquares as children.
/// </summary>
public class ChessBoard : GameObject
{
	public ChessSquare[,] squares { get; init; }
	public Transform transform;
	public float totalWidth => ChessProperties.boardSize * squares[0, 0].spriteRenderer.width;
	
	public ChessBoard(string name) : base(name, [new Transform()])
	{
		//Set own transform reference
		transform = TryGetBehavior<Transform>();
		
		//Construct ChessSquare children
		int boardSize = ChessProperties.boardSize;
		squares = new ChessSquare[boardSize, boardSize];
		for (int i = 0; i < boardSize; i++)
		{
			for (int j = 0; j < boardSize; j++)
			{
				//Construct ChessSquare
				bool isWhite = ChessProperties.IsWhiteSquare(i, j);
				ChessSquare newSquare = new ChessSquare($"Row: {i}, Col: {j}", isWhite);
				squares[i, j] = newSquare;
				
				//Set as child and position it
				Transform squareTransform = newSquare.transform;
				transform.AddChild(squareTransform);
				Vector2 squareSize = new Vector2(
					newSquare.spriteRenderer.width, newSquare.spriteRenderer.height);
				Matrix2x3 positioningMatrix = new Matrix2x3(j, 0, 0, //j - column - x
															0, i, 0);//i - row - y
				squareTransform.parentSpacePos = positioningMatrix * squareSize; 
			}
		}
	}

	/// <summary>
	/// Attempts to place a chesspiece on a square with a certain row and column
	/// </summary>
	/// <param name="row">Square row</param>
	/// <param name="column">Square column</param>
	/// <returns>True if succesful, false otherwise</returns>
	// public bool TryPlacePiece(int row, int column, ChessPiece piece)
	// {
	// 	
	// }
}