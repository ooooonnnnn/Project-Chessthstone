using System;
using System.Diagnostics;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1.Scenes;

/// <summary>
/// This scene tests the speed of inverting a float that equals 1, with two methods:<br/>
/// result = 1/x<br/>
/// result = x == 1 ? 1 : 1/x <br/>
/// When building, there was no significant speed difference between the methods, which means the first one
/// is better for the general case. 
/// </summary>
public class TestSpeedScene : Scene
{
	public TestSpeedScene()
	{
		GameObject tester = new GameObject("",
			[new Transform(), new TextRenderer(), new SpeedTester()]);
		
		AddGameObjects([tester]);
	}
}

public class SpeedTester : Behavior, IUpdatable
{
	private TextRenderer text;
	public override void Initialize()
	{
		text = gameObject.TryGetBehavior<TextRenderer>();
	}

	private float result;
	private int times;
	private int i0 = 0;
	private bool test = true;
	private int i { get => i0; set{i0 = value; if(i0%100_000_000 == 0) Console.WriteLine(i0);} }
	
	public void Update(GameTime gameTime)
	{
		if (test)
		{
			test = false;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			float x = 1f;
			times = 1_000_000_000;
			for (i = 0; i < times; i++)
			{
				result = 1 / x;
				result = 0;
			}

			double regular = stopwatch.Elapsed.TotalMilliseconds;
			stopwatch.Restart();

			for (i = 0; i < times; i++)
			{
				result = x == 1 ? 1 : 1 / x;
				result = 0;
			}

			double optimized = stopwatch.Elapsed.TotalMilliseconds;
			stopwatch.Stop();

			text.text = $"Regular: {regular}ms\nOptimized: {optimized}ms";
		}
		else if (Keyboard.GetState().IsKeyDown(Keys.Space))
		{
			test = true;
		}
	}
}