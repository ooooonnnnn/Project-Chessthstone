using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;
using MonoGameProject1.Engine;
using MonoGameProject1.Extensions;

namespace MonoGameProject1.Scenes;

/// <summary>
/// A game scene for testing the PVE system. One real player and one AI player. players start with random teams and pieces
/// placed randomly.
/// </summary>
/// <param name="playerIsWhite"></param>
public class TestPVEScene : Scene
{
	private Player realPlayer;
	private Player aiPlayer;
	private PlayerStatsHUD whiteHud;
	private PlayerStatsHUD blackHud;
	private List<ChessPiece> whiteTeam;
	private List<ChessPiece> blackTeam;
	private ChessBoard board;
	private Button endTurnButton;
	private bool isEndTurnButtonAvailable = false;
    private bool isRealPlayerWhite;
	
	public TestPVEScene(bool isRealPlayerWhite)
    {
        this.isRealPlayerWhite = isRealPlayerWhite;
        
        TriggerManager.Instantiate("TriggerManager");
        GamePhaseManager.Instantiate("GamePhaseManager");
        TurnManager.Instantiate("TurnManager");
        MatchManager.Instantiate("MatchManager");
        
        board = new ChessBoard("");
        board.transform.SetScaleFromFloat(0.35f);
        board.transform.origin = Vector2.One * board.totalWidth * 0.5f;
        Vector2 center = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
        board.transform.parentSpacePos = center + new Vector2(0, 64);

        realPlayer = new Player("Real Player", isRealPlayerWhite) { board = board };
        aiPlayer = new Player("AI Player", !isRealPlayerWhite) { board = board };

        whiteTeam = ChessPieceFactory.GetRandomTeam(true).ToList();
        blackTeam = ChessPieceFactory.GetRandomTeam(false).ToList();
        
        whiteHud = new PlayerStatsHUD(isRealPlayerWhite ? realPlayer : aiPlayer);
        blackHud = new PlayerStatsHUD(isRealPlayerWhite ? aiPlayer : realPlayer);
        whiteHud.transform.parentSpacePos = center + new Vector2(600, -300);
        blackHud.transform.parentSpacePos = center + new Vector2(-770, -300);

        endTurnButton = new Button("End Turn Button", "", TextureManager.WhiteTurnButtonTextureClear);
        endTurnButton.ChangeBackgroundScale(new Vector2(0.18f, 0.18f));
        //endTurnButton.transform.origin = endTurnButton.spriteRenderer.sizePx.ToVector2() * 0.5f;
        endTurnButton.transform.parentSpacePos = new Vector2(
            GameManager.Graphics.Viewport.Width / 2f - 115,
            GameManager.Graphics.Viewport.Height / 2f - 128 - 400);
        endTurnButton.hoverTinting.tintWhenHover = Color.White;
        endTurnButton.hoverTinting.tintWhenMouseDown = Color.White;


        AddGameObjects([
            board,
            realPlayer,
            aiPlayer,
            whiteHud,
            blackHud,
            TurnManager.instance, endTurnButton,
            TriggerManager.instance,
            GamePhaseManager.instance,
            MatchManager.instance
        ]);
    }
	
