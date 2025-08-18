using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class BishopRegainAp : Bishop
{
	public BishopRegainAp(bool isWhite) : base(isWhite, 20, 6)
	{
		string color = this.isWhite ? "White" : "Black";
		name = $"{color} Agile Bishop";
		AddBehaviors([new RegainApAbility()]);
	}
}