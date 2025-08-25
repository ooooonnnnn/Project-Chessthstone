using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class KnightRegainAp : Knight
{
	public KnightRegainAp(bool isWhite) : base(isWhite, 20, 10, true)
	{
		string color = this.isWhite ? "White" : "Black";
		name = $"{color} Agile Knight";
		AddBehaviors([new RegainApAbility()]);
	}
}