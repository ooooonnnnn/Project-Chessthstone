using System.Collections.Generic;

namespace MonoGameProject1;

/// <summary>
/// A player in the game.
/// </summary>
public class Player : GameObject
{
	/// <summary>
	/// Mana to pay for activated abilities
	/// </summary>
	public int mana { get; private set; }
	/// <summary>
	/// Color. White goes first
	/// </summary>
	public bool isWhite { get; init; }
	
	public Player(string name) : base(name)
	{
	}
}