using MonoGameProject1.Enums;

namespace MonoGameProject1;

/// <summary>
/// Interface for chess pieces
/// </summary>
public interface IChessPiece : IChessMovable
{ 
    ChessPieceColor Color { get; }
    ChessPieceType Type { get; }
    IChessPosition CurrentPosition { get; set; }
    bool HasMoved { get; }
    float Value { get; }
}