using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameProject1.Behaviors;
using MonoGameProject1.Content;

namespace MonoGameProject1;

/// <summary>
/// Changes the position with wasd. Requires Transform
/// </summary>
public class WasdMove : Behavior, IUpdatable
{
	public float speed = 100;
	private Transform _transform;
	
	public override void Initialize()
	{
		_transform = gameObject.TryGetBehavior<Transform>();
	}

	public void Update(GameTime gameTime)
	{
		//moves the transform using the keyboard input
		KeyboardState keyboardState = Keyboard.GetState();
		float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
		float moveAmount = speed * deltaTime;
		if (keyboardState.IsKeyDown(Keys.W))
		{
			_transform.parentSpacePos += -moveAmount * Vector2.UnitY;
		}
		if (keyboardState.IsKeyDown(Keys.A))
		{
			_transform.parentSpacePos += -moveAmount * Vector2.UnitX;
		}
		if (keyboardState.IsKeyDown(Keys.S))
		{
			_transform.parentSpacePos += moveAmount * Vector2.UnitY;
		}
		if (keyboardState.IsKeyDown(Keys.D))
		{
			_transform.parentSpacePos += moveAmount * Vector2.UnitX;
		}
	}
}