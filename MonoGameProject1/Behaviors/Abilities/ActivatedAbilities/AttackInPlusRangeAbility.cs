using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Behaviors;

public class AttackInPlusRangeAbility : ActivatedAbility
{
    public AttackInPlusRangeAbility() => manaCost = 5;
    
    public override string ToString()
    {
        return $"{manaCost} mana: Deal 15 damage to all pieces horizontal and vertical to this piece";
    }

    private ChessPiece ownerPiece => gameObject as ChessPiece;
    
    protected override void OneShotEffect()
    {
        
        //Take positions of ally pieces
        GameState currentState = new GameState(TurnManager.instance.isWhiteTurn, gameObject.parentScene);
        IReadOnlyList<Point> enemyCoords = ownerPiece.isWhite 
            ? currentState.blackPieces : currentState.whitePieces;
        Point myPosition = ownerPiece.position;
		
        foreach (Point coord in enemyCoords)
        {
            Point diff = coord - myPosition;
            if (diff == Point.Zero) continue;
            if (Math.Abs(diff.X) == 0 || Math.Abs(diff.Y) == 0)
            {
                var attackSquare = ownerPiece.board.squares[coord.X, coord.Y];
                ownerPiece.DealDamageToPiece(attackSquare.occupyingPiece, 15);
            }
        }
    }
}