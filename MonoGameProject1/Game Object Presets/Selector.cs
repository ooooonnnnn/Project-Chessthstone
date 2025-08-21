using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;
using MonoGameProject1.Behaviors.Abstract;

namespace MonoGameProject1;

/// <summary>
/// Allows scrolling through a list of sprites
/// </summary>
public class Selector : GameObject
{
    public Sprite currentSprite => _currentSprite.Value;
    public Transform transform;
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

    public Selector(string name, IEnumerable<Sprite> sprites) : base(name)
    {
        transform = new Transform();
        AddBehaviors([transform]);

        //Text Child
        _textTransform = new Transform();
        _textRenderer = new TextRenderer("", 200, true);
        new GameObject($"{name} text", [_textTransform, _textRenderer]);
        _textRenderer.color = Color.Beige;
        transform.AddChild(_textTransform);
        _textTransform.parentSpacePos = Vector2.UnitY * -50;

        //Sprite children
        InitializeSprites(sprites);

        //Button children
        nextButton = new Button($"{name} next button", "Next");
        previousButton = new Button($"{name} previous button", "Previous");
        nextButton.ChangeBackgroundScale(new Vector2(1, 0.7f));
        previousButton.ChangeBackgroundScale(new Vector2(1, 0.7f));
        nextButton.AddListener(NextSprite);
        previousButton.AddListener(PreviousSprite);

        transform.AddChild(nextButton.transform);
        transform.AddChild(previousButton.transform);

        nextButton.transform.parentSpacePos = Vector2.UnitY * -150;
        previousButton.transform.parentSpacePos = Vector2.UnitY * 150;
        nextButton.transform.origin = nextButton.spriteRenderer.sizePx.ToVector2() * 0.5f;
        previousButton.transform.origin = previousButton.spriteRenderer.sizePx.ToVector2() * 0.5f;

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
        foreach (var sprite in _sprites)
        {
            Transform childTransform = sprite.transform;
            transform.AddChild(childTransform);
            childTransform.parentSpacePos = new(-32, 0);
            sprite.spriteRenderer.sizePx = new Point(100, 100);
            childTransform.origin = sprite.spriteRenderer.sizePx.ToVector2() * 0.5f;
            if (sprite == currentSprite)
                sprite.SetActive(true);
            else
                sprite.SetActive(false);

            objects.Add(sprite);
        }
        
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

    private void NextSprite()
    {
        if (_currentSprite == null)
            return;
        _currentSprite.Value?.SetActive(false);
        _currentSprite = _currentSprite.Next ?? _sprites.First;
        _currentSprite.Value?.SetActive(true);
        
        OnSpriteChanged?.Invoke(_currentSprite.Value);

        UpdateText();
    }

    private void PreviousSprite()
    {
        if (_currentSprite == null)
            return;
        _currentSprite.Value?.SetActive(false);
        _currentSprite = _currentSprite.Previous ?? _sprites.Last;
        _currentSprite.Value?.SetActive(true);
        
        OnSpriteChanged?.Invoke(_currentSprite.Value);

        UpdateText();
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