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
		whitePlayer.mana = 10;
		
		AddGameObjects([board, whitePlayer]);
	}
}