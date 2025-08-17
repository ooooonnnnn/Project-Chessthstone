using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

/// <summary>
/// Base class for chess pieces
/// </summary>
public abstract class ChessPiece : Sprite
{
	public PieceType type { get; init; }

	public Player ownerPlayer
	{
		get
		{
			if (_ownerPlayer == null)
				throw new Exception($"Owner player not set for {name}");
			return _ownerPlayer;
		}
		set
		{
			if (value.isWhite != isWhite)
				throw new Exception($"Player {value.name} is not the same color as {name}");
			_ownerPlayer = value;
		}
	}

	private Player _ownerPlayer;

	public bool isWhite;

	public ChessBoard board
	{
		get
		{
			if (_board == null)
				throw new Exception($"Board not set for {name}");
			return _board;
		}
		set => _board = value;
	}

	private ChessBoard _board;

	/// <summary>
	/// Health and damage
	/// </summary>
	public int baseHealth { get; protected set; }
	public int baseDamage { get; protected set; }
	public int health { get; protected set; }

	/// <summary>
	/// Attacking and moving costs 1 action point.
	/// </summary>
	public int actionPoints
	{
		get => _actionPoints;
		set
		{
			_actionPoints = value;
			Console.WriteLine($"{name} has {actionPoints} action points");
		}
	}

	private int _actionPoints = 1;

	/// <summary>
	/// One of its behaviors can be an ability. Assigned automatically on AddBehaviors => ClassifyBehavior
	/// </summary>
	public Ability ability { get; private set; }

	/// <summary>
	/// Current position
	/// </summary>
	public int column => currentSquare.column;
	public int row => currentSquare.row;
	public Point position => new Point(column, row);
	protected ChessSquare currentSquare;

	public event Action OnTeleport;
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
		OnTeleport?.Invoke();
	}
	protected bool canTeleport = true;

	/// <summary>
	/// Base class for chess pieces
	/// </summary>
	protected ChessPiece(bool isWhite, PieceType type, int baseHealth, int baseDamage) : base(CreateName(isWhite, type), TextureManager.GetChessPieceTexture(isWhite, type))
	{
		this.type = type;
		this.isWhite = isWhite;
		this.baseHealth = baseHealth;
		this.baseDamage = baseDamage;
		health = baseHealth;
		
		spriteRenderer.layerDepth = LayerDepthManager.GameObjectDepth;
		
		AddBehaviors([new ChessPieceFeedback()]);
	}

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
		
		DoMoveToSquare(square);

		return true;
	}

	public event Action OnMove;
	/// <summary>
	/// Moves without requiring an action point
	/// </summary>
	/// <param name="square"></param>
	private void DoMoveToSquare(ChessSquare square)
	{
		currentSquare.occupyingPiece = null;
		currentSquare = square;
		currentSquare.occupyingPiece = this;
		transform.parentSpacePos = square.transform.worldSpacePos;
		
		OnMove?.Invoke();
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
			DoMoveToSquare(square);
		}
		
		return true;
	}

	// /// <summary>
	// /// Activates this pieces' ActivatedAbility if it exists
	// /// </summary>
	// /// <returns>True if successful</returns>
	// public bool ActivateAbility(GameObjectReferences objects)
	// {
	// 	if(ability is null or not ActivatedAbility) 
	// 		return false;
	// 	((ActivatedAbility)ability).Activate(objects);
	// 	return true;
	// }
	
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
		return true;
	}
	
	public event Action OnGetHit;
	
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
		
		OnGetHit?.Invoke();
		
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
		OnDeath?.Invoke(this);
		parentScene.RemoveGameObject(this);
	}
	public event Action<ChessPiece> OnDeath;
	
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

	/// <summary>
	/// Gets a reference to the Ability behavior
	/// </summary>
	protected override void ClassifyBehavior(Behavior behavior)
	{
		base.ClassifyBehavior(behavior);
		if (behavior is Ability) ability = (Ability) behavior;
	}

	private static string CreateName(bool isWhite, PieceType type)
	{
		string color = isWhite ? "White" : "Black";
		return $"{color} {type}";
	}

	public override void Dispose()
	{
		base.Dispose();
		OnDeath = null;
		OnGetHit = null;
	}
}