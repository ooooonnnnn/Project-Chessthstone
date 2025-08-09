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
	/// Current position
	/// </summary>
	protected int row, column;
	protected int baseHealth = baseHealth, baseDamage = baseDamage, health = baseHealth;

	/// <summary>
	/// Moves the piece to the target square
	/// </summary>
	public void GoToSquare(ChessSquare square)
	{
		row = square.row;
		column = square.column;
		transform.parentSpacePos = square.transform.worldSpacePos;
	}

	/// <summary>
	/// Attacks the piece that's on the square.
	/// Assumes occupied square 
	/// </summary>
	public bool AttackPieceOnSquare(ChessSquare square)
	{
		ChessPiece attackedPiece = square.occupyingPiece;
		return attackedPiece.TakeDamage(baseDamage);
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
	/// <param name="nextCoord"></param>
	/// <param name="directionValid"></param>
	/// <returns></returns>
	protected bool ValidateAttack(Point nextCoord, ref bool directionValid)
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