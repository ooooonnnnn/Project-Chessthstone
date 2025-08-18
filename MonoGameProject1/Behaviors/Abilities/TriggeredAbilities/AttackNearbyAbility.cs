using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Behaviors;

public class AttackNearbyAbility : TriggeredAbility
{
    public override string ToString()
    {
        return $"When your turn starts, damage all adjacent enemy pieces by 3";
    }

    private GameState currentState;
    private ChessPiece ownerPiece => gameObject as ChessPiece;
	
    protected override void OneShotEffect()
    {
        //Take positions of ally pieces
        IReadOnlyList<Point> enemyCoords = ownerPiece.isWhite 
            ? currentState.blackPieces : currentState.whitePieces;
        Point myPosition = ownerPiece.position;
		
        foreach (Point coord in enemyCoords)
        {
            Point diff = coord - myPosition;
            if (diff == Point.Zero) continue;
            if (Math.Abs(diff.X) < 2 && Math.Abs(diff.Y) < 2)
            {
                var attackSquare = ownerPiece.board.squares[coord.X, coord.Y];
                ownerPiece.DealDamageToPiece(attackSquare.occupyingPiece, 3);
            }
        }
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