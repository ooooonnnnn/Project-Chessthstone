using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1.Scenes;

/// <summary>
/// Main menu scene: shows title and navigation buttons.
/// </summary>
public class MainMenuScene : Scene
{
    public MainMenuScene()
    {
        // Title
        var title = new TextBox("Main Menu Title", "Project Chesstone");
        var titleTransform = title.transform;

        // Center and scale title similar to WinScreenScene
        titleTransform.origin = title.textRenderer.Font.MeasureString(title.text) * 0.5f;
        titleTransform.SetScaleFromFloat(3.0f);

        // Buttons
        var teamSelectionButton = new Button("Team Selection Button", "Start Game");
        var tutorialButton = new Button("Tutorial Button", "Tutorial");
        var quitButton = new Button("Quit Button", "Quit");

        // Center origins like WinScreenScene
        teamSelectionButton.transform.origin = teamSelectionButton.spriteRenderer.sizePx.ToVector2() * 0.5f;
        tutorialButton.transform.origin = tutorialButton.spriteRenderer.sizePx.ToVector2() * 0.5f;
        quitButton.transform.origin = quitButton.spriteRenderer.sizePx.ToVector2() * 0.5f;

        // Match scaling to WinScreenScene
        teamSelectionButton.transform.parentSpaceScale = new Vector2(3, 1);
        teamSelectionButton.textChildTransform.parentSpaceScale = new Vector2(0.33f, 1) * 2;
        tutorialButton.transform.parentSpaceScale = new Vector2(3, 1);
        tutorialButton.textChildTransform.parentSpaceScale = new Vector2(0.33f, 1) * 2;
        quitButton.transform.parentSpaceScale = new Vector2(3, 1);
        quitButton.textChildTransform.parentSpaceScale = new Vector2(0.33f, 1) * 2;

        // Listeners
        teamSelectionButton.AddListener(() => SceneManager.ChangeScene(new TeamSelectionScene()));
        tutorialButton.AddListener(() => SceneManager.ChangeScene(new TutorialScene()));
        quitButton.AddListener(() => GameManager.ExitGame());

        // Layout
        var center = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
        titleTransform.parentSpacePos = center + new Vector2(0, -200);

        // Space buttons vertically like WinScreenScene (200px)
        float spacing = 200f;

        // Positions (centered horizontally)
        teamSelectionButton.transform.parentSpacePos = center + new Vector2(0, 0);
        tutorialButton.transform.parentSpacePos = center + new Vector2(0, spacing);
        quitButton.transform.parentSpacePos = center + new Vector2(0, spacing * 2);

        AddGameObjects(new List<GameObject>
        {
            title,
            teamSelectionButton,
            tutorialButton,
            quitButton
        });
    }

    public override void Initialize()
    {
        // No special initialization required
    }
}
