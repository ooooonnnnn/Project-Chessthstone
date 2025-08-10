using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

/// <summary>
/// A player in the game.
/// </summary>
public class Player : GameObject
{
	/// <summary>
	/// Mana to pay for activated abilities
	/// </summary>
	public int mana
	{
		get => _mana;
		set
		{
			_mana = value;
			Console.WriteLine($"{name} has {_mana} mana");
		}
	}
	private int _mana;
	/// <summary>
	/// Color. White goes first
	/// </summary>
	public bool isWhite { get; } 
	private ChessBoard board { get; }

	public Player(string name, bool isWhite, ChessBoard board) : base(name)
	{
		this.board = board;
		this.isWhite = isWhite;
	}
	
	/// <summary>
	/// Handles what happens when a square is clicked: <br/>
	/// Nothing selected and empty square => Spawn new piece <br/>
	/// Piece selected and square is valid move => move <br/>
	/// Piece selected and square is valid attack => attack
	/// </summary>
	public void HandleSquareClicked(ChessSquare square)
	{
		if (_selectedPiece == null) //no piece selected => create or select
		{
			if (square.occupyingPiece == null) //empty square => create piece
			{
				ChessPiece newPiece = new  BasicBishop(board, this);
				// ChessPiece newPiece = PieceFactory.CreateRandomPiece(board, this);
				// ChessPiece newPiece = PieceFactory.CreatePiece(this, QuickRandom.NextInt(0,2) == 0, PieceType.Pawn);
				newPiece.transform.SetScaleFromFloat(square.transform.worldSpaceScale.X);
				parentScene.AddGameObjects([newPiece]);
				newPiece.TeleportToSquare(square);
			}
			else if (square.occupyingPiece.isWhite == isWhite) //piece belongs to this player => select it
			{
				_selectedPiece = square.occupyingPiece;
				//Test: show all possible attacks
				foreach (Point move in _selectedPiece.GetAttackCoordList())
				{
					board.squares[move.X, move.Y].spriteRenderer.color = Color.Red;
				}
				//Test: show all possible moves
				foreach (Point move in _selectedPiece.GetMoveCoordList())
				{
					board.squares[move.X, move.Y].spriteRenderer.color = Color.Green;
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
	private ChessPiece _selectedPiece;

	public void TryActivateAbility()
	{
		ActivatedAbility activatedAbility = _selectedPiece?.ability as ActivatedAbility;
		if (activatedAbility == null)
		{
			Console.WriteLine("Selected piece does not have an activated ability");
			return;
		}

		if (mana < activatedAbility.manaCost)
		{
			Console.WriteLine("Not enough mana to activate ability");
			return;
		}
		
		mana -= activatedAbility.manaCost;
		activatedAbility.Activate(null);
		DeselectAll();
	}

	private void DeselectAll()
	{
		_selectedPiece = null;
		foreach (ChessSquare square in board.squares)
		{
			square.spriteRenderer.color = Color.White;
		}
	}
	
	public override void Dispose()
	{
		base.Dispose();
		MouseInput.OnRightClick -= TryActivateAbility;
		if (board != null)
			board.OnSquareClicked -= HandleSquareClicked;
	}
}
