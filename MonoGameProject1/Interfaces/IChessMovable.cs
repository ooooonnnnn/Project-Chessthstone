using System.Collections.Generic;

namespace MonoGameProject1;

/// <summary>
/// Interface for chess pieces that can move
/// </summary>
public interface IChessMovable
{
    bool CanMoveTo(IChessPosition position);
    IEnumerable<IChessPosition> GetValidMoves();
    bool MoveTo(IChessPosition position);  
}