using System;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1.Scenes;

public class WinScreenScene : Scene
{
	public WinScreenScene(bool whiteWon)
	{
		Transform textTrans = new Transform();
		string color = whiteWon ? "White" : "Black";
		TextRenderer text = new TextRenderer("win text") { Text = $"{color} player won!" };
		GameObject txtObj = new GameObject("win text obj", [textTrans, text]);

		Button restart = new Button("restart", "Restart");
		Button quit = new Button("quit", "Quit");
		
		restart.AddListener(() => SceneManager.ChangeScene(new TeamSelectionScene()));
		quit.AddListener(GameManager.ExitGame);

		textTrans.origin = text.Font.MeasureString(text.Text) * 0.5f;
		restart.transform.origin = restart.spriteRenderer.sizePx.ToVector2() * 0.5f;
		quit.transform.origin = quit.spriteRenderer.sizePx.ToVector2() * 0.5f;

		restart.transform.parentSpaceScale = new Vector2(3,1);
		quit.transform.parentSpaceScale = new Vector2(3,1);
		
		textTrans.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2() - 
		                           Vector2.UnitY * 100;
		restart.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
		quit.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2() + 
		                                Vector2.UnitY * 100;
		
		
		AddGameObjects([txtObj, restart, quit]);
	}
	
	public override void Initialize()
	{
		Console.WriteLine($"{this} isn't initializing anything");
	}
}