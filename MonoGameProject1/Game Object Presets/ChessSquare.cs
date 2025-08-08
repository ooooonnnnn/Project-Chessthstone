using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameProject1;

public class ChessSquare : ClickableSprite
{
	public ChessBoard board;
	public ChessPiece occupyingPiece;
	public int row { get; init; }
	public int column { get; init; }
	
	public ChessSquare(string name, int row, int column, bool isWhite) : base(name, TextureManager.GetChessSquareTexture(isWhite))
	{
		this.row = row;
		this.column = column;
		AddListener(() => board.HandleSquareClicked(this));
	}

	public void SetPiece(ChessPiece piece)
	{
		occupyingPiece = piece;
		piece.GoToSquare(this);
	}
}