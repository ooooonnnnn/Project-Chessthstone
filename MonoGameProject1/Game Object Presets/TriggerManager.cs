using System.Collections.Generic;

namespace MonoGameProject1;

/// <summary>
/// Prompts the pieces to check their triggers and conditions for their abilities
/// </summary>
public class TriggerManager(string name) : GameObject(name)
{
	/// <summary>
	/// Snapshots of the game state
	/// </summary>
	private GameState _prevState, _currentState;

	/// <summary>
	/// Updates the game state history and prompts all pieces to try activating
	/// </summary>
	public void UpdateStateAndTrigger(bool isWhiteTurn, Scene gameScene)
	{
		_prevState = _currentState;
		_currentState = new GameState(isWhiteTurn, gameScene);
	}
}