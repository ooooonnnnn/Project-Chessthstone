using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        // Buttons
        var backButton = new Button("Back To Menu Button", "Back to Main Menu");
        var startButton = new Button("Go To Team Selection Button", "Start Game");

        // Center origins like WinScreenScene
        backButton.transform.origin = backButton.spriteRenderer.sizePx.ToVector2() * 0.5f;
        startButton.transform.origin = startButton.spriteRenderer.sizePx.ToVector2() * 0.5f;

        // Match scaling to WinScreenScene
        backButton.transform.parentSpaceScale = new Vector2(3, 1);
        backButton.textChildTransform.parentSpaceScale = new Vector2(0.33f, 1) * 2;
        startButton.transform.parentSpaceScale = new Vector2(3, 1);
        startButton.textChildTransform.parentSpaceScale = new Vector2(0.33f, 1) * 2;

        backButton.AddListener(() => SceneManager.ChangeScene(new MainMenuScene()));
        startButton.AddListener(() => SceneManager.ChangeScene(new TeamSelectionScene()));

        // Layout
        var center = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();
        header.transform.parentSpacePos = center + new Vector2(0, -260);

        // Space buttons vertically like WinScreenScene (200px) and center horizontally
        float spacing = 50f;
        int bottom = GameManager.Graphics.Viewport.Height;
        int right = GameManager.Graphics.Viewport.Width;
        backButton.transform.origin = new Vector2(0, backButton.spriteRenderer.sourceRectangle.Height);
        startButton.transform.origin = startButton.spriteRenderer.sourceRectangle.Size.ToVector2();
        backButton.transform.parentSpacePos = new Vector2(spacing, bottom - spacing);
        startButton.transform.parentSpacePos = new Vector2(right - spacing, bottom - spacing);

        // Create four toggles arranged evenly in a row near the left side
        var toggles = new List<Toggle>();
        var contents = new List<GameObject>();

        // Base position near the left side of the screen, arranged in a column
        Vector2 columnStart = new Vector2(50, 50);;
        float toggleSpacingY = 200f; // vertical space between toggles
        
        for (int i = 0; i < 4; i++)
        {
            var toggle = new Toggle($"Tutorial Toggle {i + 1}",
                $"Option {i + 1}", canBeSwitchedOff: false) ;

            // Size and position
            toggle.ChangeBackgroundScale(new Vector2(2.0f, 1.0f));
            toggle.transform.parentSpacePos = columnStart + new Vector2(0, i * toggleSpacingY);

            // Linked content with a sprite child and a text child
            var content = new GameObject($"Toggle {i + 1} Content", [new Transform()]);
            var contentTransform = content.TryGetBehavior<Transform>();

            // Create sprite child (use a built-in icon as placeholder)
            Texture2D iconTexture = TextureManager.GetHealthIcon();
            if (i == 1) iconTexture = TextureManager.GetDamageIcon();
            if (i == 2) iconTexture = TextureManager.GetActionPointsIcon();
            if (i == 3) iconTexture = TextureManager.RightArrowTexture;

            var sprite = new Sprite($"Toggle {i + 1} Sprite", iconTexture);
            sprite.transform.origin = sprite.spriteRenderer.sizePx.ToVector2() * 0.5f;
            sprite.spriteRenderer.layerDepth = LayerDepthManager.UiDepth - 0.02f;
            sprite.transform.SetScaleFromFloat(0.75f);

            // Create text child under the sprite
            var text = new TextBox($"Toggle {i + 1} Text", $"Content for Option {i + 1}", 300, true);
            text.textRenderer.layerDepth = LayerDepthManager.UiDepth - 0.02f;

            // Position content area 
            contentTransform.parentSpacePos = GameManager.Graphics.Viewport.Bounds.Center.ToVector2();

            // Parent children to content container
            contentTransform.AddChild(sprite.transform);
            contentTransform.AddChild(text.transform);
            sprite.transform.parentSpacePos = new Vector2(0, 0);
            text.transform.parentSpacePos = new Vector2(0, 70);

            // Visibility toggling per requirement
            int capturedIndex = i;
            toggle.OnToggled += (isOn) =>
            {
                content.SetActive(isOn);
            };

            toggles.Add(toggle);
            contents.Add(content);
        }

        // Mutual exclusivity: when one turns on, all others turn off
        for (int i = 0; i < toggles.Count; i++)
        {
            int idx = i;
            toggles[i].OnToggled += (isOn) =>
            {
                if (!isOn) return; // only react when turned on
                for (int j = 0; j < toggles.Count; j++)
                {
                    if (j == idx) continue;
                    toggles[j].isOn = false;
                }
            };
        }

        // Initial states: first on by default, others off
        for (int i = 0; i < toggles.Count; i++)
        {
            if (i == 0)
            {
                // Force a state refresh so visuals/clickability match canBeSwitchedOff=false when on
                toggles[i].isOn = false;
                toggles[i].isOn = true;
                contents[i].SetActive(true);
            }
            else
            {
                toggles[i].isOn = false;
                contents[i].SetActive(false);
            }
        }

        // Add all to scene
        List<GameObject> toAdd = new()
        {
            header,
            backButton,
            startButton
        };
        toAdd.AddRange(toggles);
        toAdd.AddRange(contents);

        AddGameObjects(toAdd);
    }

    public override void Initialize()
    {
        // No special initialization required
    }
}
