using System;
using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

/// <summary>
/// Button with text (as a child) and hover tinting. 
/// </summary>
public class Button : GameObject, ICallback
{
    /// <summary>
    /// The text appearing on the child
    /// </summary>
    public string text
    {
        get => _text;
        set
        {
            _text = value;
            _childTextRenderer.Text = value;
            CenterText();
        }
    }

    private string _text = "";
    public Texture2D texture;
    public Transform transform;


    /// <summary>
    /// Can be either a SpriteRenderer or a NineSliced, depending on the constructor used
    /// </summary>
    public SpriteRenderer spriteRenderer { get; private set; }

    public ChangeTintWhenHover hoverTinting;

    /// <summary>
    /// The behaviors of the child
    /// </summary>
    private TextRenderer _childTextRenderer;

    public Transform textChildTransform;

    private Clickable _clickable;

    /// <summary>
    /// A button with the default texture of a rounded square. 9-sliced scaling.
    /// </summary>
    /// <param name="text">Text on the button</param>
    public Button(string name, string text = "", Texture2D texture = null) : base(name)
    {
        if (texture == null)
        {
            texture = TextureManager.ToolTipNineSliceTexture;
            spriteRenderer = new NineSliced(texture, 50, 99, 50, 99, 0.5f);
        }
        else
            spriteRenderer = new NineSliced(texture, 0, texture.Width - 1, 0, texture.Height - 1);

        spriteRenderer.layerDepth = LayerDepthManager.UiDepth;
        AddButtonBehaviors();

        CreateTextChild();
        this.text = text;
    }

    /// <summary>
    /// Creates the text child
    /// </summary>
    /// <returns>The text renderer of the child</returns>
    private void CreateTextChild()
    {
        Transform textTransform = new Transform();
        textTransform.parentSpaceScale *= 1.4f;
        TextRenderer textRenderer = new TextRenderer(text) { color = Color.Black };
        GameObject textChild = new GameObject(name + " Text", [textTransform, textRenderer]);

        transform.AddChild(textChild);

        _childTextRenderer = textRenderer;
        textChildTransform = textTransform;
        _childTextRenderer.layerDepth = LayerDepthManager.UiDepth - 0.01f;

        CenterText();
    }

    private void CenterText()
    {
        textChildTransform.origin = _childTextRenderer.Font.MeasureString(text) * 0.5f;
        textChildTransform.parentSpacePos = new Vector2(
            spriteRenderer.sourceWidth * 0.5f, spriteRenderer.sourceHeight * 0.5f);
    }

    public void ChangeBackgroundScale(Vector2 newScale)
    {
        transform.parentSpaceScale *= newScale;
        textChildTransform.parentSpaceScale *= new Vector2(1f / newScale.X, 1f / newScale.Y);
    }

    private void AddButtonBehaviors()
    {
        transform = new Transform();
        hoverTinting = new ChangeTintWhenHover();
        _clickable = new Clickable();
        AddBehaviors(new List<Behavior>
        {
            transform,
            spriteRenderer,
            new SpriteRectCollider(),
            new SenseMouseHover(),
            hoverTinting,
            _clickable
        });
    }

    public void AddListener(Action listener)
    {
        _clickable.OnClick += listener;
    }

    public void RemoveListener(Action listener)
    {
        _clickable.OnClick -= listener;
    }
}