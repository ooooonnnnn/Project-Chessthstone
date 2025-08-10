using System.Linq;
using MonoGameProject1.Behaviors;

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
	public void UpdateStateAndTrigger(bool isWhiteTurn)
	{
		_prevState = _currentState;
		_currentState = new GameState(isWhiteTurn, parentScene);
		parentScene.gameObjects.ForEach(obj =>
		{
			if (obj is not ChessPiece chessPiece)
				return;
			Ability ability = chessPiece.ability;
			(ability as TriggeredAbility)?.CheckTriggerAndActivate(_prevState, _currentState);
			(ability as StaticAbility)?.TryApplyEffect(_currentState);
			return;
		});
	}
}