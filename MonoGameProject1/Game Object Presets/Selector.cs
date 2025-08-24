using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Behaviors;
using MonoGameProject1.Behaviors.Abstract;
using MonoGameProject1.Extensions;
using MonoGameProject1.Settings;

namespace MonoGameProject1;

/// <summary>
/// Allows scrolling through a list of sprites
/// </summary>
public class Selector : GameObject
{
    public Sprite currentSprite => _currentSprite.Value;
    public Transform transform = new();
    private SelectorRenderer _selectorRenderer = new();
    private LinkedList<Sprite> _sprites;
    private LinkedListNode<Sprite> _currentSprite;
    private TextRenderer _textRenderer;
    private Transform _textTransform;
    public Button nextButton;
    public Button previousButton;
    /// <summary>
    /// Invoked when the current sprite changes (NextSprite and PreviousSprite)
    /// </summary>
    public event Action<Sprite> OnSpriteChanged;
    private const float _nextAndPrevBtnDist = 50;

    public Selector(string name, IEnumerable<Sprite> sprites) : base(name)
    {
        // AddBehaviors([transform]);
        AddBehaviors([transform, _selectorRenderer]);

        //Text Child
        var textBox = new TextBox($"{name} text", "", 200);
        _textTransform = textBox.transform;
        _textRenderer = textBox.textRenderer;
        _textRenderer.color = GraphicsSettings.textColor;
        transform.AddChild(textBox);
        _textTransform.parentSpacePos = Vector2.UnitY * 70;

        //Sprite children
        InitializeSprites(sprites);

        //Button children
        nextButton = new Button($"{name} next button");
        previousButton = new Button($"{name} previous button");
        nextButton.AddListener(() => NextOrPrevious(true));
        previousButton.AddListener(() => NextOrPrevious(false));
        transform.AddChild(nextButton.transform);
        transform.AddChild(previousButton.transform);

        nextButton.transform.origin = nextButton.spriteRenderer.sizePx.ToVector2() * 0.5f;
        previousButton.transform.origin = previousButton.spriteRenderer.sizePx.ToVector2() * 0.5f;
        Vector2 buttonDistance = Vector2.UnitX * 90;
        nextButton.transform.parentSpacePos = buttonDistance;
        previousButton.transform.parentSpacePos = -buttonDistance;
        Vector2 buttonScale = new Vector2(0.3f, 0.4f);
        nextButton.transform.parentSpaceScale = buttonScale;
        previousButton.transform.parentSpaceScale = buttonScale;
        
        const float cornerScaling = 0.3f;
        (nextButton.spriteRenderer as NineSliced).cornerScale = cornerScaling;
        (previousButton.spriteRenderer as NineSliced).cornerScale = cornerScaling;
        
        //Arrows on buttons
        Sprite nextArrow = new Sprite($"{name} next arrow", TextureManager.RightArrowTexture);
        Sprite prevArrow = new Sprite($"{name} previous arrow", TextureManager.RightArrowTexture);
        prevArrow.spriteRenderer.effects |= SpriteEffects.FlipHorizontally;
        nextButton.transform.AddChild(nextArrow.transform);
        previousButton.transform.AddChild(prevArrow.transform);
        nextButton.spriteRenderer.AddChild(nextArrow.spriteRenderer);
        previousButton.spriteRenderer.AddChild(prevArrow.spriteRenderer);
        
        nextArrow.spriteRenderer.layerDepth = nextButton.spriteRenderer.layerDepth - 0.01f;
        prevArrow.spriteRenderer.layerDepth = previousButton.spriteRenderer.layerDepth - 0.01f;
        
        nextArrow.transform.origin = nextArrow.spriteRenderer.sizePx.ToVector2() * 0.5f;
        prevArrow.transform.origin = prevArrow.spriteRenderer.sizePx.ToVector2() * 0.5f;
        float arrowScale = 0.75f;
        nextArrow.transform.SetScaleFromFloat(arrowScale);
        prevArrow.transform.SetScaleFromFloat(arrowScale);
        nextArrow.transform.parentSpacePos = nextButton.transform.origin;
        prevArrow.transform.parentSpacePos = previousButton.transform.origin;

        UpdateText();
    }

