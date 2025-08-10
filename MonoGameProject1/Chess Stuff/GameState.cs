using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

/// <summary>
/// Records a state of the game.
/// </summary>
public record GameState(bool isWhiteTurn, List<Point> blackPieces, List<Point> whitePieces);