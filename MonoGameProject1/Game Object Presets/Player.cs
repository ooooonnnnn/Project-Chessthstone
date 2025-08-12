using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

/// <summary>
/// A player in the game.
/// </summary>
public class Player(string name, bool isWhite, ChessBoard board) : GameObject(name)
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
	public bool isWhite { get; } = isWhite;
	/// <summary>
	/// The pieces the player starts with and places when the game starts
	/// </summary>
	public List<ChessPiece> teamPieces;
	private ChessPiece _pieceToPlace;
	/// <summary>
	/// The pieces that are currently on the board
	/// </summary>
	private List<ChessPiece> _activePieces { get; } = new();
	public IReadOnlyList<ChessPiece> pieces => _activePieces;
	private ChessBoard board { get; } = board;

	/// <summary>
	/// Handles what happens when a square is clicked: <br/>
	/// Nothing selected and empty square => Spawn a new piece. <br/>
	/// Piece selected and square is valid move => move <br/>
	/// Piece selected and square is valid attack => attack
	/// </summary>
	public void HandleSquareClicked(ChessSquare square)
	{
		if (teamPieces.Count > 0)
		{
			//try placing a piece
			//pass the turn if successful
			//TODO: test
			_pieceToPlace = teamPieces[0];
			if (TryPlacePiece(square))
			{
				TurnManager.instance.ChangeTurn();
			}
			return;
		}
		
		if (_selectedActivePiece == null) //no piece selected, try select
		{
			if (square.occupyingPiece == null)
				return;
			if (square.occupyingPiece.isWhite != isWhite) //piece doesn't belongs to this player => can't select select it
				return;
			
			_selectedActivePiece = square.occupyingPiece;
			//Test: show all possible attacks
			foreach (Point move in _selectedActivePiece.GetAttackCoordList())
			{
				board.squares[move.X, move.Y].spriteRenderer.color = Color.Red;
			}
			//Test: show all possible moves
			foreach (Point move in _selectedActivePiece.GetMoveCoordList())
			{
				board.squares[move.X, move.Y].spriteRenderer.color = Color.Green;
			}
		}
		else //piece selected => move or attack or deselect
		{
			if (square.occupyingPiece == _selectedActivePiece)
			{
				DeselectAll();
			}
			else
			{
				Point squareCoords = new Point(square.column, square.row);
				if (_selectedActivePiece.GetMoveCoordList().Contains(squareCoords))
				{
					if (_selectedActivePiece.MoveToSquare(square))
					{
						DeselectAll();
						//Inform trigger manager
						TriggerManager.instance.UpdateStateAndTrigger(isWhite);
					}
				}
				else if (_selectedActivePiece.GetAttackCoordList().Contains(squareCoords))
				{
					if (_selectedActivePiece.AttackPieceOnSquare(square)) //true if successful attack
					{
						DeselectAll();
						//Inform trigger manager
						TriggerManager.instance.UpdateStateAndTrigger(isWhite);
					}
				}
				else
				{
					Console.WriteLine("Square not in possible moves, try again");
				}
			}
		}
		
	}
	private ChessPiece _selectedActivePiece;

	//TODO: this should probably be in another class like ChessBoard
	private bool TryPlacePiece(ChessSquare square)
	{
		if (_pieceToPlace == null)
			return false;
		//Can't place if the square is occupied
		if (square.occupyingPiece != null)
			return false;
		//check square is on my side
		bool onMySide = isWhite ? square.column >= ChessProperties.boardSize / 2 :
			square.column < ChessProperties.boardSize / 2;
		if (!onMySide)
			return false;
		
		_activePieces.Add(_pieceToPlace);
		teamPieces.Remove(_pieceToPlace);
		_pieceToPlace.OnDeath += pieceToRemove =>
		{
			Console.WriteLine($"Removing {pieceToRemove.name}");
			_activePieces.Remove(pieceToRemove);
		};

		_pieceToPlace.transform.SetScaleFromFloat(square.transform.worldSpaceScale.X);
		parentScene.AddGameObjects([_pieceToPlace]);
		_pieceToPlace.TeleportToSquare(square);

		return true;
	}
	
	public void TryActivateAbility()
	{
		ActivatedAbility activatedAbility = _selectedActivePiece?.ability as ActivatedAbility;
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
		//Inform trigger manager
		TriggerManager.instance.UpdateStateAndTrigger(isWhite);
		DeselectAll();
	}

	private void DeselectAll()
	{
		_selectedActivePiece = null;
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
