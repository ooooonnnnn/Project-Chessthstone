using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGameProject1;

/// <summary>
/// Helper class for setting up chess game using OOP principles
/// </summary>
public static class ChessGameSetup
{
    public static ChessBoard CreateChessBoard(Game1 game, Texture2D lightSquare, Texture2D darkSquare)
    {
        var board = new ChessBoard("MainChessBoard", lightSquare, darkSquare, 64);
        board.TryGetBehavior<Transform>().position = new Vector2(100, 100);
        
        return board;
    }

    public static void SetupInitialPieces(IChessBoard board, Dictionary<ChessPieceType, Texture2D> pieceTextures)
    {
        // Setup white pieces (using interface polymorphism)
        // Add more pieces as needed...
    }

    private static void PlacePiece(IChessBoard board, IChessPiece piece, int row, int col)
    {
        var square = board.GetSquare(row, col);
        square?.PlacePiece(piece); // Boxing piece as interface
    }
}