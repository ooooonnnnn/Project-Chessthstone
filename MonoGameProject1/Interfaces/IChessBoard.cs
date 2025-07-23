using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameProject1.Enums;
namespace MonoGameProject1;

/// <summary>
/// Interface for chess board with proper abstraction
/// </summary>
public interface IChessBoard
{
    ChessSquare GetSquare(int row, int col);
    ChessSquare GetSquare(Vector2 boardPosition);
    IEnumerable<ChessSquare> GetAllSquares();
    IEnumerable<IChessPiece> GetAllPieces();
    IEnumerable<IChessPiece> GetPieces(ChessPieceColor color);
    Vector2 ScreenToBoardCoordinates(Vector2 screenPosition);
    bool IsValidPosition(int row, int col);
}