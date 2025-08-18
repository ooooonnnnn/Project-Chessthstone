using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;
using MonoGameProject1.Engine;

namespace MonoGameProject1.Scenes;

public class TestGameScene : Scene
{
	private Player whitePlayer;
	private Player blackPlayer;
	private List<ChessPiece> whiteTeam;
	private List<ChessPiece> blackTeam;
	private ChessBoard board;
	private Button endTurnButton;

	public TestGameScene(IEnumerable<ChessPiece> whiteTeam = null, IEnumerable<ChessPiece> blackTeam = null)
	{
		Console.WriteLine($"Constructing game scene with {whiteTeam.Count()} white pieces.");
		
		TriggerManager.Instantiate("TriggerManager");
		GamePhaseManager.Instantiate("GamePhaseManager");
		TurnManager.Instantiate("TurnManager");
		MatchManager.Instantiate("MatchManager");
		PieceOverlay pieceOverlay;
		board = new ChessBoard("");
		board.transform.SetScaleFromFloat(0.4f);
		board.transform.origin = Vector2.One * board.totalWidth * 0.5f;
		board.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
		
		whitePlayer = new Player("White", true){board = board};
		blackPlayer = new Player("Black", false){board = board};

		this.whiteTeam = whiteTeam.ToList();
		this.blackTeam = blackTeam.ToList();

		endTurnButton = new Button("End Turn Button", "End Turn");
		((NineSliced)endTurnButton.spriteRenderer).cornerScale = 0.2f;
		
		AddGameObjects([board, whitePlayer, blackPlayer, TurnManager.instance, endTurnButton,
			TriggerManager.instance, 
			GamePhaseManager.instance,
			MatchManager.instance]);
	}

	public override void Initialize()
	{
		Console.WriteLine("Initializing Game Scene");
		
		whitePlayer.teamPieces = whiteTeam?.ToList() ?? new List<ChessPiece>();
		blackPlayer.teamPieces = blackTeam?.ToList() ?? new List<ChessPiece>();
		// whitePlayer.teamPieces = [new Pawn(true, 1, 1)];
		// blackPlayer.teamPieces = [new Pawn(false, 1, 1)];
		
		TurnManager.instance.SetPlayers(whitePlayer, blackPlayer);
		TurnManager.instance.board = board;
		
		MatchManager.instance.board =  board;

		foreach (ChessPiece piece in whitePlayer.teamPieces)
		{
			piece.ownerPlayer = whitePlayer;
			Clickable clickable = new();
			SenseMouseHover hover = new();
			SpriteRectCollider collider = new();
			piece.AddBehaviors([clickable, hover, collider]);

			clickable.OnClick += () => piece.ownerPlayer.TryChooseTeamPiece(piece);
		}

		foreach (ChessPiece piece in blackPlayer.teamPieces)
		{
			piece.ownerPlayer = blackPlayer;
			
			Clickable clickable = new();
			SenseMouseHover hover = new();
			SpriteRectCollider collider = new();
			piece.AddBehaviors([clickable, hover, collider]);

			clickable.OnClick += () => piece.ownerPlayer.TryChooseTeamPiece(piece);
		}
		
		foreach (var piece in whitePlayer.teamPieces.Concat(blackPlayer.teamPieces))
		{
			piece.SetActive(true);
			piece.InitializeBehaviors();
			piece.board = board;
			piece.transform.origin = Vector2.Zero;
			PieceOverlay pieceOverlay = new PieceOverlay(
				TextureManager.GetHealthIcon(),
				TextureManager.GetDamageIcon(),
				TextureManager.GetActionPointsIcon(),
				FontManager.defaultFont);
			GameObject overlayObj = new GameObject(piece.name + " Overlay", [pieceOverlay, new Transform()]);
			pieceOverlay.SetChessPiece(piece);
			
			overlayObj.SetActive(false);
		}

		ArrangeTeamPieces();

		AddGameObjects(whitePlayer.teamPieces.Concat(blackPlayer.teamPieces).Cast<
		GameObject>().ToList());
		
		endTurnButton.AddListener(() =>
		{
			if (GamePhaseManager.instance.phase == GamePhase.Gameplay)
				TurnManager.instance.ChangeTurn();
		});
		
		GamePhaseManager.instance.OnPhaseChanged += (prev, phase) =>
		{
			if (phase is GamePhase.Gameplay or GamePhase.Setup &&
			    prev is not (GamePhase.Gameplay or GamePhase.Setup))
			{
				TurnManager.instance.StartGame();
			}
		};
		
		GamePhaseManager.instance.phase = GamePhase.Setup;
	}

	private void ArrangeTeamPieces()
	{
		Vector2 center = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
		float groupDist = 700;
		float upOffset = -200;
		float rightOffset = -125;
		float elementDist = 150;

		Vector2 groupTopLeft = center + Vector2.UnitX * (rightOffset + groupDist) + Vector2.UnitY * upOffset;
		for (int i = 0; i < whitePlayer.teamPieces.Count; i ++)
		{
			ChessPiece piece = whitePlayer.teamPieces[i];
			piece.transform.parentSpacePos = groupTopLeft 
			                                 + Vector2.UnitX * elementDist * (i % 2)
			                                 +  Vector2.UnitY * elementDist * (i / 2);
		}

		groupTopLeft = center + Vector2.UnitX * (rightOffset - groupDist) + Vector2.UnitY * upOffset;
		for (int i = 0; i < blackPlayer.teamPieces.Count; i ++)
		{
			ChessPiece piece = blackPlayer.teamPieces[i];
			piece.transform.parentSpacePos = groupTopLeft 
			                                 + Vector2.UnitX * elementDist * (i % 2)
			                                 +  Vector2.UnitY * elementDist * (i / 2);
		}
	}
}