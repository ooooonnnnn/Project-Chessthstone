using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class QueenGainManaOnAttack : Queen
{
    public QueenGainManaOnAttack(bool isWhite) : base(isWhite, 30, 3, true)
    {
        string color = this.isWhite ? "White" : "Black";
        name = $"{color} Mana-drain Queen";

        OnAttack += () => ownerPlayer.mana += 2;
        AddBehaviors([new GainManaOnAttackAbility()]);
    }
}