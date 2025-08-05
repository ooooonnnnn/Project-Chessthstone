using MonoGameProject1.Behaviors;

namespace MonoGameProject1.Scenes;

public class TestStartScene : Scene
{
	public TestStartScene()
	{
		GameObject startable = new GameObject("Startable", [new TestStartBehavior()]);
		gameObjects = [startable];
	}
}