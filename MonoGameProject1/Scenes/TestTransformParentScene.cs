using System;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1.Scenes;

public class TestTransformParentScene : Scene
{
	public TestTransformParentScene()
	{
		Transform transform = new Transform();
		GameObject object1 = new GameObject("Test", [transform]);
		Sprite object2 = new Sprite("Test", TextureManager.GetLogoTexture());
		
		transform.AddChild(object2);
		Console.WriteLine($"source size {object2.spriteRenderer.sourceWidth}, {object2.spriteRenderer.sourceHeight}");
		Console.WriteLine($"Pixel size {object2.spriteRenderer.sizePx}");
		Console.WriteLine("Settings size to 100x100");
		object2.spriteRenderer.sizePx = new Point(100, 100);
		Console.WriteLine("Settings size to 100x100");
		object2.spriteRenderer.sizePx = new Point(100, 100);
		Console.WriteLine($"Pixel size {object2.spriteRenderer.sizePx}");
		Console.WriteLine("changing parent scale");
		transform.SetScaleFromFloat(4f);
		Console.WriteLine("Settings size to 100x100");
		object2.spriteRenderer.sizePx = new Point(100, 100);
		Console.WriteLine($"Pixel size {object2.spriteRenderer.sizePx}");

		AddGameObjects([object1]);
	}
}