	public override void Initialize()
    {
        realPlayer.teamPieces = isRealPlayerWhite ? whiteTeam : blackTeam;
        aiPlayer.teamPieces = isRealPlayerWhite ? blackTeam : whiteTeam;
        realPlayer.OnManaChanged += _ => (isRealPlayerWhite ? whiteHud : blackHud).UpdateText();
        aiPlayer.OnManaChanged += _ => (isRealPlayerWhite ? blackHud : whiteHud).UpdateText();

        TurnManager.instance.SetPlayers(isRealPlayerWhite ? realPlayer : aiPlayer, isRealPlayerWhite ? aiPlayer : realPlayer);
        TurnManager.instance.board = board;
        MatchManager.instance.board = board;

        #region Music
        
        AudioManager.PlaySong(AudioClips.SetupPhaseMusic);
        
        GamePhaseManager.instance.OnPhaseChanged += (_, phase) =>
        {
            if (phase is GamePhase.Gameplay)
                AudioManager.PlaySong(AudioClips.BattlePhaseMusic);
        };

        #endregion

        foreach (ChessPiece piece in realPlayer.teamPieces)
        {
            piece.ownerPlayer = realPlayer;
        }

        foreach (ChessPiece piece in aiPlayer.teamPieces)
        {
            piece.ownerPlayer = aiPlayer;
        }

        foreach (var piece in realPlayer.teamPieces.Concat(aiPlayer.teamPieces))
        {
            piece.SetActive(true);
            piece.InitializeBehaviors();
            piece.board = board;
            piece.transform.origin = Vector2.Zero;
            
            Clickable clickable = new();
            SenseMouseHover hover = new();
            SpriteRectCollider collider = new();
            piece.AddBehaviors([clickable, hover, collider]);

            clickable.OnClick += () => piece.ownerPlayer.TryChooseTeamPiece(piece);
            
            PieceOverlay pieceOverlay = new PieceOverlay(
                TextureManager.GetHealthIcon(),
                TextureManager.GetDamageIcon(),
                TextureManager.GetActionPointsIcon(),
                FontManager.defaultFont);
            GameObject overlayObj = new GameObject(piece.name + " Overlay", [pieceOverlay, new Transform()]);
            pieceOverlay.SetChessPiece(piece);
            
            ToolTip toolTip = new ToolTip(piece.name + " tooltip", 
                piece.ability?.ToString() ?? "No special ability");

            FollowTransform followTransform = new FollowTransform(piece.transform, new Vector2(100, 100));
            toolTip.AddBehaviors([followTransform]);

            hover.OnStartHover += () => toolTip.SetActive(true);
            hover.OnEndHover += () => toolTip.SetActive(false);
            
            piece.OnDeath += _ => RemoveGameObjectAndChildren(toolTip);
            AddGameObjects([overlayObj, toolTip]);
        }

        ArrangeTeamPieces();

        AddGameObjects(realPlayer.teamPieces.Concat(aiPlayer.teamPieces).
            Cast<GameObject>().ToList());

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

            if (phase is GamePhase.Setup)
            {
                endTurnButton.hoverTinting.tintWhenHover = Color.White;
                endTurnButton.hoverTinting.tintWhenMouseDown = Color.White;
                isEndTurnButtonAvailable = false;
            }
            else
            {
                isEndTurnButtonAvailable = true;
                endTurnButton.hoverTinting.tintWhenHover = Color.LightGray;
                endTurnButton.hoverTinting.tintWhenMouseDown = Color.DarkGray;
            }
        };

        TurnManager.instance.OnTurnChanged += isWhiteTurn =>
        {
            if (isEndTurnButtonAvailable)
                endTurnButton.spriteRenderer.texture = isWhiteTurn
                    ? TextureManager.WhiteTurnButtonTexture
                    : TextureManager.BlackTurnButtonTexture;
            else
                endTurnButton.spriteRenderer.texture = isWhiteTurn
                    ? TextureManager.WhiteTurnButtonTextureClear
                    : TextureManager.BlackTurnButtonTextureClear;
        };

        GamePhaseManager.instance.phase = GamePhase.Gameplay;
        
        whiteHud.UpdateText();
        blackHud.UpdateText();
    }

    /// <summary>
    /// Arranges the pieces on the first and last columns of the board
    /// </summary>
    private void ArrangeTeamPieces()
    {
        IEnumerable<ChessSquare> whiteSideSquares = board.squares.Flatten().
            Where(square => square.column >= ChessProperties.boardSize / 2);
        IEnumerable<ChessSquare> blackSideSquares = board.squares.Flatten().
            Where(square => square.column < ChessProperties.boardSize / 2);
        
        IEnumerable<ChessSquare> currentSquares = isRealPlayerWhite ? whiteSideSquares : blackSideSquares;
        while (realPlayer.teamPieces.Count > 0)
        {
            ChessPiece piece = realPlayer.teamPieces[0];
            realPlayer.TryPlacePiece(currentSquares.Where(square => !square.IsOccupied).Random(), piece);
        }
        
        currentSquares = !isRealPlayerWhite ? whiteSideSquares : blackSideSquares;
        while (aiPlayer.teamPieces.Count > 0)
        {
            ChessPiece piece = aiPlayer.teamPieces[0];
            aiPlayer.TryPlacePiece(currentSquares.Where(square => !square.IsOccupied).Random(), piece);
        }
    }
}