using System;
using System.Collections.Generic;

namespace MonoGameProject1;

/// <summary>
/// Keeps track of the turns of two players
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
		StartTurn();
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
	public void ChangeTurn(bool sendTrigger = true)
	{
		EndTurn();
		isWhiteTurn = !isWhiteTurn;
		StartTurn();
		if (sendTrigger)
			TriggerManager.instance.UpdateStateAndTrigger(isWhiteTurn);
		else
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
		player.mana = 0;
		foreach (var playerPiece in player.pieces)
		{
			playerPiece.actionPoints = 1;
		}
		
		Console.WriteLine($"{player.name}'s turn started");
	}
}