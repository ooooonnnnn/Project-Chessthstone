using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Engine;
using IUpdateable = MonoGameProject1.Content.IUpdateable;

namespace MonoGameProject1.Behaviors;
/// <summary>
/// Handles animations. Requires SpriteRenderer behavior.
/// The constructor takes a dictionary of animations, where the key is the animation name and the value is a SpriteSheet.
/// </summary>
public class Animation : Behavior, IUpdateable, IStart
{
    private Texture2D originalSprite;
    /// <summary>
    /// A dictionary of all animations for this GameObject.
    /// </summary>
    public Dictionary<string, SpriteSheet> Animations;
    
    private double frameTimer = 0.0;
    public int fps = 10;
    private int currentFrame = 0;
    
    public bool isAnimating { get; private set; } = false;
    public bool isLooping;
    private bool playOnStart = false;
    
    private SpriteSheet ActiveAnimationSheet;
    
    private string _activeAnimation;
    public string ActiveAnimation
    {
        get => _activeAnimation;
        set
        {
            _activeAnimation = Animations.ContainsKey(value) ? value : "default";
            ActiveAnimationSheet = Animations[ActiveAnimation];
        }
    }
    
    private SpriteRenderer spriteRenderer;
    /// <summary>
    /// Handles animations. Requires SpriteRenderer behavior.
    /// </summary>
    /// <param name="animations">string name, SpriteSheet.</param>
    /// <param name="isLooping">false = animation plays once.</param>
    /// <param name="playOnStart">true = Animation automatically plays when the game starts.</param>
    /// <param name="fps">Frames per second.</param>
    public Animation(Dictionary<string, SpriteSheet> animations, bool isLooping = false, bool playOnStart = false, int fps = 10)
    {
        Animations = animations;
        
        this.playOnStart = playOnStart;
        this.isLooping = isLooping;
        this.fps = fps;
    }

    public override void Initialize()
    {
        spriteRenderer = gameObject.TryGetBehavior<SpriteRenderer>();
        originalSprite = spriteRenderer.texture;
        
        if (!Animations.ContainsKey("default"))
            Animations.Add("default", new SpriteSheet(originalSprite, 1, 1));
        ActiveAnimation = "default";
    }

    /// <summary>
    /// if isLooping is false, the animation will stop after playing once.
    /// </summary>
    public void StartAnimation()
    {
        isAnimating = true;
    }

    public void PauseAnimation()
    {
        isAnimating = false;
    }
    
    public void StopAnimation()
    {
        currentFrame = 0;
        PauseAnimation();
    }

    private bool ShouldGetNextFrame(GameTime gameTime)
    {
        frameTimer += gameTime.ElapsedGameTime.TotalSeconds;

        if (frameTimer > 1.0 / fps)
        {
            frameTimer = 0.0;
            return true;
        }
        
        return false;
    }
    
    public void Update(GameTime gameTime)
    {
        if (!isAnimating) return;
        
        if (spriteRenderer.texture != ActiveAnimationSheet.SourceTexture)
            spriteRenderer.texture = ActiveAnimationSheet.SourceTexture;
        
        if (ShouldGetNextFrame(gameTime))
        {
            currentFrame++;
            if (currentFrame >= ActiveAnimationSheet.Rectangles.Length)
            {
                currentFrame = 0;
                if (!isLooping)
                {
                    isAnimating = false;
                    currentFrame = ActiveAnimationSheet.Rectangles.Length - 1; // Stay on the last frame
                }
            }
        }
        spriteRenderer.sourceRectangle = ActiveAnimationSheet.Rectangles[currentFrame];
    }

    public void Start()
    {
        if (playOnStart)
            StartAnimation();
    }
}