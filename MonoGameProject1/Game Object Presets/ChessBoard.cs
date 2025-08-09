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
	public float totalWidth => ChessProperties.boardSize * squares[0, 0].spriteRenderer.width;
	
	private ChessPiece _selectedPiece;
	private ChessSquare _selectedSquare;
	
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
				ChessSquare newSquare = new ChessSquare($"Row: {i}, Col: {j}", i, j, isWhite);
				newSquare.board = this;
				squares[i, j] = newSquare;
				
				//Set as child and position it
				Transform squareTransform = newSquare.transform;
				transform.AddChild(squareTransform);
				Vector2 squareSize = new Vector2(
					newSquare.spriteRenderer.width, newSquare.spriteRenderer.height);
				
				squareTransform.parentSpacePos = new Vector2(
					squareSize.X * i, squareSize.Y * j); //i - column - x | j - row - y
			}
		}
	}

	/// <summary>
	/// Handles what happens when a square is clicked: <br/>
	/// Nothing selected & empty square => Spawn new piece <br/>
	/// </summary>
	public void HandleSquareClicked(ChessSquare square)
	{
		if (_selectedPiece == null) //no piece selected => create or select
		{
			if (square.occupyingPiece == null)
			{
				// ChessPiece newPiece = PieceFactory.CreateRandomPiece(this);
				ChessPiece newPiece = PieceFactory.CreatePiece(this, QuickRandom.NextInt(0,2) == 0, PieceType.Pawn);
				newPiece.transform.SetScaleFromFloat(square.transform.worldSpaceScale.X);
				parentScene.AddGameObjects([newPiece]);
				square.SetPiece(newPiece);;
			}
			else
			{
				_selectedPiece = square.occupyingPiece;
				_selectedSquare = square;
				//Test: show all possible attacks
				foreach (Point move in _selectedPiece.GetAttackCoordList())
				{
					squares[move.X, move.Y].spriteRenderer.color = Color.Red;
				}
			}
		}
		else //piece selected => move or deselect
		{
			if (square == _selectedSquare)
			{
				DeselectAll();
			}
			else if (square.occupyingPiece == null)
			{
				Point squareCoords = new Point(square.column, square.row);
				if(_selectedPiece.GetMoveCoordList().Contains(squareCoords))
				{
					_selectedSquare.occupyingPiece = null;
					_selectedPiece.GoToSquare(square);
					square.occupyingPiece = _selectedPiece;
					DeselectAll();
				}
				else
				{
					Console.WriteLine("Square not in possible moves, try again");
				}
			}
			else
			{
				Console.WriteLine("Can't move to occupied square, try again");
			}
		}
		
	}

	private void DeselectAll()
	{
		_selectedSquare = null;
		_selectedPiece = null;
		foreach (ChessSquare square in squares)
		{
			square.spriteRenderer.color = Color.White;
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