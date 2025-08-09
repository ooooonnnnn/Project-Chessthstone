using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameProject1;

public class ChessSquare : ClickableSprite
{
	public ChessBoard board;
	public ChessPiece occupyingPiece;
	public int row { get; init; }
	public int column { get; init; }
	
	public ChessSquare(string name, int column, int row, bool isWhite) : base(name, TextureManager.GetChessSquareTexture(isWhite))
	{
		this.row = row;
		this.column = column;
		AddListener(() => board.HandleSquareClicked(this));
	}
}