using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Scenes;

public class TestGameScene : Scene
{
	public TestGameScene(List<ChessPiece> whiteTeam = null, List<ChessPiece> blackTeam = null)
	{
		ChessBoard board = new ChessBoard("");
		board.transform.SetScaleFromFloat(0.2f);
		board.transform.origin = Vector2.One * board.totalWidth * 0.5f;
		board.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();

		TriggerManager.Instantiate("TriggerManager");
		GamePhaseManager.Instantiate("GamePhaseManager");
		
		Player whitePlayer = new Player("White", true);
		Player blackPlayer = new Player("Black", false);
		
		whitePlayer.teamPieces = whiteTeam ?? new List<ChessPiece>();
		blackPlayer.teamPieces = blackTeam ?? new List<ChessPiece>();
		
		// whitePlayer.teamPieces = 
		// [
		// 	new BasicBishop(board, whitePlayer), new BasicKing(board, whitePlayer), new BasicKnight(board, whitePlayer)
		// ];
		// blackPlayer.teamPieces =
		// [
		// 	new BasicBishop(board, blackPlayer), new BasicKing(board, blackPlayer), new BasicKnight(board, blackPlayer)
		// ];

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