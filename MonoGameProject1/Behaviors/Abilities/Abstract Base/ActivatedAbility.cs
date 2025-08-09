
namespace MonoGameProject1.Behaviors;

/// <summary>
/// An ability that can be activated by the player at any time by paying a mana cost
/// </summary>
public abstract class ActivatedAbility : InstantaneousAbility
{
	public int manaCost { get; init; }

	/// <summary>
	/// Activates the ability
	/// </summary>
	public void Activate(GameObjectReferences objects)
	{
		OneShotEffect(objects);
	} 
	
	public override string ToString()
	{
		throw new System.NotImplementedException();
	}
}