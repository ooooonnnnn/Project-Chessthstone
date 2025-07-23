using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGameProject1;

public static class MouseInput 
{
	//handles mouse click events
	private static MouseState _prevState;
	private static MouseState _currState;
	public static event Action OnLeftClick;

	public static void Update(GameTime gameTime)
	{
		_prevState = _currState; // FIX: This was missing!
		_currState = Mouse.GetState();
		if (_currState.LeftButton == ButtonState.Pressed && _prevState.LeftButton == ButtonState.Released)
		{
			OnLeftClick?.Invoke();
		}
	}
}