namespace MonoGameProject1;

public class QueenBasic : Queen
{
    public QueenBasic(bool isWhite) : base(isWhite, 20, 6)
    {
        string color = this.isWhite ? "White" : "Black";
        name = $"{color} Common Queen";
    }
}