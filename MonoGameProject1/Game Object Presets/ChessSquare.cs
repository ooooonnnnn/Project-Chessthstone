using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class ChessSquare : ClickableSprite
{
	public ChessBoard board;
	public ChessPiece occupyingPiece;
	public int row { get; init; }
	public int column { get; init; }
	
	public ChessSquare(string name, ChessBoard board, int column, int row, bool isWhite) : base(name, TextureManager.GetChessSquareTexture(isWhite))
	{
		this.board = board;
		this.row = row;
		this.column = column;
		spriteRenderer.layerDepth = LayerDepthManager.BackgroundObjectDepth;
		
		AddBehaviors([new NotifyBoardOnSquareClicked()]);
	}
}