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
		
		Player whitePlayer = new Player("White", true, board);
		Player blackPlayer = new Player("Black", false, board);

		TurnManager turnManager = new TurnManager("TurnManager", blackPlayer, whitePlayer);

		Button endTurnButton = new Button("End Turn Button", "End Turn");
		((NineSliced)endTurnButton.spriteRenderer).cornerScale = 0.2f;
		
		AddGameObjects([board, whitePlayer, blackPlayer, turnManager, endTurnButton]);
	}
}