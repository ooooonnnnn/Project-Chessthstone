using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Behaviors;
using MonoGameProject1.Extensions;

namespace MonoGameProject1.Scenes;

public class WinScreenScene : Scene
{
    public WinScreenScene(bool whiteWon)
    {
        string color = whiteWon ? "White" : "Black";
        var winText = new TextBox("win text obj", $"{color} player won!");
        var textTrans = winText.transform;

        // Animation of king celebrating and king dying
        var winnerSheet =
            new SpriteSheet(
                whiteWon ? TextureManager.KingWhiteWinSheet : TextureManager.KingBlackWinSheet,
                6, 1);
        var loserSheet =
            new SpriteSheet(
                whiteWon ? TextureManager.KingBlackDeathSheet : TextureManager.KingWhiteDeathSheet,
                5, 1);
        Dictionary<string, SpriteSheet> sceneSheets = new();
        sceneSheets["winner"] = winnerSheet;
        sceneSheets["loser"] = loserSheet;
        Animation winnerAnim = new(sceneSheets, false, false, 5);
        Animation loserAnim = new(sceneSheets, false, false, 5);
        Sprite winnerSprite = new("winner sprite", TextureManager.GetChessPieceTexture(whiteWon, PieceType.King));
        Sprite loserSprite = new("loser sprite", TextureManager.GetChessPieceTexture(!whiteWon, PieceType.King));
        winnerSprite.transform.origin = winnerSprite.spriteRenderer.sourceRectangle.Size.ToVector2() / 2;
        loserSprite.transform.origin = loserSprite.spriteRenderer.sourceRectangle.Size.ToVector2() / 2;
        winnerSprite.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2() +
                                                Vector2.UnitX * -500;
        loserSprite.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2() +
                                               Vector2.UnitX * 450 + Vector2.UnitY * 50;
        winnerSprite.AddBehaviors([winnerAnim]);
        loserSprite.AddBehaviors([loserAnim]);
        winnerAnim.ActiveAnimation = "winner";
        loserAnim.ActiveAnimation = "loser";
        winnerAnim.StartAnimation();
        loserAnim.StartAnimation();

        Button restart = new Button("restart", "Restart");
        Button quit = new Button("quit", "Quit");

        restart.AddListener(() => SceneManager.ChangeScene(new TeamSelectionScene()));
        quit.AddListener(GameManager.ExitGame);

        textTrans.origin = winText.textRenderer.Font.MeasureString(winText.text) * 0.5f;
        restart.transform.origin = restart.spriteRenderer.sizePx.ToVector2() * 0.5f;
        quit.transform.origin = quit.spriteRenderer.sizePx.ToVector2() * 0.5f;

        textTrans.SetScaleFromFloat(3f);
        restart.transform.parentSpaceScale = new Vector2(3, 1);
        restart.textChildTransform.parentSpaceScale = new Vector2(0.33f, 1) * 2;
        quit.transform.parentSpaceScale = new Vector2(3, 1);
        quit.textChildTransform.parentSpaceScale = new Vector2(0.33f, 1) * 2;

        textTrans.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2() -
                                   Vector2.UnitY * 200;
        restart.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
        quit.transform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2() +
                                        Vector2.UnitY * 200;


        AddGameObjects([winText, restart, quit, winnerSprite, loserSprite]);
    }

    public override void Initialize()
    {
        // Console.WriteLine($"{this} isn't initializing anything");
    }
}