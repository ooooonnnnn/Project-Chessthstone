using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// When your turn starts, you gain 1 mana for each adjacent friendly piece
/// </summary>
public class BasicKingAbility : TriggeredAbility
{
	public override string ToString()
	{
		return $"When your turn starts, you gain 1 mana for each adjacent friendly piece";
	}

	private GameState currentState;
	private ChessPiece ownerPiece => gameObject as ChessPiece;
	
	protected override void OneShotEffect()
	{
		//Take positions of ally pieces
		IReadOnlyList<Point> allyCoords = ownerPiece.isWhite 
			? currentState.whitePieces : currentState.blackPieces;
		Point myPosition = ownerPiece.position;
		
		//Count adjacent
		int numAdjacent = 0;
		foreach (Point coord in allyCoords)
		{
			Point diff = coord - myPosition;
			if (diff == Point.Zero) continue;
			if (Math.Abs(diff.X) < 2 && Math.Abs(diff.Y) < 2)
				numAdjacent++;
		}
		
		//Add mana
		ownerPiece.ownerPlayer.mana += numAdjacent;
	}

	/// <summary>
	/// When your turn starts
	/// </summary>
	protected override bool Trigger(GameState previousState, GameState currentState)
	{
		this.currentState = currentState;
		return currentState.isWhiteTurn == ownerPiece.isWhite &&
		       previousState.isWhiteTurn != currentState.isWhiteTurn;
	}
}