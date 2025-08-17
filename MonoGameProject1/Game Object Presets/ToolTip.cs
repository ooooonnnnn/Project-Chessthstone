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
            modifyBackgroundSize(_textRenderer.MaxWidth + _padding);
        }
    }
    private NineSliced _nineSlicedBackground;
    private Transform _nineSlicedBackgroundTransform;
    
    public Transform transform;
    
    private int _padding = 10;

    public ToolTip(string name, string text, int width = 100, int padding = 10) : base(name)
    {
        this._padding = padding;
        this._textRenderer = new TextRenderer(text, width - padding);
        transform = new Transform();
        AddBehaviors([transform, _textRenderer]);
        
        this._nineSlicedBackground = new NineSliced(
            TextureManager.TestSpriteSheetTexture,
            padding, padding, padding, padding);
        
        this._nineSlicedBackgroundTransform = new Transform();
        
        transform.AddChild(new GameObject("ToolTipBackground",
            [_nineSlicedBackground, _nineSlicedBackgroundTransform]));
    }
    
    
    private void modifyBackgroundSize(int width)
    {
        // Adjust the size of the background based on the text size
        var textSize = _textRenderer.GetTextSize();
        _nineSlicedBackground.sizePx = new (width, (int)textSize.Y + _padding);
    }
}