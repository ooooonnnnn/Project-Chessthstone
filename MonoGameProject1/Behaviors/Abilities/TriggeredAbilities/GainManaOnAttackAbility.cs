namespace MonoGameProject1.Behaviors;

public class GainManaOnAttackAbility : TriggeredAbility
{
    public override string ToString()
    {
        return "Gain 2 Mana on Attack";
    }

    protected override void OneShotEffect()
    {
        
    }

    protected override bool Trigger(GameState previousState, GameState currentState)
    {
        return false;
    }
}