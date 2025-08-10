using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class BasicKing : King
{
	public BasicKing(ChessBoard board, Player owner) : base(board, owner, 40, 5)
	{
		AddBehaviors([new BasicKingAbility()]);
	}
}