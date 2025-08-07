using Microsoft.Xna.Framework;

namespace MonoGameProject1.Scenes;

public class TestTransformParentScene : Scene
{
	public TestTransformParentScene()
	{
		Sprite character = new Sprite("Test", TextureManager.GetLogoTexture());
		character.AddBehaviors([new WasdMove()]);
		Button child = new Button("nothing", "TEXT");
		(child.spriteRenderer as NineSliced).cornerScale = 0.5f; 
		
		character.transform.AddChild(child);
		child.transform.parentSpacePos = Vector2.UnitX * 100;
		character.transform.SetScaleFromFloat(1f);
		
		gameObjects = [character];
	}
}