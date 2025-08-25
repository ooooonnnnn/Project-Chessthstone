using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

public class Queen(bool isWhite, int baseHealth, int baseDamage, bool isSpecial = false)
    : ChessPiece(isWhite, PieceType.Queen, baseHealth, baseDamage, isSpecial)
{
    public override List<Point> GetMoveCoordList()
    {
        List<Point> moves = new();
        bool dir1 = true, dir2 = true, dir3 = true, dir4 = true;
        bool dir5 = true, dir6 = true, dir7 = true, dir8 = true;

        for (int i = 1; i < ChessProperties.boardSize && (dir1 || dir2 || dir3 || dir4 || dir5 || dir6 || dir7 || dir8); i++)
        {
            Point nextCoord = new Point(column + i, row + i);
            if (ValidateMove(nextCoord, ref dir1)) moves.Add(nextCoord);

            nextCoord = new Point(column - i, row + i);
            if (ValidateMove(nextCoord, ref dir2)) moves.Add(nextCoord);

            nextCoord = new Point(column + i, row - i);
            if (ValidateMove(nextCoord, ref dir3)) moves.Add(nextCoord);

            nextCoord = new Point(column - i, row - i);
            if (ValidateMove(nextCoord, ref dir4)) moves.Add(nextCoord);

            nextCoord = new Point(column + i, row);
            if (ValidateMove(nextCoord, ref dir5)) moves.Add(nextCoord);

            nextCoord = new Point(column - i, row);
            if (ValidateMove(nextCoord, ref dir6)) moves.Add(nextCoord);

            nextCoord = new Point(column, row + i);
            if (ValidateMove(nextCoord, ref dir7)) moves.Add(nextCoord);

            nextCoord = new Point(column, row - i);
            if (ValidateMove(nextCoord, ref dir8)) moves.Add(nextCoord);
        }

        return moves;
    }

    public override List<Point> GetAttackCoordList()
    {
        List<Point> attacks = new();
        bool dir1 = true, dir2 = true, dir3 = true, dir4 = true;
        bool dir5 = true, dir6 = true, dir7 = true, dir8 = true;

        for (int i = 1; i < ChessProperties.boardSize && (dir1 || dir2 || dir3 || dir4 || dir5 || dir6 || dir7 || dir8); i++)
        {
            Point nextCoord = new Point(column + i, row + i);
            if (ValidateAttackCoord(nextCoord, ref dir1)) attacks.Add(nextCoord);

            nextCoord = new Point(column - i, row + i);
            if (ValidateAttackCoord(nextCoord, ref dir2)) attacks.Add(nextCoord);

            nextCoord = new Point(column + i, row - i);
            if (ValidateAttackCoord(nextCoord, ref dir3)) attacks.Add(nextCoord);

            nextCoord = new Point(column - i, row - i);
            if (ValidateAttackCoord(nextCoord, ref dir4)) attacks.Add(nextCoord);

            nextCoord = new Point(column + i, row);
            if (ValidateAttackCoord(nextCoord, ref dir5)) attacks.Add(nextCoord);

            nextCoord = new Point(column - i, row);
            if (ValidateAttackCoord(nextCoord, ref dir6)) attacks.Add(nextCoord);

            nextCoord = new Point(column, row + i);
            if (ValidateAttackCoord(nextCoord, ref dir7)) attacks.Add(nextCoord);

            nextCoord = new Point(column, row - i);
            if (ValidateAttackCoord(nextCoord, ref dir8)) attacks.Add(nextCoord);
        }

        return attacks;
    }
}