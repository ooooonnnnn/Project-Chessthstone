using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1.Scenes;

public class TestStartScene : Scene
{
    public TestStartScene()
    {
        GameObject kingBlack = new Sprite("kingBlack", TextureManager.GetChessPieceTexture(false, PieceType.King));

        var kingBlackAnimSheets = new Dictionary<string, SpriteSheet>
        {
            {
                "death", new SpriteSheet(
                    TextureManager.KingBlackDeathSheet, 1, 5)
            }
        };

        var kingBlackAnimDic = new Animation(kingBlackAnimSheets);
        
        kingBlack.AddBehaviors([
            kingBlackAnimDic,
            new SpriteRectCollider(),
            new SenseMouseHover(),
            new Clickable()
        ]);
        kingBlack.TryGetBehavior<Clickable>().OnClick += () =>
        {
            kingBlackAnimDic.ActiveAnimation = "death";
            kingBlackAnimDic.StartAnimation();
        };
        kingBlack.TryGetBehavior<Animation>().OnAnimationEnd += () =>
        {
            Console.WriteLine("Animation ended");
            kingBlackAnimDic.ActiveAnimation = "default";
            kingBlackAnimDic.StartAnimation();
        };

        AddGameObjects([kingBlack]);
    }
    
    
    public override void Initialize()
    {
        Console.WriteLine($"{this} isn't initializing anything");
    }
}