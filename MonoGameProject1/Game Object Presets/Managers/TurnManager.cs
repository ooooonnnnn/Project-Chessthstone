using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameProject1;

/// <summary>
/// Keeps track of the turns of two players.
/// </summary>
public class TurnManager : SingletonGameObject<TurnManager>
{
	/// <summary>
	/// Starts as true because white goes first
	/// </summary>
	public bool isWhiteTurn { get; private set; } = true;
	private Player _blackPlayer, _whitePlayer;
	public ChessBoard board;
	public Player activePlayer => isWhiteTurn ? _whitePlayer : _blackPlayer;
	public Player inactivePlayer => isWhiteTurn ? _blackPlayer : _whitePlayer;

	public static bool Instantiate(string name)
	{
		if (instance != null) 
			return false;
		
		instance = new TurnManager(name);
		return true;
	}

	protected TurnManager(string name) : base(name)
	{
	}

	public void SetPlayers(Player whitePlayer, Player blackPlayer)
	{
		this._blackPlayer = blackPlayer;
		this._whitePlayer = whitePlayer;
		if (blackPlayer.isWhite || !whitePlayer.isWhite)
			throw new ArgumentException("The black player must be black and the white player must be white");
	}

	public event Action<bool> OnTurnChanged;
	/// <summary>
	/// Passes the turn from one player to the other.
	/// </summary>
	public void ChangeTurn()
	{
		if (GamePhaseManager.instance.phase != GamePhase.Gameplay && 
		    GamePhaseManager.instance.phase != GamePhase.Setup)
			return;
		
		EndTurn();
		isWhiteTurn = !isWhiteTurn;
		StartTurn();
		OnTurnChanged?.Invoke(isWhiteTurn);
		TriggerManager.instance.UpdateStateAndTryTrigger(isWhiteTurn);
	}

	/// <summary>
	/// Sets the turn to white and starts turn
	/// </summary>
	public void StartGame()
	{
		isWhiteTurn = true;
		StartTurn();
	}
	
	private void EndTurn()
	{
		Player player = activePlayer;
		board.OnSquareClicked -= player.HandleSquareClicked;
		MouseInput.OnRightClick -= player.TryActivateAbility;
		Console.WriteLine($"{player.name}'s turn ended");
		
		//try changing phase 
		if (player.teamPieces.Count == 0 && inactivePlayer.teamPieces.Count == 0)
			GamePhaseManager.instance.phase = GamePhase.Gameplay;
	}

	private void StartTurn()
	{
		Player player = activePlayer;
		Console.WriteLine($"{player.name}'s turn starting");
		//Subscribe player to input actions
		board.OnSquareClicked += player.HandleSquareClicked;
		MouseInput.OnRightClick += player.TryActivateAbility;
		
		//Reset mana and action points
		if (GamePhaseManager.instance.phase == GamePhase.Gameplay)
		{
			player.mana = 0;
			foreach (var playerPiece in player.pieces)
			{
				playerPiece.actionPoints = 1;
			}
		}
		
		Console.WriteLine($"{player.name}'s turn started");
		GamePhase currentPhase = GamePhaseManager.instance.phase;
		if (currentPhase == GamePhase.Setup)
			Console.WriteLine($"Choose piece to place: [{string.Join(", ", player.teamPieces.Select(p => p.name))}]");
		else if (currentPhase == GamePhase.Gameplay)
		{
			Console.WriteLine(string.Join(", ",
				player.pieces.Select(p => string.Concat(p.name, ": ", p.health, "HP"))));
			Console.WriteLine(string.Join(", ",
				inactivePlayer.pieces.Select(p => string.Concat(p.name, ": ", p.health, "HP"))));
		}
	}

	public override void Dispose()
	{
		base.Dispose();
		OnTurnChanged = null;
	}
}