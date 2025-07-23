using Microsoft.Xna.Framework;

namespace MonoGameProject1;

/// <summary>
/// Interface for objects that have chess board coordinates
/// </summary>
public interface IChessPosition
{
    int Row { get; }
    int Column { get; }
    Vector2 BoardPosition { get; }
    string AlgebraicNotation { get; }
}