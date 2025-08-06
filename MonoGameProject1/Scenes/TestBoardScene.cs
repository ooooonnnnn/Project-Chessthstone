namespace MonoGameProject1.Scenes;

public class TestBoardScene : Scene
{
	public TestBoardScene()
	{
		ChessBoard board = new ChessBoard("");
		board.transform.SetScaleFromFloat(0.2f);
		
		gameObjects = [board];
	}
}