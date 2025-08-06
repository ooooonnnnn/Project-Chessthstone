using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameProject1;

public class ChessSquare : ClickableSprite
{
	public ChessSquare(string name, bool isWhite) : base(name, TextureManager.GetChessSquareTexture(isWhite))
	{
		
	}
}