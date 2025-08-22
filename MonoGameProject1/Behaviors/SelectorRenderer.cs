using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Extensions;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// Renders the currently visible sprites of the selector, and the gradient overlay
/// </summary>
public class SelectorRenderer : Renderer
{
	public LinkedListNode<Sprite> currentSprite;
	/// <summary>
	/// How far the reel has been moved.
	/// </summary>
	private float _currentDisplacement;
	/// <summary>
	/// true = forward = scrolling to the left, false = backward = scrolling to the right
	/// </summary>
	private bool _currentDirection = true;
	private bool _nowAnimating = false;
	
	public override void Draw(SpriteBatch spriteBatch)
	{
		if (!_nowAnimating)
		{
			Texture2D currTexture = currentSprite.Value.spriteRenderer.texture;
			Rectangle currRect = currentSprite.Value.spriteRenderer.sourceRectangle;
			Vector2 currOrg = currentSprite.Value.transform.origin;
			spriteBatch.Draw(
				currTexture, transform.worldSpacePos, currRect, color, transform.rotation,currOrg, transform.worldSpaceScale, effects, layerDepth
				
				);
		}
	}

	public void JumpToSprite(LinkedListNode<Sprite> spriteNode)
	{
		
	}
	
	/// <summary>
	/// Animates the transition to a new sprite. Goes in the provided direction.
	/// </summary>
	/// <param name="spriteNode">The </param>
	/// <param name="isForward"></param>
	public void AnimateToSprite(LinkedListNode<Sprite> spriteNode, bool isForward)
	{
		
	}
}