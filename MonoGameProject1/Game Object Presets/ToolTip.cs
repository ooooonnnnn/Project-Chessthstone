using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class ToolTip : GameObject
{
    private TextRenderer _textRenderer;
    public string Text {
        get => _textRenderer.Text;
        set
        {
            _textRenderer.Text = value;
            
        }
    }
    private NineSlicedSprite _nineSlicedBackground;

    public ToolTip(string name, string text, int maxWidth = 100, int padding = 10) : base(name)
    {
        this._textRenderer = new TextRenderer(text, maxWidth + padding);
    }
    
    public void GenerateBackground(Texture2D texture, Rectangle sourceRectangle, int padding = 10)
    {
    }
}