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
	private ChessBoard _board;
	public Player activePlayer => isWhiteTurn ? _whitePlayer : _blackPlayer;
	public Player inactivePlayer => isWhiteTurn ? _blackPlayer : _whitePlayer;

	public static bool Instantiate(string name, ChessBoard board, Player blackPlayer, Player whitePlayer)
	{
		if (instance != null) 
			return false;
		
		instance = new TurnManager(name, board, blackPlayer, whitePlayer);
		return true;
	}

	protected TurnManager(string name,ChessBoard board, Player blackPlayer, Player whitePlayer) : base(name)
	{
		ValidatePlayerColors(blackPlayer, whitePlayer);
		_blackPlayer = blackPlayer;
		_whitePlayer = whitePlayer;
		_board = board;
		GamePhaseManager.instance.OnPhaseChanged += (prev, phase) =>
		{
			if (phase is GamePhase.Gameplay or GamePhase.Setup && prev == GamePhase.None)
			{
				StartTurn();
			}
		};
	}

	private void ValidatePlayerColors(Player blackPlayer, Player whitePlayer)
	{
		if (blackPlayer.isWhite || !whitePlayer.isWhite)
			throw new ArgumentException("The black player must be black and the white player must be white");
	}

	/// <summary>
	/// Passes the turn from one player to the other.
	/// </summary>
	/// <param name="sendTrigger">Default true. set false to prevent turn change triggers (in setup phase)</param>
	public void ChangeTurn()
	{
		EndTurn();
		isWhiteTurn = !isWhiteTurn;
		StartTurn();
		GamePhase currentPhase = GamePhaseManager.instance.phase;
		if (currentPhase == GamePhase.Gameplay)
			TriggerManager.instance.UpdateStateAndTrigger(isWhiteTurn);
		else if (currentPhase == GamePhase.Setup)
			TriggerManager.instance.UpdateGameState(isWhiteTurn);
	}

	private void EndTurn()
	{
		Player player = activePlayer;
		_board.OnSquareClicked -= player.HandleSquareClicked;
		MouseInput.OnRightClick -= player.TryActivateAbility;
		Console.WriteLine($"{player.name}'s turn ended");
	}

	private void StartTurn()
	{
		Player player = activePlayer;
		//Subscribe player to input actions
		_board.OnSquareClicked += player.HandleSquareClicked;
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
		if (player.teamPieces.Count > 0)
			Console.WriteLine($"Choose piece to place: [{string.Join(", ", player.teamPieces.Select(p => p.name))}]");
		else
		{
			Console.WriteLine(string.Join(", ",
				player.pieces.Select(p => string.Concat(p.name, ": ", p.health, "HP"))));
			Console.WriteLine(string.Join(", ",
				inactivePlayer.pieces.Select(p => string.Concat(p.name, ": ", p.health, "HP"))));
		}
	}
}