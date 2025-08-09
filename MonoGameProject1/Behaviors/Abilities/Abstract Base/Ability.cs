namespace MonoGameProject1.Behaviors;

/// <summary>
/// Base class for all ChessPiece abilities
/// </summary>
public abstract class Ability : Behavior
{
	/// <summary>
	/// The piece this ability is on
	/// </summary>
	protected ChessPiece ownerPiece => gameObject as ChessPiece;
	
	public override void Initialize()
	{
	}

	public abstract override string ToString();
}