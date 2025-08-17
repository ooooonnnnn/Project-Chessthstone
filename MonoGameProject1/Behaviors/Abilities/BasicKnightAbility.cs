namespace MonoGameProject1.Behaviors;

/// <summary>
/// 2 Mana, this gains 1 action point
/// </summary>
public class BasicKnightAbility : ActivatedAbility
{
	public BasicKnightAbility() => manaCost = 2;
	
	protected override void OneShotEffect()
	{
		ownerPiece.actionPoints++;
	}

	public override string ToString()
	{
		return $"{manaCost} Mana: {ownerPiece.name} gains 1 Action Point";
	}
}