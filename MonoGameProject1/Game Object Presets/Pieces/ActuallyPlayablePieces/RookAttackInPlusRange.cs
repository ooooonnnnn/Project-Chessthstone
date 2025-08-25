using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class RookAttackInPlusRange : Rook
{
    public RookAttackInPlusRange(bool isWhite) : base(isWhite, 20, 7, true)
    {
        string color = this.isWhite ? "White" : "Black";
        name = $"{color} Mana-burst Rook";
        AddBehaviors([new AttackInPlusRangeAbility()]);
    }
}