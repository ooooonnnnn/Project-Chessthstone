using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

/// <summary>
/// GameObject with a NineSliced renderer (like Sprite but nine-sliced). Uses the same default
/// texture and nine-slice configuration as Button and ToolTip.
/// </summary>
public class Frame : GameObject
{
    public Transform transform;
    public NineSliced spriteRenderer;

    /// <summary>
    /// Creates a Frame using the default nine-slice texture and margins used by Button/ToolTip.
    /// </summary>
    public Frame(string name) : base(name,
        new List<Behavior>
        {
            new Transform(),
            new NineSliced(TextureManager.ToolTipNineSliceTexture, 50, 99, 50, 99, 0.5f)
        })
    {
        GetBehaviorReferences();
    }

    private void GetBehaviorReferences()
    {
        transform = TryGetBehavior<Transform>();
        spriteRenderer = TryGetBehavior<NineSliced>();
    }
}
