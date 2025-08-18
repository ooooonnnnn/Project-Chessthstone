using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class KingGainManaFromAdj : King
{
	public KingGainManaFromAdj(bool isWhite) : base(isWhite, 40, 5)
	{
		string color = this.isWhite ? "White" : "Black";
		name = $"{color} Mana Hoarder King";
		AddBehaviors([new GainManaFromAdjAbility()]);
	}
}