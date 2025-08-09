using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

/// <summary>
/// Base class for chess pieces
/// </summary>
public abstract class ChessPiece(ChessBoard board, bool isWhite, PieceType type, int baseHealth, int baseDamage)
	: Sprite(ChessPieceName(isWhite, type), TextureManager.GetChessPieceTexture(isWhite, type))
{
	public PieceType type { get; init; } = type;
	public bool isWhite { get; init; } = isWhite;
	public ChessBoard board { get; init; } = board;

	/// <summary>
	/// Attacking, moving, and activating abilities costs 1 action point.
	/// </summary>
	public int actionPoints { get; private set; } = 1;

	/// <summary>
	/// Current position
	/// </summary>
	protected int column => currentSquare.column;
	protected int row => currentSquare.row;
	protected ChessSquare currentSquare;
	protected int baseHealth = baseHealth, baseDamage = baseDamage, health = baseHealth;

	/// <summary>
	/// Moves the piece to the target square. To move the piece, use MoveToSquare
	/// </summary>
	public void TeleportToSquare(ChessSquare square)
	{
		if (!canTeleport)
		{
			Console.WriteLine($"{name} can't teleport");
			return;
		}
		if (currentSquare != null) 
			currentSquare.occupyingPiece = null;
		currentSquare = square;
		currentSquare.occupyingPiece = this;
		transform.parentSpacePos = square.transform.worldSpacePos;
		canTeleport = false;
	}
	protected bool canTeleport = true;
	/// <summary>
	/// Use this if you want the piece to teleport after the initial spawning.
	/// </summary>
	public void EnableTeleportOnce() => canTeleport = true;

	/// <summary>
	/// Tries to move. Checks valid movement before moving. Requires an action point 
	/// </summary>
	/// <returns>True if move was succesful</returns>
	public bool MoveToSquare(ChessSquare square)
	{
		if (!PayActionPoint()) return false;
		
		Point nextCoord = new Point(square.column, square.row);
		if (!GetMoveCoordList().Contains(nextCoord)) return false;
		
		currentSquare.occupyingPiece = null;
		currentSquare = square;
		currentSquare.occupyingPiece = this;
		transform.parentSpacePos = square.transform.worldSpacePos;

		return true;
	}

	/// <summary>
	/// Tries attacking the piece that's on the square. Moves if the attacked piece died.
	/// </summary>
	/// <returns>True if successful</returns>
	public bool AttackPieceOnSquare(ChessSquare square)
	{
		if (!PayActionPoint()) return false;
		
		Point nextCoord = new Point(square.column, square.row);
		if(!GetAttackCoordList().Contains(nextCoord)) return false;
		
		ChessPiece attackedPiece = square.occupyingPiece;
		if (attackedPiece.TakeDamage(baseDamage))
		{
			MoveToSquare(square);
		}
		
		return true;
	}

	/// <summary>
	/// Tries to pay one action point. 
	/// </summary>
	/// <returns>True if there was an action point to pay</returns>
	private bool PayActionPoint()
	{
		if (actionPoints <= 0)
		{
			Console.WriteLine($"{name} has no available action points");
			return false;
		}
		actionPoints--;
		Console.WriteLine($"{name} has {actionPoints} action points left");
		return true;
	}
	
	/// <summary>
	/// Takes damage and dies if necessary
	/// </summary>
	/// <param name="damage">amount of damage</param>
	/// <returns>true of this piece died</returns>
	public bool TakeDamage(int damage)
	{
		Console.WriteLine($"{name} took {damage} damage");
		bool die = false;
		health -= damage;
		if (health <= 0)
		{
			die = true;
			health = 0;
		}
		Console.WriteLine($"{name} has {health}/{baseHealth} health");

		if (die)
			Die();

		return die;
	}

	/// <summary>
	/// Plays death animation and removes the piece from the scene
	/// </summary>
	private void Die()
	{
		//TODO: add death animation
		Console.WriteLine($"{name} is dead!");
		currentSquare.occupyingPiece = null;
		parentScene.RemoveGameObject(this);
	}
	
	/// <summary>
	/// List of int coordinates of the squares this piece can move to. <br/>
	/// Takes the board size and existing pieces into account
	/// </summary>
	public abstract List<Point> GetMoveCoordList();

	/// <summary>
	/// List of coordinates of the squares this piece can attack.<br/>
	/// Takes the board size and existing pieces into account
	/// </summary>
	public abstract List<Point> GetAttackCoordList();
	
	/// <summary>
	/// Returns true if: 1. directionValid is true 2. the target square is within the board 3. the target square is unoccupied.<br/>
	/// Returns false otherwise and sets directionValid false.
	/// Used for constructing the possible moves of a piece. 
	/// </summary>
	/// <param name="nextCoord">The move to consider</param>
	/// <param name="directionValid">If the move is part of a direction, true means the direction is not blocked and
	/// within the board. The function checks and updates its value</param>
	protected bool ValidateMove(Point nextCoord, ref bool directionValid)
	{
		if (directionValid)
		{
			if (!ChessProperties.IsPointInBoard(nextCoord))
			{
				directionValid = false;
			}
			else if (board.squares[nextCoord.X, nextCoord.Y].occupyingPiece != null)
			{
				directionValid = false;
			}
		}
		return directionValid;
	}

	/// <summary>
	/// Returns true if: 1. directionValid is true 2. the target square is within the board 3. the target square is<br/>
	///  occupied by an opposing piece.
	/// If (2) is false or the target square is occupied by an ally, returns false and updates directionValid <br/>
	/// False otherwise
	/// </summary>
	/// <param name="nextCoord">The move to consider</param>
	/// <param name="directionValid">If the move is part of a direction, true means the direction is not blocked and
	/// within the board. The function checks and updates its value</param>
	/// <returns>True if the attack is valid</returns>
	protected bool ValidateAttackCoord(Point nextCoord, ref bool directionValid)
	{
		if(!directionValid) 
			return false;

		if (!ChessProperties.IsPointInBoard(nextCoord))
		{
			directionValid = false;
			return false;
		}
		
		ChessPiece occupyingPiece = board.squares[nextCoord.X, nextCoord.Y].occupyingPiece;
		if (occupyingPiece == null)
		{
			return false;
			//Square not blocked, can continue searching this direction
		}
		if (occupyingPiece.isWhite != isWhite)
		{
			directionValid = false;
			return true;
			//Found valid attack, stop searching this direction
		}
		//Square blocked, stop searching this direction
		directionValid = false;
		return false;
	}

	private static string ChessPieceName(bool isWhite, PieceType type)
	{
		string color = isWhite ? "White" : "Black";
		return $"{color} {type}";
	}
}