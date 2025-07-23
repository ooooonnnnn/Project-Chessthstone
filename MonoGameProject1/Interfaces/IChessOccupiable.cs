namespace MonoGameProject1;

/// <summary>
/// Interface for squares that can hold chess pieces
/// </summary>
public interface IChessOccupiable
{
    IChessPiece OccupyingPiece { get; }
    bool IsOccupied { get; }
    void PlacePiece(IChessPiece piece);
    IChessPiece RemovePiece(); 
}