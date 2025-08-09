using System.Collections.Generic;

namespace MonoGameProject1;

/// <summary>
/// References for all GameObjects in the game. <br/>
/// This is different from a subset of the scene, because it can contain GameObjects that aren't in the scene, which <br/>
/// can be used to simulate a game without relying on the Update method and user input. (Meant for AI)
/// </summary>
public record GameObjectReferences
{
	public Player whitePlayer { get; init; }
	public Player blackPlayer { get; init; }
	public ChessBoard board { get; init; }
	public List<ChessPiece> pieces { get; init; }
}