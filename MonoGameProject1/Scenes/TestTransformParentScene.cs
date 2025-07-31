using Microsoft.Xna.Framework;

namespace MonoGameProject1.Scenes;

public class TestTransformParentScene : Scene
{
	public TestTransformParentScene()
	{
		Sprite character = new Sprite("Test", TextureManager.GetLogoTexture());
		character.AddBehaviors([new WasdMove()]);
		Sprite child = new Sprite("Test", TextureManager.GetDefaultButtonTexture());
		child.AddBehaviors([new WasdMove()]);
		
		
		character.transform.AddChild(child);
		
		gameObjects = [character, child];
	}
}