using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;
using MonoGameProject1.Engine;

namespace MonoGameProject1.Scenes;

public class TestGameScene : Scene
{
	public TestGameScene(IEnumerable<ChessPiece> whiteTeam = null, IEnumerable<ChessPiece> blackTeam = null)
	{
		ChessBoard board = new ChessBoard("");
		board.transform.SetScaleFromFloat(0.2f);
		board.transform.origin = Vector2.One * board.totalWidth * 0.5f;
		board.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();

		TriggerManager.Instantiate("TriggerManager");
		GamePhaseManager.Instantiate("GamePhaseManager");
		
		Player whitePlayer = new Player("White", true){board = board};
		Player blackPlayer = new Player("Black", false){board = board};
		
		whitePlayer.teamPieces = whiteTeam?.ToList() ?? new List<ChessPiece>();
		blackPlayer.teamPieces = blackTeam?.ToList() ?? new List<ChessPiece>();

		foreach (ChessPiece piece in whitePlayer.teamPieces)
		{
			piece.ownerPlayer = whitePlayer;
		}
		
		foreach (ChessPiece piece in blackPlayer.teamPieces)
		{
			piece.ownerPlayer = blackPlayer;
		}
		
		foreach (ChessPiece piece in whitePlayer.teamPieces.Concat(blackPlayer.teamPieces))
		{
			piece.SetActive(true);
			piece.board = board;
			piece.transform.origin = Vector2.Zero;
			
			//Add overlays to the pieces
			PieceOverlay pieceOverlay = new PieceOverlay(
				TextureManager.GetHealthIcon(),
				TextureManager.GetDamageIcon(),
				TextureManager.GetActionPointsIcon(),
				FontManager.defaultFont);
			new GameObject(piece.name + " Overlay", [pieceOverlay, new Transform()]);
			pieceOverlay.SetChessPiece(piece);
		}
		
		TurnManager.Instantiate("TurnManager", board, blackPlayer, whitePlayer);

		Button endTurnButton = new Button("End Turn Button", "End Turn");
		((NineSliced)endTurnButton.spriteRenderer).cornerScale = 0.2f;
		
		endTurnButton.AddListener(() =>
		{
			if (GamePhaseManager.instance.phase == GamePhase.Gameplay)
				TurnManager.instance.ChangeTurn();
		});
		
		AddGameObjects([board, whitePlayer, blackPlayer, TurnManager.instance, endTurnButton, TriggerManager.instance, 
		GamePhaseManager.instance]);
	}
}