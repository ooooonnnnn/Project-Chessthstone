namespace MonoGameProject1.Behaviors;

/// <summary>
/// An abiltity that has a one-shot effect
/// </summary>
public abstract class InstantaneousAbility : Ability
{
	/// <summary>
	/// The effect of the ability. Affects the pieces in the same scene.
	/// </summary>
	/// <param name="objects"></param>
	//TODO: this will be a OneShotEffect class
	protected abstract void OneShotEffect(); 
}