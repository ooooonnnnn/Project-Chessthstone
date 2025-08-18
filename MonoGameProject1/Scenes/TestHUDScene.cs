using MonoGameProject1.Behaviors;

namespace MonoGameProject1.Scenes;

public class TestHUDScene : Scene
{
    public TestHUDScene()
    {   
        GamePhaseManager.Instantiate("Shawarma");
        TurnManager.Instantiate("Bolbolim");
        // Create a HUD with a health bar and a mana bar
        PlayerStatsHUD hud = new PlayerStatsHUD(true,10);
        hud.transform.parentSpacePos = new Microsoft.Xna.Framework.Vector2(10, 10);
        
        AddGameObjects([hud]);
    }
}