using System;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Behaviors;

public class PlayerEventReaction : Behavior
{
	public override void Initialize()
	{
		Player player = gameObject as Player;
		if (player == null)
			throw new Exception("PlayerEventReaction must be on a player");

		player.OnTeamPieceChosen += piece => piece.spriteRenderer.color = Color.Yellow;
		player.OnTeamPieceChosen += _ =>
		{
			foreach (var square in player.board.squares)
			{
				bool onMySide = player.isWhite
					? square.column >= ChessProperties.boardSize / 2
					: square.column < ChessProperties.boardSize / 2;
				if (onMySide)
					square.spriteRenderer.color = Color.Yellow;
			}
		};

		player.OnPiecePlaced += piece => piece.spriteRenderer.color = Color.White;
		player.OnPiecePlaced += _ => player.DeselectAll();
	}
}