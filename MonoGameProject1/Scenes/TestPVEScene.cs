using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

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
	
	public TestPVEScene(bool isRealPlayerWhite)
    {
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
		throw new System.NotImplementedException();
	}
}