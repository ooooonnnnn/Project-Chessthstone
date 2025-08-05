using System;
using MonoGameProject1.Engine;

namespace MonoGameProject1.Behaviors;

public class TestStartBehavior : Behavior, IStart
{
	public override void Initialize()
	{
		
	}

	public void Start()
	{
		Console.WriteLine("I'm STARTINGGGG!! omfggggg... Imabouttostaaaart");
	}
}