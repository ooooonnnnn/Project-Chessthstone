using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1.Scenes;

/// <summary>
/// Main menu scene: shows title and navigation buttons.
/// </summary>
public class MainMenuScene : Scene
{
    private GameObject _title;
    private Button _teamSelectionButton;
    private Button _tutorialButton;
    private Button _quitButton;

    public MainMenuScene()
    {
        // Title
        var titleTransform = new Transform();
        var titleText = new TextRenderer("Project Chesstone");
        _title = new GameObject("Main Menu Title", new List<Behavior> { titleTransform, titleText });

        // Center and scale title similar to WinScreenScene
        titleTransform.origin = titleText.Font.MeasureString(titleText.Text) * 0.5f;
        titleTransform.SetScaleFromFloat(3.0f);

        // Buttons
        _teamSelectionButton = new Button("Team Selection Button", "Start Game");
        _tutorialButton = new Button("Tutorial Button", "Tutorial");
        _quitButton = new Button("Quit Button", "Quit");

        // Center origins like WinScreenScene
        _teamSelectionButton.transform.origin = _teamSelectionButton.spriteRenderer.sizePx.ToVector2() * 0.5f;
        _tutorialButton.transform.origin = _tutorialButton.spriteRenderer.sizePx.ToVector2() * 0.5f;
        _quitButton.transform.origin = _quitButton.spriteRenderer.sizePx.ToVector2() * 0.5f;

        // Match scaling to WinScreenScene
        _teamSelectionButton.transform.parentSpaceScale = new Vector2(3, 1);
        _teamSelectionButton.textChildTransform.parentSpaceScale = new Vector2(0.33f, 1) * 2;
        _tutorialButton.transform.parentSpaceScale = new Vector2(3, 1);
        _tutorialButton.textChildTransform.parentSpaceScale = new Vector2(0.33f, 1) * 2;
        _quitButton.transform.parentSpaceScale = new Vector2(3, 1);
        _quitButton.textChildTransform.parentSpaceScale = new Vector2(0.33f, 1) * 2;

        // Listeners
        _teamSelectionButton.AddListener(() => SceneManager.ChangeScene(new TeamSelectionScene()));
        _tutorialButton.AddListener(() => SceneManager.ChangeScene(new TutorialScene()));
        _quitButton.AddListener(() => GameManager.ExitGame());

        // Layout
        var center = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
        titleTransform.parentSpacePos = center + new Vector2(0, -200);

        // Space buttons vertically like WinScreenScene (200px)
        float spacing = 200f;

        // Positions (centered horizontally)
        _teamSelectionButton.transform.parentSpacePos = center + new Vector2(0, 0);
        _tutorialButton.transform.parentSpacePos = center + new Vector2(0, spacing);
        _quitButton.transform.parentSpacePos = center + new Vector2(0, spacing * 2);

        AddGameObjects(new List<GameObject>
        {
            _title,
            _teamSelectionButton,
            _tutorialButton,
            _quitButton
        });
    }

    public override void Initialize()
    {
        // No special initialization required
    }
}
