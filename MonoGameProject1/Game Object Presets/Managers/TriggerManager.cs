using System;
using System.Buffers.Text;
using System.Linq;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

/// <summary>
/// Prompts the pieces to check their triggers and conditions for their abilities
/// </summary>
public class TriggerManager : SingletonGameObject<TriggerManager>
{
	/// <summary>
	/// Snapshots of the game state
	/// </summary>
	private GameState _prevState, _currentState;

	public static bool Instantiate(string name)
	{
		if (instance != null) 
			return false;
		
		instance = new TriggerManager(name);
		return true;
	}
	
	protected TriggerManager(string name) : base(name) { }

	/// <summary>
	/// Updates the game state history. If the phase is gameplay: prompts all pieces to try activating
	/// </summary>
	public void UpdateStateAndTryTrigger(bool isWhiteTurn)
	{
		if (GamePhaseManager.instance.phase != GamePhase.Gameplay)
		{
			Console.WriteLine("Not updating state because phase is not gameplay");
			return;
		}
		
		UpdateGameState(isWhiteTurn);
		if (GamePhaseManager.instance.phase != GamePhase.Gameplay)
			return;
		
		parentScene.gameObjects.ForEach(obj =>
		{
			if (obj is not ChessPiece chessPiece)
				return;
			Ability ability = chessPiece.ability;
			(ability as TriggeredAbility)?.CheckTriggerAndActivate(_prevState, _currentState);
			(ability as StaticAbility)?.TryApplyEffect(_currentState);
		});
	}

	/// <summary>
	/// Only updates the state history without triggering the pieces
	/// </summary>
	private void UpdateGameState(bool isWhiteTurn)
	{
		if (parentScene?.gameObjects is null)
		{
			Console.WriteLine("Can't check state because parent scene is empty");
			return;
		}
		_prevState = _currentState;
		_currentState = new GameState(isWhiteTurn, parentScene);
	}
}