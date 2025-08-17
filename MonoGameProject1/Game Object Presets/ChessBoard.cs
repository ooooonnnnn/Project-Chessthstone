using System;
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
	public float totalWidth => ChessProperties.boardSize * squares[0, 0].spriteRenderer.sourceWidth;
	public event Action<ChessSquare> OnSquareClicked;
	
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
				ChessSquare newSquare = new ChessSquare($"Row: {i}, Col: {j}", this, i, j, isWhite);
				newSquare.board = this;
				squares[i, j] = newSquare;
				
				//Set as child and position it
				Transform squareTransform = newSquare.transform;
				transform.AddChild(squareTransform);
				Vector2 squareSize = new Vector2(
					newSquare.spriteRenderer.sourceWidth, newSquare.spriteRenderer.sourceHeight);
				
				squareTransform.parentSpacePos = new Vector2(
					squareSize.X * i, squareSize.Y * j); //i - column - x | j - row - y
			}
		}
	}

	public void SquareClicked(ChessSquare square)
	{
		OnSquareClicked?.Invoke(square);
	}
}