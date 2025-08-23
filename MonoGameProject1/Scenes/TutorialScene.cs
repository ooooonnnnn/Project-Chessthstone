using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1.Scenes;

/// <summary>
/// Tutorial scene with navigation back to main menu or to team selection.
/// </summary>
public class TutorialScene : Scene
{
    public TutorialScene()
    {
        // Header text
        var header = new TextBox("Tutorial Header", "Tutorial");
        header.textRenderer.layerDepth = LayerDepthManager.UiDepth;
        // Center and scale header similar to WinScreenScene
        header.transform.origin = header.textRenderer.Font.MeasureString(header.text) * 0.5f;
        header.transform.SetScaleFromFloat(3.0f);

        // Body/placeholder text
        var body = new TextBox("Tutorial Body", "Welcome to the tutorial!\n(Placeholder) Learn the basics, then start by selecting your team.", 900, false);
        body.textRenderer.color = Color.Black;
        body.textRenderer.layerDepth = LayerDepthManager.UiDepth;
        // Center body horizontally by setting origin X to half its width
        body.transform.origin = new Vector2(body.textRenderer.GetTextSize().X * 0.5f, 0f);

        // Buttons
        var backToMenuButton = new Button("Back To Menu Button", "Back to Main Menu");
        var teamSelectionButton = new Button("Go To Team Selection Button", "Start Game");

        // Center origins like WinScreenScene
        backToMenuButton.transform.origin = backToMenuButton.spriteRenderer.sizePx.ToVector2() * 0.5f;
        teamSelectionButton.transform.origin = teamSelectionButton.spriteRenderer.sizePx.ToVector2() * 0.5f;

        // Match scaling to WinScreenScene
        backToMenuButton.transform.parentSpaceScale = new Vector2(3, 1);
        backToMenuButton.textChildTransform.parentSpaceScale = new Vector2(0.33f, 1) * 2;
        teamSelectionButton.transform.parentSpaceScale = new Vector2(3, 1);
        teamSelectionButton.textChildTransform.parentSpaceScale = new Vector2(0.33f, 1) * 2;

        backToMenuButton.AddListener(() => SceneManager.ChangeScene(new MainMenuScene()));
        teamSelectionButton.AddListener(() => SceneManager.ChangeScene(new TeamSelectionScene()));

        // Layout
        var center = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
        header.transform.parentSpacePos = center + new Vector2(0, -260);
        body.transform.parentSpacePos = center + new Vector2(0, -100);
        body.transform.parentSpaceScale *= 1.0f;

        // Space buttons vertically like WinScreenScene (200px) and center horizontally
        float spacing = 200f;
        backToMenuButton.transform.parentSpacePos = center + new Vector2(0, 0);
        teamSelectionButton.transform.parentSpacePos = center + new Vector2(0, spacing);

        AddGameObjects([
            header,
            body,
            backToMenuButton,
            teamSelectionButton
        ]);
    }

    public override void Initialize()
    {
        // No special initialization required
    }
}
