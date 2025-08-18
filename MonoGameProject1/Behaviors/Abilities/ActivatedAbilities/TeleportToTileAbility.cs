namespace MonoGameProject1.Behaviors;

public class TeleportToTileAbility : ActivatedAbility
{
    public TeleportToTileAbility() => manaCost = 5;
    
    public override string ToString()
    {
        return $"{manaCost} mana: Teleport {ownerPiece.name} to a tile";
    }

    private GameState currentState;
    private ChessPiece ownerPiece => gameObject as ChessPiece;
    
    protected override void OneShotEffect()
    {
        
    }
}