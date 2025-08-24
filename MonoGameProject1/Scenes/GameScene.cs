using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;
using MonoGameProject1.Engine;

namespace MonoGameProject1.Scenes;

public class GameScene : Scene
{
    private Player whitePlayer;
    private Player blackPlayer;
    private PlayerStatsHUD whiteHud;
    private PlayerStatsHUD blackHud;
    private List<ChessPiece> whiteTeam;
    private List<ChessPiece> blackTeam;
    private ChessBoard board;
    private Button endTurnButton;
    private bool isEndTurnButtonAvailable = false;

    public GameScene(IEnumerable<ChessPiece> whiteTeam = null, IEnumerable<ChessPiece> blackTeam = null)
    {
        Console.WriteLine($"Constructing game scene with {whiteTeam.Count()} white pieces.");

        TriggerManager.Instantiate("TriggerManager");
        GamePhaseManager.Instantiate("GamePhaseManager");
        TurnManager.Instantiate("TurnManager");
        MatchManager.Instantiate("MatchManager");
        PieceOverlay pieceOverlay;
        board = new ChessBoard("");
        board.transform.SetScaleFromFloat(0.35f);
        board.transform.origin = Vector2.One * board.totalWidth * 0.5f;
        Vector2 center = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
        board.transform.parentSpacePos = center + new Vector2(0, 64);

        whitePlayer = new Player("White", true) { board = board };
        blackPlayer = new Player("Black", false) { board = board };

        this.whiteTeam = whiteTeam.ToList();
        this.blackTeam = blackTeam.ToList();
        
        whiteHud = new PlayerStatsHUD(whitePlayer);
        blackHud = new PlayerStatsHUD(blackPlayer);
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
            whitePlayer,
            blackPlayer,
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
        Console.WriteLine("Initializing Game Scene");

        whitePlayer.teamPieces = whiteTeam?.ToList() ?? new List<ChessPiece>();
        blackPlayer.teamPieces = blackTeam?.ToList() ?? new List<ChessPiece>();
        whitePlayer.OnManaChanged += _ => whiteHud.UpdateText();
        blackPlayer.OnManaChanged += _ => blackHud.UpdateText();

        TurnManager.instance.SetPlayers(whitePlayer, blackPlayer);
        TurnManager.instance.board = board;

        MatchManager.instance.board = board;

        foreach (ChessPiece piece in whitePlayer.teamPieces)
        {
            piece.ownerPlayer = whitePlayer;
        }

        foreach (ChessPiece piece in blackPlayer.teamPieces)
        {
            piece.ownerPlayer = blackPlayer;
        }

        foreach (var piece in whitePlayer.teamPieces.Concat(blackPlayer.teamPieces))
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
            overlayObj.SetActive(false);
            
            ToolTip toolTip = new ToolTip(piece.name + " tooltip", 
                piece.ability?.ToString() ?? "No special ability");

            FollowTransform followTransform = new FollowTransform(piece.transform, new Vector2(100, 100));
            toolTip.AddBehaviors([followTransform]);

            hover.OnStartHover += () => toolTip.SetActive(true);
            hover.OnEndHover += () => toolTip.SetActive(false);
            
            piece.OnDeath += _ => RemoveGameObjectAndChildren(toolTip);
            
            AddGameObjects([overlayObj, toolTip]);
            toolTip.SetActive(false);
        }

        ArrangeTeamPieces();

        AddGameObjects(whitePlayer.teamPieces.Concat(blackPlayer.teamPieces).Cast<
            GameObject>().ToList());

        endTurnButton.AddListener(() =>
        {
            if (GamePhaseManager.instance.phase == GamePhase.Gameplay)
                TurnManager.instance.ChangeTurn();
            endButtonCooldown();
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

            endButtonCooldown();
        };

        GamePhaseManager.instance.phase = GamePhase.Setup;
        
        whiteHud.UpdateText();
        blackHud.UpdateText();
    }

    private async Task endButtonCooldown()
    {
        endTurnButton.SetClickable(false);
        await Task.Delay(2000);
        endTurnButton.SetClickable(true);
    }

    private void ArrangeTeamPieces()
    {
        Vector2 center = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
        float groupDist = 700;
        float upOffset = -200;
        float rightOffset = -125;
        float elementDist = 150;

        Vector2 groupTopLeft = center + Vector2.UnitX * (rightOffset + groupDist) + Vector2.UnitY * upOffset;
        for (int i = 0; i < whitePlayer.teamPieces.Count; i++)
        {
            ChessPiece piece = whitePlayer.teamPieces[i];
            piece.transform.parentSpacePos = groupTopLeft
                                             + Vector2.UnitX * elementDist * (i % 2)
                                             + Vector2.UnitY * elementDist * (i / 2);
        }

        groupTopLeft = center + Vector2.UnitX * (rightOffset - groupDist) + Vector2.UnitY * upOffset;
        for (int i = 0; i < blackPlayer.teamPieces.Count; i++)
        {
            ChessPiece piece = blackPlayer.teamPieces[i];
            piece.transform.parentSpacePos = groupTopLeft
                                             + Vector2.UnitX * elementDist * (i % 2)
                                             + Vector2.UnitY * elementDist * (i / 2);
            
        }

        foreach (var gameObject in gameObjects)
        {
            if (gameObject is ToolTip tooltip)
            {
                tooltip.TryGetBehavior<FollowTransform>()?.MoveNow();
            }
        }
    }
}