using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

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

	public Selector(string name, IEnumerable<Sprite> sprites) : base(name)
	{
		
		transform = new Transform();
		AddBehaviors([transform]);

		//Text Child
		_textTransform = new Transform();
		_textRenderer = new TextRenderer();
		new GameObject($"{name} text", [_textTransform, _textRenderer]);
		_textRenderer.color = Color.Black;
		transform.AddChild(_textTransform);
		_textTransform.parentSpacePos = Vector2.UnitY * -80;
		
		//Sprite children
		InitializeSprites(sprites);

		//Button children
		Button next = new Button($"{name} next button", "Next");
		Button previous = new Button($"{name} previous button", "Previous");
		next.AddListener(NextSprite);
		previous.AddListener(PreviousSprite);
		
		transform.AddChild(next.transform);
		transform.AddChild(previous.transform);
		
		next.transform.parentSpacePos = Vector2.UnitY * -150;
		previous.transform.parentSpacePos = Vector2.UnitY * 150;
		next.transform.origin = next.spriteRenderer.sizePx.ToVector2() * 0.5f;
		previous.transform.origin = previous.spriteRenderer.sizePx.ToVector2() * 0.5f;
		
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
			childTransform.parentSpacePos = Vector2.Zero;
			sprite.spriteRenderer.sizePx = new Point(100, 100);
			childTransform.origin = sprite.spriteRenderer.sizePx.ToVector2() * 0.5f;
			if (sprite == currentSprite)
				sprite.SetActive(true);
			else
				sprite.SetActive(false);
			
			objects.Add(sprite);
		}
		
		parentScene?.AddGameObjects(objects);
	}

	/// <summary>
	/// Removes the current sprites from the scene and adds the new ones
	/// </summary>
	public void ChangeSprites(IEnumerable<Sprite> sprites)
	{
		foreach (var sprite in _sprites)
		{
			parentScene.RemoveGameObject(sprite);
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
		
		UpdateText();
	}

	private void PreviousSprite()
	{
		if (_currentSprite == null) 
			return;
		_currentSprite.Value?.SetActive(false);
		_currentSprite = _currentSprite.Previous ?? _sprites.Last;
		_currentSprite.Value?.SetActive(true);
		
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
			foreach (IHierarchy hierarchy in hierarchicalBehaviors)
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
		_textRenderer.text = _currentSprite.Value.name;
		CenterText();
	}
	//TODO: this should be a function of textRenderer
	private void CenterText()
	{
		_textTransform.origin = _textRenderer.font.MeasureString(_textRenderer.text) * 0.5f;
	}
}
