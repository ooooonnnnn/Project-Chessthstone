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

	public Selector(string name, IEnumerable<Sprite> sprites) : base(name)
	{
		_sprites = new LinkedList<Sprite>(sprites);
		_currentSprite = _sprites.First;
		
		transform = new Transform();
		AddBehaviors([transform]);

		foreach (var sprite in _sprites)
		{
			Transform childTransform = sprite.transform;
			transform.AddChild(childTransform);
			childTransform.parentSpacePos = Vector2.Zero;
			childTransform.origin = sprite.spriteRenderer.sizePx.ToVector2() * 0.5f;
		}
		
		//Button children
		Button next = new Button($"{name} next button", "Next");
		Button previous = new Button($"{name} previous button", "Previous");
		next.AddListener(NextSprite);
		previous.AddListener(PreviousSprite);
		
		transform.AddChild(next.transform);
		transform.AddChild(previous.transform);
		
		next.transform.parentSpacePos = Vector2.UnitY * -50;
		previous.transform.parentSpacePos = Vector2.UnitY * 50;
		next.transform.origin = next.spriteRenderer.sizePx.ToVector2() * 0.5f;
		previous.transform.origin = previous.spriteRenderer.sizePx.ToVector2() * 0.5f;
	}
	
	private void NextSprite()
	{
		_currentSprite.Value.SetActive(false);
		_currentSprite = _currentSprite.Next ?? _sprites.First;
		_currentSprite.Value.SetActive(true);
	}

	private void PreviousSprite()
	{
		_currentSprite.Value.SetActive(false);
		_currentSprite = _currentSprite.Previous ?? _sprites.Last;
		_currentSprite.Value.SetActive(true);
	}

	/// <summary>
	/// When activating, don't activate the sprites that aren't selected
	/// </summary>
	public override void SetActive(bool active)
	{
		//Set active on children
		if (!active)
		{
			//deactivate normally
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

					//don't activate sprites that aren't selected
					if (_sprites.Contains(childSprite) && childSprite == currentSprite)
						child.SetActive(true);
				}
			}
		}
	}
}