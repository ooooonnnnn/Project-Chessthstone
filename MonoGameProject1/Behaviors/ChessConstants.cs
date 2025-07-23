using Microsoft.Xna.Framework;

namespace MonoGameProject1;


/// <summary>
/// Static class containing chess-related constants and helper methods
/// </summary>
public static class ChessConstants
{
    public const int BOARD_SIZE = 8;
    
    public static readonly Color LightSquareColor = Color.NavajoWhite;
    public static readonly Color DarkSquareColor = Color.SaddleBrown;
    
    public static readonly Color SelectedHighlight = Color.Yellow;
    public static readonly Color ValidMoveHighlight = Color.LightGreen;
    public static readonly Color CaptureHighlight = Color.Red;
    public static readonly Color CheckHighlight = Color.OrangeRed;
    
    public static bool IsLightSquare(int row, int col) => (row + col) % 2 == 0;
    
    public static string ToAlgebraicNotation(int row, int col)
    {
        char file = (char)('a' + col);
        int rank = 8 - row; // Chess ranks are numbered 1-8 from bottom to top
        return $"{file}{rank}";
    }
}
