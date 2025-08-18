namespace MonoGameProject1;

public class KnightBasic : Knight
{
    public KnightBasic(bool isWhite) : base(isWhite, 25, 15)
    {
        string color = this.isWhite ? "White" : "Black";
        name = $"{color} Common Knight";
    }
}