using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Scenes;

public class TestGameScene : Scene
{
	public TestGameScene(IEnumerable<ChessPiece> whiteTeam = null, IEnumerable<ChessPiece> blackTeam = null)
	{
		ChessBoard board = new ChessBoard("");
		board.transform.SetScaleFromFloat(0.4f);
		board.transform.origin = Vector2.One * board.totalWidth * 0.5f;
		board.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();

		TriggerManager.Instantiate("TriggerManager");
		GamePhaseManager.Instantiate("GamePhaseManager");
		
		Player whitePlayer = new Player("White", true){board = board};
		Player blackPlayer = new Player("Black", false){board = board};
		
		whitePlayer.teamPieces = whiteTeam?.ToList() ?? new List<ChessPiece>();
		blackPlayer.teamPieces = blackTeam?.ToList() ?? new List<ChessPiece>();

		foreach (ChessPiece piece in whitePlayer.teamPieces)
		{
			piece.SetActive(true);
			piece.ownerPlayer = whitePlayer;
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
		
		TurnManager.Instantiate("TurnManager", board, blackPlayer, whitePlayer);

		Button endTurnButton = new Button("End Turn Button", "End Turn");
		((NineSliced)endTurnButton.spriteRenderer).cornerScale = 0.2f;
		
		endTurnButton.AddListener(() =>
		{
			if (GamePhaseManager.instance.phase == GamePhase.Gameplay)
				TurnManager.instance.ChangeTurn();
		});
		
		AddGameObjects([board, whitePlayer, blackPlayer, TurnManager.instance, endTurnButton, TriggerManager.instance, 
		GamePhaseManager.instance]);
	}
}