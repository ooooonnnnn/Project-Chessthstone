using System;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Scenes;

public class TestBoardScene : Scene
{
	public TestBoardScene()
	{
		ChessBoard board = new ChessBoard("");
		board.transform.SetScaleFromFloat(0.4f);
		board.transform.origin = Vector2.One * board.totalWidth * 0.5f;
		board.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
		
		board.squares[0,0].AddListener(() =>
		{
			Console.WriteLine("Closing Scene");
			SceneManager.RemoveScene(this);
		});
		
		AddGameObjects([board]);
	}
}