using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameProject1;
/// <summary>
/// Contains a sprite sheet with a source texture and the number of rows and columns.
/// </summary>
public class SpriteSheet
{
    /// <summary>
    /// Amount of sprites on the X axis
    /// </summary>
    public readonly int Columns;
    /// <summary>
    /// Amount of sprites on the Y axis
    /// </summary>
    public readonly int Rows;
    /// <summary>
    /// Source texture of the sprite sheet.
    /// </summary>
    public readonly Texture2D SourceTexture;
    /// <summary>
    /// Array of rectangles representing the frames of the sprite sheet.
    /// </summary>
    public readonly Rectangle[] Rectangles;

    public SpriteSheet(Texture2D sourceTexture, int rows, int columns)
    {
        SourceTexture = sourceTexture;
        Columns = columns;
        Rows = rows;
        Rectangles = new Rectangle[rows * columns];
        for (int i = 0; i < Rectangles.Length; i++)
        {
            Rectangles[i] = GetFrameRect(i);
        }
    }
    
    /// <summary>
    /// Get the (column, row) vector of a specific frame in the sprite sheet.
    /// </summary>
    public Vector2 GetFramePositionInSheetMatrix(int frame)
    {
        if (frame < 0 || frame >= Columns * Rows)
        {
            throw new ArgumentOutOfRangeException(nameof(frame), "Frame index is out of range.");
        }
        var column = frame % Columns;
        var row = frame / Columns;
        return new Vector2(column, row);
    }

    /// <summary>
    /// Get the rectangle of a specific frame in the sprite sheet.
    /// </summary>
    public Rectangle GetFrameRect(int frame)
    {
        Vector2 frameVector = GetFramePositionInSheetMatrix(frame);
        var frameWidth = SourceTexture.Width / Columns;
        var frameHeight = SourceTexture.Height / Rows;
        return new Rectangle(
            (int)frameVector.X * frameWidth,
            (int)frameVector.Y * frameHeight,
            frameWidth,
            frameHeight
        );
    }
}