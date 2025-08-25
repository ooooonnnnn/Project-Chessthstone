namespace MonoGameProject1.Behaviors;

/// <summary>
/// An ability that activates in response to a trigger
/// </summary>
public abstract class TriggeredAbility : InstantaneousAbility
{
	/// <summary>
	/// Meant to be checked whenever a player does anything. Returns true if the condition is met. <br/>
	/// All conditions can be defined as a certain difference between the previous and current game states, for example: <br/>
	/// Previous state: blacks turn + current state: whites turn => whites turn began.
	/// </summary>
	/// <returns>True if condition is met</returns>
	protected abstract bool Trigger(GameState previousState, GameState currentState);

	/// <summary>
	/// Checks the trigger and activates the ability if it's true.
	/// </summary>
	/// <param name="objects">GameObjects to act on</param>
	public void CheckTriggerAndActivate(GameState previousState, GameState currentState)
	{
		if (Trigger(previousState, currentState))
		{
			OneShotEffect();
			AudioManager.PlaySound(AudioClips.AbilitySound);
		}
	}
}