using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class NineSlicedSprite : GameObject
{
    public NineSlicedSprite(string name, Texture2D texture, int leftMargin, int rightMargin, int topMargin,
        int bottomMargin, float cornerScale) : base(name)
    {
        AddBehaviors(new List<Behavior>
        {
            new Transform(),
            new NineSliced(texture, leftMargin, rightMargin, topMargin, bottomMargin, cornerScale)
        });
    }
    
    public void SetSize(int width, int height)
    {
        var transform = TryGetBehavior<Transform>();
        if (transform != null)
        {
            //here I would set the size of the transform
        }
        else
        {
            throw new System.Exception("Transform behavior not found on NineSlicedSprite.");
        }
    }
}