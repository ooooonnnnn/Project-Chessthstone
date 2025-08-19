using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// Displays a piece's health, damage, and action points with numbers and icons.
/// Should be attached to a child GameObject of the piece's transform.
/// This is a permanent overlay, not a tooltip.
/// </summary>
public class PieceOverlay : Renderer
{
    // Icon textures
    public Texture2D healthIcon;
    public Texture2D damageIcon;
    public Texture2D actionPointsIcon;
    
    // Font for numbers
    public SpriteFont font;
    
    // Values to display
    private int health;
    private int damage;
    private int actionPoints;
    
    // Layout properties
    public Vector2 iconSpacing = new Vector2(0, 32); // Vertical spacing between elements
    public Vector2 textOffset = new Vector2(0, 0);  // Offset of text from icon
    public Color textColor = Color.White;
    public Color underTextColor = Color.Black;
    public float iconScale = 0.35f;
    public float textScale = 4f;
    public float underTextScale = 4.3f;

    private ChessPiece parentPiece;

    public PieceOverlay(Texture2D healthIcon, Texture2D damageIcon, Texture2D actionPointsIcon, 
                       SpriteFont font)
    {
        this.healthIcon = healthIcon;
        this.damageIcon = damageIcon;
        this.actionPointsIcon = actionPointsIcon;
        this.font = font;
    }

    /// <summary>
    /// Sets the parent piece of this overlay and subscribes to the pieces' stat change events
    /// </summary>
    /// </summary>
    /// <param name="piece"></param>
    public void SetChessPiece(ChessPiece piece)
    {
        parentPiece = piece;
        
        //Set transform parent and relative position
        _transform.parent = piece.transform;
        
        // Get the piece's sprite renderer to calculate center offset in parent space
        var spriteRenderer = piece.TryGetBehavior<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Set the overlay's position to the center of the sprite in parent space
            _transform.parentSpacePos = new Vector2(
                spriteRenderer.sourceWidth * 0.5f,
                spriteRenderer.sourceHeight * 0.5f
            );
        }
        else
        {
            _transform.parentSpacePos = Vector2.Zero;
        }
        
        //Get stats and subscribe to events
        health = piece.Health;
        damage = piece.BaseDamage;
        actionPoints = piece.ActionPoints;
        
        //TODO: Events Disappeared!
        piece.OnBaseDamageChanged += d => damage = d;
        piece.OnHealthChanged += h => health = h;
        piece.OnActionPointsChanged += ap => actionPoints = ap;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        // The overlay's world position is at the center of the sprite
        Vector2 centerPosition = _transform.worldSpacePos;
        // Define positioning offsets from center
        float verticalOffset = -20f; // No horizontal offset for the center icon
        float horizontalSpacing = 120f; // Distance from center for left/right icons
        float verticalSpacing = -75f;   // Distance from center for top icon
        
        // Calculate positions relative to the sprite center
        Vector2 healthPos = centerPosition + new Vector2(0, -verticalSpacing) * _transform.worldSpaceScale; // Top
        Vector2 damagePos = centerPosition + new Vector2(-horizontalSpacing, verticalOffset) * _transform.worldSpaceScale; // Left
        Vector2 actionPos = centerPosition + new Vector2(horizontalSpacing, verticalOffset) * _transform.worldSpaceScale; // Right
        
        // Draw the icons at their respective positions
        DrawStatWithIcon(spriteBatch, healthIcon, health.ToString(), healthPos);
        DrawStatWithIcon(spriteBatch, damageIcon, damage.ToString(), damagePos);
        DrawStatWithIcon(spriteBatch, actionPointsIcon, actionPoints.ToString(), actionPos);
        }

    private void DrawStatWithIcon(SpriteBatch spriteBatch, Texture2D icon, string text, Vector2 position)
    {
        // Calculate the center of the icon as its origin
        Vector2 iconOrigin = new Vector2(icon.Width * 0.5f, icon.Height * 0.5f);
        
        // Draw icon with its center as origin
        spriteBatch.Draw(
            icon, 
            position, 
            null, 
            color, 
            _transform.rotation,
            iconOrigin, // Use icon's center as origin
            _transform.worldSpaceScale * iconScale, 
            effects, 
            layerDepth
        );
        
        // Draw text
        Vector2 textPosition = position + textOffset * _transform.worldSpaceScale;
        
        spriteBatch.DrawString(
            font, 
            text, 
            textPosition, 
            underTextColor, 
            _transform.rotation, 
            _transform.worldSpaceScale* underTextScale / 2, 
            _transform.worldSpaceScale* underTextScale, 
            effects, 
            layerDepth - 0.01f // Slightly higher layer to appear on top
        );
        
        spriteBatch.DrawString(
            font, 
            text, 
            textPosition, 
            textColor, 
            _transform.rotation, 
            _transform.worldSpaceScale* textScale / 2, 
            _transform.worldSpaceScale* textScale, 
            effects, 
            layerDepth - 0.02f // Slightly higher layer to appear on top
            
        );

    }
}