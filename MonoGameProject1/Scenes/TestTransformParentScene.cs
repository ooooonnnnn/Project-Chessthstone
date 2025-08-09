using System;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1.Scenes;

public class TestTransformParentScene : Scene
{
	public TestTransformParentScene()
	{
		Transform transform = new Transform();
		GameObject object1 = new GameObject("Test", [transform, new WasdMove()]);
		Sprite object2 = new Sprite("Test", TextureManager.GetLogoTexture());
		object2.AddBehaviors([new FollowTransform(transform, new Vector2(100,200))]);
		

		AddGameObjects([object1, object2]);
	}
}