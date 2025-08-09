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
	
	private ChessPiece _selectedPiece;
	
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
					newSquare.spriteRenderer.sourceWidth, newSquare.spriteRenderer.sourceHeight);
				
				squareTransform.parentSpacePos = new Vector2(
					squareSize.X * i, squareSize.Y * j); //i - column - x | j - row - y
			}
		}
	}

	/// <summary>
	/// Handles what happens when a square is clicked: <br/>
	/// Nothing selected & empty square => Spawn new piece <br/>
	/// Piece selected & square is valid move => move <br/>
	/// Piece selected & square is valid attack => attack
	/// </summary>
	public void HandleSquareClicked(ChessSquare square)
	{
		if (_selectedPiece == null) //no piece selected => create or select
		{
			if (square.occupyingPiece == null)
			{
				ChessPiece newPiece = PieceFactory.CreateRandomPiece(this);
				// ChessPiece newPiece = PieceFactory.CreatePiece(this, QuickRandom.NextInt(0,2) == 0, PieceType.Pawn);
				newPiece.transform.SetScaleFromFloat(square.transform.worldSpaceScale.X);
				parentScene.AddGameObjects([newPiece]);
				newPiece.TeleportToSquare(square);
			}
			else
			{
				_selectedPiece = square.occupyingPiece;
				//Test: show all possible attacks
				foreach (Point move in _selectedPiece.GetAttackCoordList())
				{
					squares[move.X, move.Y].spriteRenderer.color = Color.Red;
				}
				//Test: show all possible moves
				foreach (Point move in _selectedPiece.GetMoveCoordList())
				{
					squares[move.X, move.Y].spriteRenderer.color = Color.Green;
				}
			}
		}
		else //piece selected => move or attack or deselect
		{
			if (square.occupyingPiece == _selectedPiece)
			{
				DeselectAll();
			}
			else
			{
				Point squareCoords = new Point(square.column, square.row);
				if (_selectedPiece.GetMoveCoordList().Contains(squareCoords))
				{
					if (_selectedPiece.MoveToSquare(square))
						DeselectAll();
				}
				else if (_selectedPiece.GetAttackCoordList().Contains(squareCoords))
				{
					if (_selectedPiece.AttackPieceOnSquare(square)) //true if successful attack
						DeselectAll();
				}
				else
				{
					Console.WriteLine("Square not in possible moves, try again");
				}
			}
		}
		
	}

	private void DeselectAll()
	{
		_selectedPiece = null;
		foreach (ChessSquare square in squares)
		{
			square.spriteRenderer.color = Color.White;
		}
	}
}