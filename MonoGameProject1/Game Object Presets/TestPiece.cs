using System;

namespace MonoGameProject1;

public class TestPiece() : ChessPiece(QuickRandom.NextInt(0,2) == 1, (PieceType)QuickRandom.NextInt(0, 6));