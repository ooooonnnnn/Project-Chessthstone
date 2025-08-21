using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1.Scenes;

/// <summary>
/// Tutorial scene with navigation back to main menu or to team selection.
/// </summary>
public class TutorialScene : Scene
{
    private GameObject _header;
    private GameObject _body;
    private Button _backToMenuButton;
    private Button _teamSelectionButton;

    public TutorialScene()
    {
        // Header text
        var headerTransform = new Transform();
        var headerText = new TextRenderer("Tutorial");
        headerText.layerDepth = LayerDepthManager.UiDepth - 0.02f;
        _header = new GameObject("Tutorial Header", new List<Behavior> { headerTransform, headerText });
        // Center and scale header similar to WinScreenScene
        headerTransform.origin = headerText.Font.MeasureString(headerText.Text) * 0.5f;
        headerTransform.SetScaleFromFloat(3.0f);

        // Body/placeholder text
        var bodyTransform = new Transform();
        var bodyText = new TextRenderer("Welcome to the tutorial!\n(Placeholder) Learn the basics, then start by selecting your team.", 900, false)
        {
            color = Color.Black,
            layerDepth = LayerDepthManager.UiDepth - 0.02f
        };
        _body = new GameObject("Tutorial Body", new List<Behavior> { bodyTransform, bodyText });
        // Center body horizontally by setting origin X to half its width
        bodyTransform.origin = new Vector2(bodyText.GetTextSize().X * 0.5f, 0f);

        // Buttons
        _backToMenuButton = new Button("Back To Menu Button", "Back to Main Menu");
        _teamSelectionButton = new Button("Go To Team Selection Button", "Start Game");

        // Center origins like WinScreenScene
        _backToMenuButton.transform.origin = _backToMenuButton.spriteRenderer.sizePx.ToVector2() * 0.5f;
        _teamSelectionButton.transform.origin = _teamSelectionButton.spriteRenderer.sizePx.ToVector2() * 0.5f;

        // Match scaling to WinScreenScene
        _backToMenuButton.transform.parentSpaceScale = new Vector2(3, 1);
        _backToMenuButton.textChildTransform.parentSpaceScale = new Vector2(0.33f, 1) * 2;
        _teamSelectionButton.transform.parentSpaceScale = new Vector2(3, 1);
        _teamSelectionButton.textChildTransform.parentSpaceScale = new Vector2(0.33f, 1) * 2;

        _backToMenuButton.AddListener(() => SceneManager.ChangeScene(new MainMenuScene()));
        _teamSelectionButton.AddListener(() => SceneManager.ChangeScene(new TeamSelectionScene()));

        // Layout
        var center = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
        headerTransform.parentSpacePos = center + new Vector2(0, -260);
        bodyTransform.parentSpacePos = center + new Vector2(0, -100);
        bodyTransform.parentSpaceScale *= 1.0f;

        // Space buttons vertically like WinScreenScene (200px) and center horizontally
        float spacing = 200f;
        _backToMenuButton.transform.parentSpacePos = center + new Vector2(0, 0);
        _teamSelectionButton.transform.parentSpacePos = center + new Vector2(0, spacing);

        AddGameObjects(new List<GameObject>
        {
            _header,
            _body,
            _backToMenuButton,
            _teamSelectionButton
        });
    }

    public override void Initialize()
    {
        // No special initialization required
    }
}
