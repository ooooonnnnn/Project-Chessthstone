using Microsoft.Xna.Framework;
using MonoGameProject1.Enums;

namespace MonoGameProject1;

/// <summary>
/// Interface for objects that can be highlighted (squares, pieces, etc.)
/// </summary>
public interface IChessHighlightable
{
    bool IsHighlighted { get; }
    void SetHighlight(bool highlighted, Color? highlightColor = null);
    void SetHighlightType(ChessHighlightedType type);
}