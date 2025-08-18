using System;

namespace MonoGameProject1.Scenes;

public class TestCharacterScene : Scene
{
	public TestCharacterScene()
	{
		GameObject character = new Sprite("Test", TextureManager.GetLogoTexture());
		character.AddBehaviors([new WasdMove()]);
		AddGameObjects([character]);
	}
	
	
	public override void Initialize()
	{
		// Console.WriteLine($"{this} isn't initializing anything");
	}
}