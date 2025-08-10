using System;
using System.Collections.Generic;

namespace MonoGameProject1;

/// <summary>
/// Keeps track of the turns of two players
/// </summary>
public class TurnManager : GameObject
{
	/// <summary>
	/// Starts as true because white goes first
	/// </summary>
	public bool isWhiteTurn { get; private set; } = true;
	private Player _blackPlayer, _whitePlayer;
	private ChessBoard _board;
	public Player activePlayer => isWhiteTurn ? _whitePlayer : _blackPlayer;
	
	public TurnManager(string name,ChessBoard board, Player blackPlayer, Player whitePlayer) : base(name)
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

	public void ChangeTurn()
	{
		EndTurn();
		isWhiteTurn = !isWhiteTurn;
		StartTurn();
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
		_board.OnSquareClicked += player.HandleSquareClicked;
		MouseInput.OnRightClick += player.TryActivateAbility;
		player.mana = 10;
		Console.WriteLine($"{player.name}'s turn started");
	}
}