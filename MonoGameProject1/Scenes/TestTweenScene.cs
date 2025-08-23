using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Scenes;

public class TestTweenScene : Scene
{
	FloatController floatController = new();
	private Button button1;

	public TestTweenScene()
	{
		button1 = new Button("btn");
		button1.AddListener(() => NewMethod());
		
		AddGameObjects([button1]);
	}

	private async Task NewMethod()
	{
		Console.WriteLine(DateTime.Now.Millisecond);
		await Tween.Move(button1.transform, new Vector2(500,700), 1f, TweenType.CounterExponential);
		Console.WriteLine(DateTime.Now.Millisecond);
		button1.transform.parentSpacePos = new Vector2(0,0);
	}

	public override void Initialize()
	{
	}
	
	
}