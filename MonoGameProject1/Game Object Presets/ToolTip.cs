using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public class ToolTip : GameObject
{
    public TextRenderer textRenderer;
    public string Text {
        get => textRenderer.Text;
        set
        {
            textRenderer ??= new TextRenderer(value);
            textRenderer.Text = value;
            ModifyBackgroundSize(textRenderer.MaxWidth + padding);
        }
    }
    private NineSliced _nineSlicedBackground;
    private Transform _nineSlicedBackgroundTransform;
    
    public Transform transform;
    
    public int padding = 10;

    public ToolTip(string name, string text, int width = 200, int padding = 50) : base(name)
    {
        this.padding = padding;
        this.textRenderer = new TextRenderer(text, width - padding);
        this.textRenderer.layerDepth = LayerDepthManager.UiDepth - 0.01f;
        
        transform = new Transform();
        AddBehaviors([transform, textRenderer]);
        
        this._nineSlicedBackground = new NineSliced(
            TextureManager.ToolTipNineSliceTexture,
            50, 99, 50, 99, 0.5f);
        
        this._nineSlicedBackgroundTransform = new Transform();
        
        transform.AddChild(new GameObject("ToolTipBackground",
            [_nineSlicedBackground, _nineSlicedBackgroundTransform]));
        _nineSlicedBackground.layerDepth = LayerDepthManager.UiDepth;
        
        _nineSlicedBackgroundTransform.origin = Vector2.Zero;
        _nineSlicedBackgroundTransform.parentSpacePos = new Vector2(-this.padding, -this.padding);
        
        ModifyBackgroundSize(width + this.padding);
    }
    
    
    private void ModifyBackgroundSize(int width)
    {
        // Adjust the size of the background based on the text size
        var textSize = textRenderer.GetTextSize();
        _nineSlicedBackground.sizePx = new (width, (int)textSize.Y + padding * 2);
    }
}