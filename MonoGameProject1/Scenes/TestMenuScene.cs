using System;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;


namespace MonoGameProject1.Scenes;

public class TestMenuScene : Scene
{
	public TestMenuScene()
	{
		Button exitButton = new Button("exitButton", "Quit Game");
		Button settingsButton = new Button("SettingsButton", "Settings");
		Button startButton = new Button("startButton", "Start");
		
		exitButton.AddListener(GameManager.ExitGame);
		settingsButton.AddListener(() => Console.WriteLine("Settings."));
		startButton.AddListener(() => SceneManager.ChangeScene(new TestCharacterScene()));
		
		exitButton.transform.origin = new Vector2(exitButton.spriteRenderer.width * 0.5f,
			exitButton.spriteRenderer.height * 0.5f);
		settingsButton.transform.origin = new Vector2(settingsButton.spriteRenderer.width * 0.5f,
			settingsButton.spriteRenderer.height * 0.5f);
		startButton.transform.origin = new Vector2(startButton.spriteRenderer.width * 0.5f,
			startButton.spriteRenderer.height * 0.5f);
		
		
		Vector2 center = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
		exitButton.transform.parentSpacePos = center + new Vector2(0, 120);
		settingsButton.transform.parentSpacePos = center;
		startButton.transform.parentSpacePos = center + new Vector2(0, -120);
		
		exitButton.transform.parentSpaceScale = new Vector2(3f, 1f);
		settingsButton.transform.parentSpaceScale = new Vector2(3f, 1f);
		startButton.transform.parentSpaceScale = new Vector2(3f, 1f);
		
		(exitButton.spriteRenderer as NineSliced).cornerScale = .5f;
		(settingsButton.spriteRenderer as NineSliced).cornerScale = .5f;
		(startButton.spriteRenderer as NineSliced).cornerScale = .5f;
		
		AddGameObjects([exitButton, settingsButton, startButton, AddTextChild(exitButton.transform),
		AddTextChild(settingsButton.transform), AddTextChild(startButton.transform)]);
	}
	
	/// <summary>
	/// Adds a text child to the parent
	/// </summary>
	private GameObject AddTextChild(Transform parent)
	{
		Button button = parent.gameObject as Button;
		GameObject textChild = new GameObject( button.name + " Text");
		Transform textTransform = new Transform();
		TextRenderer textRenderer = new TextRenderer(button.text);
		textRenderer.color = Color.Black;

		textTransform.origin = textRenderer.Font.MeasureString(button.text) * 0.5f;
		textTransform.SetScaleFromFloat(1f);
		
		textChild.AddBehaviors([
			textRenderer,
			textTransform
		]);
		
		button.transform.AddChild(textChild);
		textTransform.parentSpacePos = new Vector2(
			button.spriteRenderer.width*0.5f, button.spriteRenderer.height*0.5f);
		
		return textChild;
	}
}