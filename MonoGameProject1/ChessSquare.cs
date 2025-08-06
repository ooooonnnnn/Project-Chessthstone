
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Enums;

namespace MonoGameProject1;

/// <summary>
/// Represents a single square on the chess board
/// </summary>
public class ChessSquare : ClickableSprite
{
    public int row { get; }
    public int column { get; }
    
    public ChessPiece OccupyingPiece { get; private set; }
    public bool IsOccupied => OccupyingPiece != null;
    
    // IChessHighlightable implementation
    public bool IsHighlighted { get; private set; }
    
    // Additional properties
    public bool IsLightSquare { get;}
    public ChessHighlightedType CurrentHighlightType { get; private set; }
    
    private readonly SpriteRenderer _spriteRenderer;
    private readonly Color _originalColor;
    private readonly Dictionary<ChessHighlightedType, Color> _highlightColors;

    //TODO: get texture from texture manager according to ChessProperties.IsLightSquare 
    public ChessSquare(string name, int row, int column) 
        : base(name, texture)
    {
        this.row = row;
        this.column = column;
        IsLightSquare = ChessProperties.IsLightSquare(row, column);
        
        _spriteRenderer = TryGetBehavior<SpriteRenderer>();
        _originalColor = IsLightSquare ? ChessProperties.LightSquareColor : ChessProperties.DarkSquareColor;
        _spriteRenderer.color = _originalColor;
        
        // Initialize highlight colors using boxing for enum keys
        _highlightColors = new Dictionary<ChessHighlightedType, Color>
        {
            { ChessHighlightedType.Selected, ChessProperties.SelectedHighlight },
            { ChessHighlightedType.ValidMove, ChessProperties.ValidMoveHighlight },
            { ChessHighlightedType.Capture, ChessProperties.CaptureHighlight },
            { ChessHighlightedType.Check, ChessProperties.CheckHighlight }
        };
        
        CurrentHighlightType = ChessHighlightedType.None;
    }

    /// <summary>
    /// Sets the occupying piece of the square
    /// </summary>
    /// <param name="piece">The piece to set on it</param>
    /// <exception cref="InvalidOperationException">If the square was occupied</exception>
    public void SetPiece(ChessPiece piece)
    {
        if (IsOccupied)
        {
            throw new InvalidOperationException(
                $"Can't place {piece.name} on square {row},{column} because it's already occupied");
        }
        
        OccupyingPiece = piece;
    }

    /// <summary>
    /// Removes and returns the occupying piece
    /// </summary>
    /// <returns>The removed occupying piece</returns>
    /// <exception cref="InvalidOperationException">If the quare was not occupied</exception>
    public ChessPiece RemovePiece()
    {
        if (!IsOccupied)
        {
            throw new InvalidOperationException(
                $"Can't remove piece from square {row},{column} because it's not occupied");
        }
        
        var piece = OccupyingPiece;
        OccupyingPiece = null;
        
        return piece;
    }

    public void SetHighlight(bool highlighted, Color? highlightColor = null)
    {
        IsHighlighted = highlighted;
        
        if (highlighted)
        {
            var color = highlightColor ?? _highlightColors[CurrentHighlightType];
            _spriteRenderer.color = color;
        }
        else
        {
            _spriteRenderer.color = _originalColor;
            CurrentHighlightType = ChessHighlightedType.None;
        }
    }

    public void SetHighlightType(ChessHighlightedType type)
    {
        throw new NotImplementedException();
    }

    public void SetHighlightedType(ChessHighlightedType type)
    {
        CurrentHighlightType = type;
        if (type != ChessHighlightedType.None)
        {
            SetHighlight(true, _highlightColors[type]);
        }
        else
        {
            SetHighlight(false);
        }
    }

    /// <summary>
    /// Check if this square is a valid target for the given piece
    /// Uses interface segregation and polymorphism
    /// </summary>
    public bool IsValidTargetFor(IChessPiece piece)
    {
        if (piece == null) return false;
        
        // Can't move to your own position
        if (piece.CurrentPosition == this) return false;
        
        // Can't capture your own pieces
        if (IsOccupied && OccupyingPiece.Color == piece.Color)
            return false;
            
        return piece.CanMoveTo(this); // Polymorphic call
    }

    /// <summary>
    /// Override Update to handle chess-specific logic
    /// </summary>
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        // Update piece position if there is one
        if (IsOccupied && OccupyingPiece is GameObject pieceObject)
        {
            var pieceTransform = pieceObject.TryGetBehavior<Transform>();
            if (pieceTransform != null)
            {
                // Smooth piece movement could be implemented here
                pieceTransform.position = transform.position;
            }
        }
    }
}