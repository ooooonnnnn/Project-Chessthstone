namespace MonoGameProject1.Behaviors;

/// <summary>
/// An ability that has a continuous effect that may depend on a condition
/// </summary>
public abstract class StaticAbility : Ability
{
	/// <summary>
	/// Checks if the current game state matches the abilities' condition
	/// </summary>
	/// <returns>True if the condition is met</returns>
	protected abstract bool Condition(GameState currentState);
	
	/// <summary>
	/// The effect of the ability. Has to be a constant effect, for example: if a piece has baseDamage and damage, <br/>
	/// you can have the effect be "damage++" only if the game manager resets (damage = baseDamage) before the static ability <br/>
	/// does its effect. 
	/// </summary>
	/// <param name="objects">The objects to act on</param>
	protected abstract void Effect(GameObjectReferences objects);

	/// <summary>
	/// Applies the effect if the condition is met
	/// </summary>
	/// <param name="currentState">Test the condition on this</param>
	/// <param name="objects">Apply the effect to this</param>
	public void TryApplyEffect(GameState currentState, GameObjectReferences objects)
	{
		if(Condition(currentState)) Effect(objects);
	}
}