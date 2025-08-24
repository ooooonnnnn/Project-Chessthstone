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
        #region buttons

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
        Vector2 columnStart = new Vector2(50, 50);
        float toggleSpacingY = 200f; // vertical space between toggles
        
        for (int i = 0; i < 4; i++)
        {
            var toggle = new Toggle($"Tutorial Toggle {i + 1}",
                $"Option {i + 1}", canBeSwitchedOff: false);

            // Size and position
            toggle.ChangeBackgroundScale(new Vector2(2.0f, 1.0f));
            toggle.transform.parentSpacePos = columnStart + new Vector2(0, i * toggleSpacingY);
            toggle.textChildTransform.parentSpaceScale *= 1.2f;

            // Linked content with a sprite child and a text child
            var content = new GameObject($"Toggle {i + 1} Content", [new Transform()]);
            var contentTransform = content.TryGetBehavior<Transform>();


            // Position content area 
            contentTransform.parentSpacePos = new Vector2(400, columnStart.Y);

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
                    if (j == idx) 
                        continue;
                    toggles[j].isOn = false;
                }
            };
        }

        #endregion

        #region content

        // Name sections
        toggles[0].text = "Overview";
        toggles[1].text = "Gameplay";
        toggles[2].text = "Pieces";
        toggles[3].text = "Abilities";
        
        // Add content for Overview
        AddTextContent(0, 
            "Welcome to Project Chesstone!\n\n" +
            "First, you might be wandering why it's called this way? Because it's a cross between" +
            " Chess and Hearthstone!\n" +
            "You will play a 1v1 match against another player on the same computer, so grab your " +
            "most cerebral buddy and prepare for action!\n\n" +
            "Players assemble a team of 6 chess pieces and fight until one side is gone\n\n" +
            "By choosing a synergistic team you will gain a tactical advantage, so choose wisely!\n" +
            "Some pieces have special abilities, some are just beefier... It's up to you!");

        // Add content for Gameplay
        AddTextContent(1, 
            "The game takes place in 3 phases: Team Building, Setup, and Battle.\n" +
            "Each of these phases has a turn order - the white player goes first - so choose " +
            "now who's the white player and who's the black player. " +
            "You will be passing the mouse when your turn ends.\n\n" +
            "TEAM BUILDING: Each player assembles their team, while the other player looks away!\n" +
            "You aren't allowed to know your opponents team ahead of time!\n\n" +
            "SETUP: Each player places one of their pieces on their side of the board " +
            "(left click to pick and to place), until none remain, then the BATTLE starts!\n\n" +
            "BATTLE: On each turn, the player can make as many moves as they like as long as they can.\n" +
            "Left click a piece to select it, then left click a square to move there or to attack the occupying " +
            "piece.\n" +
            "You can also pass the turn early by clicking the PASS button.\n\n" +
            "The game ends when one player runs out of pieces.");
        
        // Add content for Pieces
        AddTextContent(2,
            "Each of the 6 piece types has their own unique movement pattern.\n\n" +
            "PAWN: Moves one space orthogonally, and attacks 1 space diagonally.\n" +
            "KNIGHT: Moves and attacks towards any square that is 2 spaces out and 1 across from it.\n" +
            "BISHOP: Moves diagonally as long as it isn't blocked, attacks blocking pieces.\n" +
            "ROOK: Like bishop but orthogonally. \n" +
            "QUEEN: Like the ROOK and BISHOP combined.\n" +
            "KING: Moves and attacks one space in all 8 directions.\n\n" +
            "Unlike regular chess, pieces have three stats to them: HP, DMG, and AP.\n" +
            "HP: Health points - the piece dies when its HP reaches 0.\n" +
            "DMG: Damage - when this piece attacks, the attacked piece loses HP equal to the DMG.\n" +
            "AP: Action points - moving and attacking costs 1 AP, which is regained when your turn starts.\n\n" +
            "Some pieces also have special abilities...");
        
        // Add content for Abilities
        AddTextContent(3,
            "Some pieces have special abilities which change up the rules of the game!\n" +
            "They come in 3 varieties: TRIGGERED, ACTIVATED, and STATIC.\n\n" +
            "TRIGGERED abilities have a one-shot effect that goes off whenever the specified event occurs." +
            " For example: An ability that says \"When your turn starts\" will trigger when your turn starts.\n\n" +
            "ACTIVATED abilities also have a one-shot effect, but they can be activated by the player" +
            " at any point during their turn, by paying a MANA COST. " +
            "(right click on the piece after selecting it).\n" +
            "MANA is a resource each player has. It's gained from " +
            "some abilities, and it resets to 0 when your turn starts.\n" +
            "[TIP]: This is why you should pick some pieces that create MANA and some pieces that use MANA.\n\n" +
            "STATIC abilities alter something about the game for as long as the piece is alive and the " +
            "specified condition is met. For example: \"All pieces on white tiles get 5 pts DMG increase, " +
            "and all pieces on black tiles get 5 pts DMG reduction.\"");
        
        void AddTextContent(int contentInd, string text)
        {
            TextBox textBox = new(toggles[contentInd].text + "content", text, 750);
            Transform transform = contents[contentInd].TryGetBehavior<Transform>();
            transform.AddChild(textBox);
            textBox.transform.parentSpacePos = Vector2.Zero;
            textBox.transform.SetScaleFromFloat(2f);
        }

        #endregion
        
        // Initial states: first on by default, others off
        for (int i = 0; i < toggles.Count; i++)
        {
            if (i == 0)
            {
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
            backButton,
            startButton
        };
        toAdd.AddRange(toggles);
        toAdd.AddRange(contents);

        AddGameObjects(toAdd);
    }

    public override void Initialize()
    {
    }
}
