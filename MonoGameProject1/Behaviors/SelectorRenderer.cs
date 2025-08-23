using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Extensions;
using MonoGameProject1.Settings;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// Renders the currently visible sprites of the selector, and the gradient overlay
/// </summary>
public class SelectorRenderer : Renderer, IDisposable
{
	/// <summary>
	/// how wide to show the sprites
	/// </summary>
	public float spriteRenderWidth = 100;
	private float _elementSeperation => spriteRenderWidth * 0.7f;
	
	private DrawArguments currentSprite, nextSprite, previousSprite;
	/// <summary>
	/// How far the reel has been moved.
	/// </summary>
	private float _currentDisplacement;

	private FloatController _displacementController = new FloatController();
	/// <summary>
	/// true = forward = scrolling to the left, false = backward = scrolling to the right
	/// </summary>
	private bool _currentDirection = true;
	private bool _nowAnimating = false;
	
	/// <summary>
	/// for creating the gradient overlay
	/// </summary>
	private Texture2D _gradientTexture = TextureManager.GradientTexture;
	private Texture2D _pixelTexture = TextureManager.PixelTexture;

	public override void Initialize()
	{
		base.Initialize();
		_displacementController.HandleFloatChange = value => _currentDisplacement = value;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		// Draw current sprite
		spriteBatch.Draw(
			currentSprite.texture, transform.worldSpacePos,
			currentSprite.sourceRectangle, color, transform.rotation,
			currentSprite.origin, currentSprite.scale , effects, LayerDepthManager.GameObjectDepth);
		
		// Draw next sprite
		spriteBatch.Draw(
			nextSprite.texture, transform.worldSpacePos + Vector2.UnitX * _elementSeperation,
			nextSprite.sourceRectangle, color, transform.rotation,
			nextSprite.origin, nextSprite.scale , effects, LayerDepthManager.GameObjectDepth);
		
		// Draw previous sprite
		spriteBatch.Draw(
			previousSprite.texture, transform.worldSpacePos - Vector2.UnitX * _elementSeperation,
			previousSprite.sourceRectangle, color, transform.rotation,
			previousSprite.origin, previousSprite.scale , effects, LayerDepthManager.GameObjectDepth);
		
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
		currentSprite = new DrawArguments(spriteNode.Value.spriteRenderer, spriteRenderWidth);
		nextSprite = new DrawArguments(spriteNode.NextOrFirst().Value.spriteRenderer, spriteRenderWidth);
		previousSprite = new DrawArguments(spriteNode.PreviousOrLast().Value.spriteRenderer, spriteRenderWidth);
	}
	
	/// <summary>
	/// Animates the transition to a new sprite. Goes in the provided direction.
	/// </summary>
	/// <param name="spriteNode">The </param>
	/// <param name="isForward"></param>
	public void AnimateToSprite(LinkedListNode<Sprite> spriteNode, bool isForward)
	{
		Tween.TweenFloat(_displacementController);
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
	
	public void Dispose()
	{
		_displacementController.HandleFloatChange = null;
	}
}

