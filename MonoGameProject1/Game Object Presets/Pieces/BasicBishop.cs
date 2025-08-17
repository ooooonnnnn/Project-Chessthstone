using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class BasicBishop : Bishop
{
	public BasicBishop(bool isWhite) : base(isWhite, 20, 10)
	{
		string color = this.isWhite ? "White" : "Black";
		name = $"Basic {color} Bishop";
		AddBehaviors([new BasicKnightAbility()]);
	}
}