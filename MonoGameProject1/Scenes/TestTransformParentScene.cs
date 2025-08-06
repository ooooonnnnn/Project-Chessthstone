using Microsoft.Xna.Framework;

namespace MonoGameProject1.Scenes;

public class TestTransformParentScene : Scene
{
	public TestTransformParentScene()
	{
		Sprite character = new Sprite("Test", TextureManager.GetLogoTexture());
		character.AddBehaviors([new WasdMove()]);
		Button child = new Button("nothing", "TEXT");
		
		character.transform.AddChild(child);
		
		gameObjects = [character];
	}
}