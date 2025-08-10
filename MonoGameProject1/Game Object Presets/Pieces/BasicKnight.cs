using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class BasicKnight : Knight
{
	public BasicKnight(ChessBoard board, Player owner) : base(board, owner, 20, 10)
	{
		AddBehaviors([new BasicKnightAbility()]);
	}
}