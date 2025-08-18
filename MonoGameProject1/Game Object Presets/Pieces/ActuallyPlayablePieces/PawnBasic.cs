namespace MonoGameProject1;

public class PawnBasic : Pawn
{
    public PawnBasic(bool isWhite) : base(isWhite, 35, 18)
    {
        string color = this.isWhite ? "White" : "Black";
        name = $"{color} Common Pawn";
    }
}