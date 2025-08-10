using System;
using System.Collections.Generic;

namespace MonoGameProject1;

/// <summary>
/// Keeps track of the turns of two players
/// </summary>
public class TurnManager : GameObject
{
	public bool isWhiteTurn { get; private set; } = true;
	private Player _blackPlayer, _whitePlayer;
	
	public TurnManager(string name, Player blackPlayer, Player whitePlayer) : base(name)
	{
		ValidatePlayerColors(blackPlayer, whitePlayer);
		_blackPlayer = blackPlayer;
		_whitePlayer = whitePlayer;
	}

	private void ValidatePlayerColors(Player blackPlayer, Player whitePlayer)
	{
		if (blackPlayer.isWhite || !whitePlayer.isWhite)
			throw new ArgumentException("The black player must be black and the white player must be white");
	}
}