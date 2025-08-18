namespace MonoGameProject1;

public class RookBasic : Rook
{
    public RookBasic(bool isWhite) : base(isWhite, 25, 10)
    {
        string color = this.isWhite ? "White" : "Black";
        name = $"{color} Common Rook";
    }
}