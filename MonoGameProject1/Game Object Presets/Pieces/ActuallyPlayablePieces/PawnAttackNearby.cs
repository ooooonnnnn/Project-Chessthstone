using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class PawnAttackNearby : Pawn
{
    public PawnAttackNearby(bool isWhite) : base(isWhite, 35, 9)
    {
        string color = this.isWhite ? "White" : "Black";
        name = $"{color} Radiating Pawn";
        AddBehaviors([new AttackNearbyAbility()]);
    }
}