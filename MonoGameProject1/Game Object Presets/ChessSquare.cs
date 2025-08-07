using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameProject1;

public class ChessSquare : ClickableSprite
{
	public ChessBoard board;
	public ChessPiece occupyingPiece;
	
	public ChessSquare(string name, bool isWhite) : base(name, TextureManager.GetChessSquareTexture(isWhite))
	{
		AddListener(() => board.HandleSquareClicked(this));
	}

	public void SetPiece(ChessPiece piece)
	{
		occupyingPiece = piece;
		piece.GoToSquare(this);
	}
}