    /// <summary>
    /// Adds the sprites as children 
    /// </summary>
    /// <param name="sprites"></param>
    private void InitializeSprites(IEnumerable<Sprite> sprites)
    {
        _sprites = new LinkedList<Sprite>(sprites);
        _currentSprite = _sprites.First;
        UpdateText();
        List<GameObject> objects = new();
        // foreach (var sprite in _sprites)
        // {
        //     Transform childTransform = sprite.transform;
        //     transform.AddChild(childTransform);
        //     childTransform.origin = sprite.spriteRenderer.sizePx.ToVector2() * 0.5f;
        //     sprite.spriteRenderer.sizePx = new Point(100, 100);
        //     childTransform.parentSpacePos = Vector2.Zero;
        //     sprite.SetActive(sprite == currentSprite);
        //
        //     objects.Add(sprite);
        // }
        _selectorRenderer.JumpToSprite(_currentSprite);
        
        OnSpriteChanged?.Invoke(_currentSprite.Value);

        parentScene?.AddGameObjects(objects);
    }

    /// <summary>
    /// Removes the current sprites from the scene and adds the new ones
    /// </summary>
    /// <param name="disposeOldSprites">set to false to prevent removal and disposal of old sprites</param>
    public void ChangeSprites(IEnumerable<Sprite> sprites, bool disposeOldSprites = true)
    {
        foreach (var sprite in _sprites)
        {
            if (disposeOldSprites)
            {
                parentScene.RemoveGameObjectAndChildren(sprite);
            }
            else
            {
                sprite.SetActive(false);
            }

            transform.RemoveChild(sprite.transform);
        }

        InitializeSprites(sprites);
    }

    private async Task NextOrPrevious(bool forwards)
    {
        if (_currentSprite == null)
            return;
        
        _currentSprite = forwards ? _currentSprite.NextOrFirst() : _currentSprite.PreviousOrLast();
        OnSpriteChanged?.Invoke(_currentSprite.Value);
        UpdateText();
        
        SetButtonsClickable(false);
        await _selectorRenderer.AnimateToSprite(_currentSprite, forwards);
        SetButtonsClickable(true);
    }

    private void SetButtonsClickable(bool clickable)
    {
        nextButton.SetClickable(clickable);
        previousButton.SetClickable(clickable);
    }

    private void SetActiveVisibleSprites(bool active)
    {
        _currentSprite.Value?.SetActive(active);
        // _nextSprite.Value?.SetActive(active);
        // _previousSprite.Value?.SetActive(active);
    }

    private void PositionVisibleSprites()
    {
        _currentSprite.Value.transform.parentSpacePos = Vector2.Zero;
        // _previousSprite.Value.transform.parentSpacePos = -_nextAndPrevBtnDist * Vector2.UnitX;
        // _nextSprite.Value.transform.parentSpacePos = _nextAndPrevBtnDist * Vector2.UnitX;
    }

    /// <summary>
    /// When activating, don't activate the sprites that aren't selected
    /// </summary>
    public override void SetActive(bool active)
    {
        //Set active on children
        //deactivate normally
        if (!active)
        {
            base.SetActive(false);
        }
        else //active = true:
        {
            foreach (HierarchicalBehavior hierarchy in hierarchicalBehaviors)
            {
                foreach (var child in hierarchy.children)
                {
                    if (child is not Sprite childSprite)
                    {
                        child.SetActive(true);
                        continue;
                    }

                    //don't activate sprites from the list that aren't selected
                    if (!_sprites.Contains(childSprite))
                    {
                        childSprite.SetActive(true);
                        continue;
                    }

                    if (childSprite == currentSprite)
                        child.SetActive(true);
                }
            }
        }
    }

    private void UpdateText()
    {
        _textRenderer.Text = _currentSprite.Value.name;
        CenterText();
    }

    //TODO: this should be a function of textRenderer
    private void CenterText()
    {
        _textTransform.origin = _textRenderer.Font.MeasureString(_textRenderer.Text) * 0.5f;
    }

    public override void Dispose()
    {
        base.Dispose();
        OnSpriteChanged = null;
    }
}