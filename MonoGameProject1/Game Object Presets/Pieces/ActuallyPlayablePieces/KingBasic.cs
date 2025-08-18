using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class KingBasic : King
{
    public KingBasic(bool isWhite) : base(isWhite, 50, 20)
    {
        string color = this.isWhite ? "White" : "Black";
        name = $"{color} Common King";
    }
}