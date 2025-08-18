namespace MonoGameProject1.Behaviors;

/// <summary>
/// 2 Mana, this gains 1 action point
/// </summary>
public class RegainApAbility : ActivatedAbility
{
	public RegainApAbility() => manaCost = 2;
	
	protected override void OneShotEffect()
	{
		ownerPiece.actionPoints++;
	}

	public override string ToString()
	{
		return $"{manaCost} Mana: {ownerPiece.name} gains 1 Action Point";
	}
}