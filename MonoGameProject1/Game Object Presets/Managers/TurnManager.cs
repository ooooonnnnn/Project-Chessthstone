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
	public ChessBoard Board;
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
		GamePhaseManager.instance.OnPhaseChanged += (prev, phase) =>
		{
			if (phase is GamePhase.Gameplay or GamePhase.Setup && prev == GamePhase.None)
			{
				StartTurn();
			}
		};
	}

	public void SetPlayers(Player blackPlayer, Player whitePlayer)
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
	/// <param name="sendTrigger">Default true. set false to prevent turn change triggers (in setup phase)</param>
	public void ChangeTurn()
	{
		EndTurn();
		isWhiteTurn = !isWhiteTurn;
		OnTurnChanged?.Invoke(isWhiteTurn);
		StartTurn();
		TriggerManager.instance.UpdateStateAndTryTrigger(isWhiteTurn);
	}

	private void EndTurn()
	{
		Player player = activePlayer;
		Board.OnSquareClicked -= player.HandleSquareClicked;
		MouseInput.OnRightClick -= player.TryActivateAbility;
		Console.WriteLine($"{player.name}'s turn ended");
		
		//try changing phase 
		if (player.teamPieces.Count == 0 && inactivePlayer.teamPieces.Count == 0)
			GamePhaseManager.instance.phase = GamePhase.Gameplay;
	}

	private void StartTurn()
	{
		Player player = activePlayer;
		//Subscribe player to input actions
		Board.OnSquareClicked += player.HandleSquareClicked;
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
}