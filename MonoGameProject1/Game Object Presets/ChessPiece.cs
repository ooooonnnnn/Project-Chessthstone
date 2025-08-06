namespace MonoGameProject1;

/// <summary>
/// Base class for chess pieces
/// </summary>
// public abstract class ChessPiece : Sprite
// {
// 	public PieceType type { get; init; }
// 	public bool isWhite { get; init; }
// 	private int row, column;
//
// 	public ChessPiece(bool isWhite, PieceType type) : 
// 		base(MakeName(isWhite, type), TextureManager.GetChessPieceTexture(isWhite, type))
// 	{
// 		this.type = type;
// 		this.isWhite = isWhite;
// 	}
//
// 	private static string MakeName(bool isWhite, PieceType type)
// 	{
// 		string color = isWhite ? "White" : "Black";
// 		return $"{color} {type}";
// 	}
// }