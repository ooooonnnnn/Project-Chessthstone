using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class BasicKnight : Knight
{
	public BasicKnight(bool isWhite) : base(isWhite, 20, 10)
	{
		string color = this.isWhite ? "White" : "Black";
		name = $"Basic {color} Knight";
		AddBehaviors([new BasicKnightAbility()]);
	}
}