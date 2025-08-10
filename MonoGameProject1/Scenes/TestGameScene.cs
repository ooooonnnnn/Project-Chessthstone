using System;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Scenes;

public class TestGameScene : Scene
{
	public TestGameScene()
	{
		ChessBoard board = new ChessBoard("");
		board.transform.SetScaleFromFloat(0.2f);
		board.transform.origin = Vector2.One * board.totalWidth * 0.5f;
		board.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();

		TriggerManager.Instantiate("TriggerManager");
		
		Player whitePlayer = new Player("White", true, board);
		Player blackPlayer = new Player("Black", false, board);

		TurnManager.Instantiate("TurnManager", board, blackPlayer, whitePlayer);

		Button endTurnButton = new Button("End Turn Button", "End Turn");
		((NineSliced)endTurnButton.spriteRenderer).cornerScale = 0.2f;
		
		endTurnButton.AddListener(TurnManager.instance.ChangeTurn);
		
		AddGameObjects([board, whitePlayer, blackPlayer, TurnManager.instance, endTurnButton, TriggerManager.instance]);
	}
}