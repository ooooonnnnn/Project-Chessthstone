using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Extensions;
using MonoGameProject1.Settings;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// Renders the currently visible sprites of the selector, and the gradient overlay
/// </summary>
public class SelectorRenderer : Renderer
{
	/// <summary>
	/// how wide to show the sprites
	/// </summary>
	public float spriteRenderWidth = 100;
	private float _elementSeperation => spriteRenderWidth * 0.7f;
	
	private DrawArguments _currentSprite, _nextSprite, _previousSprite, _newSprite;
	/// <summary>
	/// How far the reel has been moved.
	/// </summary>
	private float _currentDisplacement;
	/// <summary>
	/// true = forward = scrolling to the left, false = backward = scrolling to the right
	/// </summary>
	private bool _currentDirection = true;
	private bool _nowAnimating = false;
	private bool _isMovingForward;
	
	/// <summary>
	/// for creating the gradient overlay
	/// </summary>
	private Texture2D _gradientTexture = TextureManager.GradientTexture;
	private Texture2D _pixelTexture = TextureManager.PixelTexture;
	
	public override void Draw(SpriteBatch spriteBatch)
	{
		// Draw current sprite
		spriteBatch.Draw(
			_currentSprite.texture,
			transform.worldSpacePos + Vector2.UnitX * _currentDisplacement,
			_currentSprite.sourceRectangle, color, transform.rotation,
			_currentSprite.origin, _currentSprite.scale , effects, LayerDepthManager.GameObjectDepth);
		
		// Draw next sprite
		spriteBatch.Draw(
			_nextSprite.texture,
			transform.worldSpacePos + Vector2.UnitX * (_elementSeperation + _currentDisplacement),
			_nextSprite.sourceRectangle, color, transform.rotation,
			_nextSprite.origin, _nextSprite.scale , effects, LayerDepthManager.GameObjectDepth);
		
		// Draw previous sprite
		spriteBatch.Draw(
			_previousSprite.texture,
			transform.worldSpacePos + Vector2.UnitX * (-_elementSeperation + _currentDisplacement),
			_previousSprite.sourceRectangle, color, transform.rotation,
			_previousSprite.origin, _previousSprite.scale , effects, LayerDepthManager.GameObjectDepth);
		
		// Draw new sprite (the one that will become previous or next)
		if (_nowAnimating && _newSprite != null)
		{
			float totalDisplacement = _currentDisplacement + 2 * _elementSeperation *
				(_isMovingForward ? -1 : 1);
			spriteBatch.Draw(
				_newSprite.texture,
				transform.worldSpacePos + Vector2.UnitX * totalDisplacement,
				_newSprite.sourceRectangle, color, transform.rotation,
				_newSprite.origin, _newSprite.scale , effects, LayerDepthManager.GameObjectDepth);
		}
		
		// Draw gradient overlays
		spriteBatch.Draw(
			_gradientTexture, transform.worldSpacePos + Vector2.UnitX * 40,
			null, GraphicsSettings.backGroundColor, transform.rotation,
			new Vector2(0, 0.5f), new Vector2(0.33f,100), SpriteEffects.None,
			LayerDepthManager.OverlayDepth);
		spriteBatch.Draw(
			_gradientTexture, transform.worldSpacePos - Vector2.UnitX * 40,
			null, GraphicsSettings.backGroundColor, transform.rotation,
			new Vector2(100, 0.5f), new Vector2(0.33f,100), SpriteEffects.FlipHorizontally,
			LayerDepthManager.OverlayDepth);
		
		
		// Draw obscurers
		spriteBatch.Draw(
			_pixelTexture, transform.worldSpacePos + Vector2.UnitX * 73,
			null, GraphicsSettings.backGroundColor, transform.rotation,
			new Vector2(0, 0.5f), new Vector2(100,100), SpriteEffects.None,
			LayerDepthManager.OverlayDepth);
		spriteBatch.Draw(
			_pixelTexture, transform.worldSpacePos - Vector2.UnitX * 73,
			null, GraphicsSettings.backGroundColor, transform.rotation,
			new Vector2(1, 0.5f), new Vector2(100,100), SpriteEffects.None,
			LayerDepthManager.OverlayDepth);
		
	}

	public void JumpToSprite(LinkedListNode<Sprite> spriteNode)
	{
		_currentSprite = new DrawArguments(spriteNode.Value.spriteRenderer, spriteRenderWidth);
		_nextSprite = new DrawArguments(spriteNode.NextOrFirst().Value.spriteRenderer, spriteRenderWidth);
		_previousSprite = new DrawArguments(spriteNode.PreviousOrLast().Value.spriteRenderer, spriteRenderWidth);
	}
	
	/// <summary>
	/// Animates the transition to a new sprite. Goes in the provided direction.
	/// </summary>
	/// <param name="spriteNode">The target sprite to reach</param>
	/// <param name="isForward">true to scroll left (right button pressed)</param>
	public async Task AnimateToSprite(LinkedListNode<Sprite> spriteNode, bool isForward)
	{
		//Define the four visible sprites
		LinkedListNode<Sprite> currentNode = isForward ?
			spriteNode.PreviousOrLast() : spriteNode.NextOrFirst(); 
		JumpToSprite(currentNode);
		
		_newSprite = new DrawArguments(
			(isForward ? spriteNode.NextOrFirst() : spriteNode.PreviousOrLast())
			.Value.spriteRenderer,
			spriteRenderWidth);
		
		//animate
	}
	
	private class DrawArguments
	{
		public Texture2D texture;
		public Rectangle sourceRectangle;
		public Vector2 origin;
		public Vector2 scale;

		public DrawArguments(SpriteRenderer spriteRenderer, float targetWidth)
		{
			texture = spriteRenderer.texture;
			sourceRectangle = spriteRenderer.sourceRectangle;
			origin = sourceRectangle.Size.ToVector2() * 0.5f;
			scale = Vector2.One * targetWidth / sourceRectangle.Width;
		}
	}
}

