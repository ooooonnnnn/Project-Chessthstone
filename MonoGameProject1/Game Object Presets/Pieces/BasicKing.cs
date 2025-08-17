using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class BasicKing : King
{
	public BasicKing(bool isWhite) : base(isWhite, 40, 5)
	{
		string color = this.isWhite ? "White" : "Black";
		name = $"Basic {color} King";
		AddBehaviors([new BasicKingAbility()]);
	}
}