using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1.Scenes;

public class TestStartScene : Scene
{
    public TestStartScene()
    {
        GameObject testBird = new Sprite("TestBird", TextureManager.GetLogoTexture());

        var birdAnimations = new Dictionary<string, SpriteSheet>
        {
            {
                "fly", new SpriteSheet(
                    TextureManager.TestSpriteSheetTexture, 4, 4)
            }
        };

        var testBirdAnimations = new Animation(birdAnimations, true, true, 60);
        testBird.AddBehaviors([testBirdAnimations]);
        testBirdAnimations.ActiveAnimation = "fly";
        
        AddGameObjects([testBird]);
    }
}