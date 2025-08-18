namespace MonoGameProject1;

public class BishopBasic: Bishop
{
    public BishopBasic(bool isWhite) : base(isWhite, 30, 10)
    {
        string color = this.isWhite ? "White" : "Black";
        name = $"{color} Common Bishop";
    }
}