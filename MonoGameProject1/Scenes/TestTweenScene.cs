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
		await Tween.Move(button1.transform, new Vector2(500,700), .25f, TweenType.Cubic);
		await Tween.Move(button1.transform, Vector2.Zero, .25f, TweenType.ReverseCubic);
		Console.WriteLine(DateTime.Now.Millisecond);
		button1.transform.parentSpacePos = Vector2.Zero;
	}

	public override void Initialize()
	{
	}
	
	
}