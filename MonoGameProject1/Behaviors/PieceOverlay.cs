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
    public Vector2 iconSpacing = new Vector2(32, 0); // Horizontal spacing between elements
    public Vector2 textOffset = new Vector2(16, 0);  // Offset of text from icon
    public Color textColor = Color.White;
    public float iconScale = 1.0f;

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
    /// <param name="piece"></param>
    public void SetChessPiece(ChessPiece piece)
    {
        //Set transform parent and relative position
        _transform.parent = piece.transform;
        _transform.parentSpacePos = Vector2.Zero;
        
        //Get stats and subscribe to events
        health = piece.health;
        damage = piece.baseDamage;
        actionPoints = piece.actionPoints;
        
        piece.OnBaseDamageChanged += d => damage = d;
        piece.OnHealthChanged += h => health = h;
        piece.OnActionPointsChanged += ap => actionPoints = ap;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Vector2 basePosition = _transform.worldSpacePos;
        
        // Draw health
        DrawStatWithIcon(spriteBatch, healthIcon, health.ToString(), basePosition);
        
        // Draw damage
        Vector2 damagePos = basePosition + iconSpacing * _transform.worldSpaceScale;
        DrawStatWithIcon(spriteBatch, damageIcon, damage.ToString(), damagePos);
        
        // Draw action points
        Vector2 actionPos = basePosition + iconSpacing * 2 * _transform.worldSpaceScale;
        DrawStatWithIcon(spriteBatch, actionPointsIcon, actionPoints.ToString(), actionPos);
    }

    private void DrawStatWithIcon(SpriteBatch spriteBatch, Texture2D icon, string text, Vector2 position)
    {
        // Draw icon
        spriteBatch.Draw(
            icon, 
            position, 
            null, 
            color, 
            _transform.rotation,
            _transform.origin, 
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
            textColor, 
            _transform.rotation, 
            Vector2.Zero, 
            _transform.worldSpaceScale, 
            effects, 
            layerDepth + 0.001f // Slightly higher layer to appear on top
        );
    }
}