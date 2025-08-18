using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Scenes;

public class TestGameScene : Scene
{
	private Player whitePlayer;
	private Player blackPlayer;


	public TestGameScene(IEnumerable<ChessPiece> whiteTeam = null, IEnumerable<ChessPiece> blackTeam = null)
	{
		TriggerManager.Instantiate("TriggerManager");
		GamePhaseManager.Instantiate("GamePhaseManager");
		TurnManager.Instantiate("TurnManager");
		
		ChessBoard board = new ChessBoard("");
		board.transform.SetScaleFromFloat(0.4f);
		board.transform.origin = Vector2.One * board.totalWidth * 0.5f;
		board.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();

		
		whitePlayer = new Player("White", true){board = board};
		blackPlayer = new Player("Black", false){board = board};

		whitePlayer.teamPieces = whiteTeam?.ToList() ?? new List<ChessPiece>();
		blackPlayer.teamPieces = blackTeam?.ToList() ?? new List<ChessPiece>();
		/*whitePlayer.teamPieces = [
			new BasicBishop(true)
		];
		blackPlayer.teamPieces = [
			new BasicBishop(false)
		];*/

		foreach (ChessPiece piece in this.whitePlayer.teamPieces)
		{
			piece.SetActive(true);
			piece.ownerPlayer = this.whitePlayer;
			piece.board = board;
			piece.transform.origin = Vector2.Zero;
		}

		foreach (ChessPiece piece in blackPlayer.teamPieces)
		{
			piece.SetActive(true);
			piece.ownerPlayer = blackPlayer;
			piece.board = board;
			piece.transform.origin = Vector2.Zero;
			piece.transform.parentSpacePos = Vector2.Zero;
		}
		
		TurnManager.instance.Board = board;
		TurnManager.instance.SetPlayers(blackPlayer, whitePlayer);

		Button endTurnButton = new Button("End Turn Button", "End Turn");
		((NineSliced)endTurnButton.spriteRenderer).cornerScale = 0.2f;
		
		endTurnButton.AddListener(() =>
		{
			if (GamePhaseManager.instance.phase == GamePhase.Gameplay)
				TurnManager.instance.ChangeTurn();
		});
		
		AddGameObjects([board, this.whitePlayer, blackPlayer, TurnManager.instance, endTurnButton,
			TriggerManager.instance, 
			GamePhaseManager.instance,
			MatchManager.instance]);
	}

	public override void Initialize()
	{
		foreach (var chessPiece in whitePlayer.teamPieces.Concat(blackPlayer.teamPieces))
		{
			chessPiece.InitializeBehaviors();
		}
	}
}