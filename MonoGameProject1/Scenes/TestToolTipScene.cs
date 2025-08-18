using System;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Scenes;

public class TestToolTipScene : Scene
{
    public TestToolTipScene()
    {
        ToolTip toolTip = new ToolTip("TestToolTip",
            "This is a test tooltip with a longer text to see how it behaves when the text is longer than the width of the tooltip.",
            400);
        toolTip.transform.parentSpacePos = new Vector2(100, 100);

        Button button = new Button("trololol", "trololololololololo lololol");
        
        AddGameObjects([toolTip, button]);
    }
    
    
    public override void Initialize()
    {
        Console.WriteLine($"{this} isn't initializing anything");
    }
}