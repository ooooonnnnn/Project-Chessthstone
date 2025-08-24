using System;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1.Scenes;

public class WinScreenScene : Scene
{
	public WinScreenScene(bool whiteWon)
	{
		string color = whiteWon ? "White" : "Black";
		var winText = new TextBox("win text obj", $"{color} player won!");
		var textTrans = winText.transform;

		Button restart = new Button("restart", "Restart");
		Button quit = new Button("quit", "Quit");
		
		restart.AddListener(() => SceneManager.ChangeScene(new TeamSelectionScene()));
		quit.AddListener(GameManager.ExitGame);

		textTrans.origin = winText.textRenderer.Font.MeasureString(winText.text) * 0.5f;
		restart.transform.origin = restart.spriteRenderer.sizePx.ToVector2() * 0.5f;
		quit.transform.origin = quit.spriteRenderer.sizePx.ToVector2() * 0.5f;

		textTrans.SetScaleFromFloat(3f);
		restart.transform.parentSpaceScale = new Vector2(3,1);
		restart.textChildTransform.parentSpaceScale = new Vector2(0.33f,1) * 2;
		quit.transform.parentSpaceScale = new Vector2(3,1);
		quit.textChildTransform.parentSpaceScale = new Vector2(0.33f,1) * 2;
		
		textTrans.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2() - 
		                           Vector2.UnitY * 200;
		restart.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
		quit.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2() + 
		                                Vector2.UnitY * 200;
		
		
		AddGameObjects([winText, restart, quit]);
	}
	
	public override void Initialize()
	{
		// Console.WriteLine($"{this} isn't initializing anything");
	}
}