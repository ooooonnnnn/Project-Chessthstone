using System;

namespace MonoGameProject1.Behaviors;

public class NotifyBoardOnSquareClicked : Behavior
{
	public override void Initialize()
	{
		ChessSquare square = gameObject as  ChessSquare;
		if (square == null)
			throw new Exception("NotifyBoardOnSquareClicked must be on a chess square");
		ChessBoard board =  square.board;
		if (board == null)
		{
			Console.WriteLine("Can't subscribe board to click event because it's not set");
			return;
		}
		square.AddListener(() => board.HandleSquareClicked(square));
	}
}