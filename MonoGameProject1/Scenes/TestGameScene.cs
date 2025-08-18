using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Scenes;

public class TestGameScene : Scene
{
	private Player whitePlayer;
	private Player blackPlayer;
	private List<ChessPiece> whiteTeam;
	private List<ChessPiece> blackTeam;
	private ChessBoard board;
	private Button endTurnButton;

	public TestGameScene(IEnumerable<ChessPiece> whiteTeam = null, IEnumerable<ChessPiece> blackTeam = null)
	{
		Console.WriteLine($"Constructing game scene with {whiteTeam.Count()} white pieces.");
		
		TriggerManager.Instantiate("TriggerManager");
		GamePhaseManager.Instantiate("GamePhaseManager");
		TurnManager.Instantiate("TurnManager");
		
		board = new ChessBoard("");
		board.transform.SetScaleFromFloat(0.4f);
		board.transform.origin = Vector2.One * board.totalWidth * 0.5f;
		board.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
		
		whitePlayer = new Player("White", true){board = board};
		blackPlayer = new Player("Black", false){board = board};

		this.whiteTeam = whiteTeam.ToList();
		this.blackTeam = blackTeam.ToList();

		endTurnButton = new Button("End Turn Button", "End Turn");
		((NineSliced)endTurnButton.spriteRenderer).cornerScale = 0.2f;
		
		AddGameObjects([board, whitePlayer, blackPlayer, TurnManager.instance, endTurnButton,
			TriggerManager.instance, 
			GamePhaseManager.instance,
			MatchManager.instance]);
	}

	public override void Initialize()
	{
		Console.WriteLine("Initializing Game Scene");
		
		// whitePlayer.teamPieces = whiteTeam?.ToList() ?? new List<ChessPiece>();
		// blackPlayer.teamPieces = blackTeam?.ToList() ?? new List<ChessPiece>();
		whitePlayer.teamPieces = [new Pawn(true, 1, 1)];
		blackPlayer.teamPieces = [new Pawn(false, 1, 1)];

		foreach (ChessPiece piece in whitePlayer.teamPieces)
		{
			
			piece.ownerPlayer = whitePlayer;
		}

		foreach (ChessPiece piece in blackPlayer.teamPieces)
		{
			piece.ownerPlayer = blackPlayer;
		}
		
		foreach (var piece in whitePlayer.teamPieces.Concat(blackPlayer.teamPieces))
		{
			piece.SetActive(true);
			piece.InitializeBehaviors();
			piece.board = board;
			piece.transform.origin = Vector2.Zero;
			piece.transform.parentSpacePos = Vector2.Zero;
		}
		
		endTurnButton.AddListener(() =>
		{
			if (GamePhaseManager.instance.phase == GamePhase.Gameplay)
				TurnManager.instance.ChangeTurn();
		});
		

		GamePhaseManager.instance.phase = GamePhase.Setup;
	}
}