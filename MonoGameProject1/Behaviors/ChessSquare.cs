
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Enums;

namespace MonoGameProject1;

/// <summary>
/// Represents a single square on the chess board
/// </summary>
public class ChessSquare : ClickableSprite, IChessPosition, IChessOccupiable, IChessHighlightable
{
    // IChessPosition implementation
    public int Row { get; private set; }
    public int Column { get; private set; }
    public Vector2 BoardPosition => new Vector2(Column, Row);
    public string AlgebraicNotation => ChessConstants.ToAlgebraicNotation(Row, Column);
    
    // IChessOccupiable implementation
    public IChessPiece OccupyingPiece { get; private set; }
    public bool IsOccupied => OccupyingPiece != null;
    
    // IChessHighlightable implementation
    public bool IsHighlighted { get; private set; }
    
    // Additional properties
    public bool IsLightSquare { get; private set; }
    public ChessHighlightedType CurrentHighlightType { get; private set; }
    
    private readonly SpriteRenderer _spriteRenderer;
    private readonly Color _originalColor;
    private readonly Dictionary<ChessHighlightedType, Color> _highlightColors;

    public ChessSquare(string name, Texture2D texture, int row, int column) 
        : base(name, texture)
    {
        Row = row;
        Column = column;
        IsLightSquare = ChessConstants.IsLightSquare(row, column);
        
        _spriteRenderer = TryGetBehavior<SpriteRenderer>();
        _originalColor = IsLightSquare ? ChessConstants.LightSquareColor : ChessConstants.DarkSquareColor;
        _spriteRenderer.color = _originalColor;
        
        // Initialize highlight colors using boxing for enum keys
        _highlightColors = new Dictionary<ChessHighlightedType, Color>
        {
            { ChessHighlightedType.Selected, ChessConstants.SelectedHighlight },
            { ChessHighlightedType.ValidMove, ChessConstants.ValidMoveHighlight },
            { ChessHighlightedType.Capture, ChessConstants.CaptureHighlight },
            { ChessHighlightedType.Check, ChessConstants.CheckHighlight }
        };
        
        CurrentHighlightType = ChessHighlightedType.None;
    }

    // IChessOccupiable implementation
    public void PlacePiece(IChessPiece piece)
    {
        // Remove piece from previous position if it exists
        if (OccupyingPiece != null)
        {
            OccupyingPiece.CurrentPosition = null;
        }
        
        OccupyingPiece = piece;
        if (piece != null)
        {
            piece.CurrentPosition = this; // Boxing the ChessSquare as IChessPosition
            
            // Update piece visual position if it's a GameObject
            if (piece is GameObject pieceObject)
            {
                var pieceTransform = pieceObject.TryGetBehavior<Transform>();
                if (pieceTransform != null)
                {
                    pieceTransform.position = transform.position;
                }
            }
        }
    }

    public IChessPiece RemovePiece()
    {
        var piece = OccupyingPiece;
        if (piece != null)
        {
            piece.CurrentPosition = null;
            OccupyingPiece = null;
        }
        return piece; // Returns interface reference (polymorphism)
    }

    // IChessHighlightable implementation
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