using System;
using System.Collections.Generic;
using MonoGameProject1.Scenes;

namespace MonoGameProject1;

public class MatchManager : SingletonGameObject<MatchManager>
{
	protected MatchManager(string name, List<Behavior> behaviors = null) : base(name, behaviors)
	{
	}

	public ChessBoard board;
	
	public static bool Instantiate(string name)
	{
		if (instance != null) 
			return false;
		
		instance = new MatchManager(name);
		return true;
	}
	
	public void CheckWin()
	{
		if (board == null)
			throw new Exception("Can't check who won because board isn't set");

		bool whitesRemain = false; 
		bool blacksRemain = false;
		foreach (ChessSquare square in board.squares)
		{
			bool? pieceColor = square.occupyingPiece?.isWhite;
			if (pieceColor is true)
				whitesRemain = true;
			if (pieceColor is false)
				blacksRemain = true;
		}

		if (!blacksRemain && whitesRemain)
			GamePhaseManager.instance.phase = GamePhase.WhiteWon;
		else if (!whitesRemain && blacksRemain)
			GamePhaseManager.instance.phase = GamePhase.BlackWon;
	}

	public override void Start()
	{
		base.Start();
		GamePhaseManager.instance.OnPhaseChanged += (prev, current) =>
		{
			if (prev == GamePhase.Gameplay)
			{
				if (current == GamePhase.WhiteWon)
					SceneManager.ChangeScene(new WinScreenScene(true));
				else if (current == GamePhase.BlackWon)
					SceneManager.ChangeScene(new WinScreenScene(false));
			}
		};
	}
}