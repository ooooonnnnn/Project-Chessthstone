using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Enums;
using IUpdateable = MonoGameProject1.Content.IUpdateable;
using IDrawable = MonoGameProject1.Content.IDrawable;

namespace MonoGameProject1;

/// <summary>
/// Main chess board implementation using composition and interface segregation
/// </summary>
public class ChessBoard : GameObject, IChessBoard
{
    // Private fields encapsulation
    private readonly ChessSquare[,] _squares;
    private readonly List<IChessPiece> _pieces;
    private readonly int _squareSize;
    private readonly Transform _transform;
    
    // Public properties
    public ChessSquare SelectedSquare { get; private set; }
    public IChessPiece SelectedPiece { get; private set; }

    public ChessBoard(string name, Texture2D lightSquareTexture, Texture2D darkSquareTexture, 
                     int squareSize = 64) : base(name, new List<Behavior> { new Transform() })
    {
        _squares = new ChessSquare[ChessProperties.boardSize, ChessProperties.boardSize];
        _pieces = new List<IChessPiece>();
        _squareSize = squareSize;
        _transform = TryGetBehavior<Transform>();
        
        InitializeBoard(lightSquareTexture, darkSquareTexture);
        SetupSquareCallbacks();
    }

    private void InitializeBoard(Texture2D lightTexture, Texture2D darkTexture)
    {
        for (int row = 0; row < ChessProperties.boardSize; row++)
        {
            for (int col = 0; col < ChessProperties.boardSize; col++)
            {
                bool isLightSquare = ChessProperties.IsLightSquare(row, col);
                Texture2D texture = isLightSquare ? lightTexture : darkTexture;
                
                var square = new ChessSquare($"Square_{row}_{col}", texture, row, col);
                square.transform.position = new Vector2(col * _squareSize, row * _squareSize);
                
                _squares[row, col] = square;
            }
        }
    }

    private void SetupSquareCallbacks()
    {
        // Use boxing with Action delegates for event handling
        for (int row = 0; row < ChessProperties.boardSize; row++)
        {
            for (int col = 0; col < ChessProperties.boardSize; col++)
            {
                var square = _squares[row, col];
                
                // Boxing the square reference in the closure
                square.AddListener(() => OnSquareClicked(square));
            }
        }
    }

    private void OnSquareClicked(ChessSquare clickedSquare)
    {
        // Polymorphic behavior based on game state
        if (SelectedPiece == null)
        {
            SelectSquare(clickedSquare);
        }
        else
        {
            AttemptMove(clickedSquare);
        }
    }

    private void SelectSquare(ChessSquare square)
    {
        // Clear previous selection
        ClearHighlights();
        
        if (square.IsOccupied)
        {
            SelectedSquare = square;
            SelectedPiece = square.OccupyingPiece; // Interface reference
            
            // Highlight selected piece and valid moves
            if (SelectedPiece is IChessHighlightable highlightablePiece)
            {
                highlightablePiece.SetHighlightType(ChessHighlightedType.Selected);
            }
            
            HighlightValidMoves(SelectedPiece);
        }
    }

    private void AttemptMove(ChessSquare targetSquare)
    {
        if (SelectedPiece?.MoveTo(targetSquare) == true)
        {
            Console.WriteLine($"Moved {SelectedPiece.Type} to {targetSquare.AlgebraicNotation}");
        }
        
        // Clear selection regardless of move success
        ClearSelection();
    }

    private void HighlightValidMoves(IChessPiece piece)
    {
        foreach (var position in piece.GetValidMoves())
        {
            // Unboxing interface to concrete type
            if (position is ChessSquare square)
            {
                var highlightType = square.IsOccupied ? 
                    ChessHighlightedType.Capture : ChessHighlightedType.ValidMove;
                square.SetHighlightType(highlightType);
            }
        }
    }

    private void ClearHighlights()
    {
        // Use LINQ with interface operations
        GetAllSquares()
            .Where(s => s.IsHighlighted)
            .ToList()
            .ForEach(s => s.SetHighlight(false));
    }

    private void ClearSelection()
    {
        if (SelectedPiece is IChessHighlightable highlightablePiece)
        {
            highlightablePiece.SetHighlight(false);
        }
        
        ClearHighlights();
        SelectedSquare = null;
        SelectedPiece = null;
    }

    // IChessBoard implementation using interface segregation
    public ChessSquare GetSquare(int row, int col)
    {
        if (!IsValidPosition(row, col)) return null;
        return _squares[row, col];
    }

    public ChessSquare GetSquare(Vector2 boardPosition)
    {
        return GetSquare((int)boardPosition.Y, (int)boardPosition.X);
    }

    public IEnumerable<ChessSquare> GetAllSquares()
    {
        // Iterator pattern with yield return
        for (int row = 0; row < ChessProperties.boardSize; row++)
        {
            for (int col = 0; col < ChessProperties.boardSize; col++)
            {
                yield return _squares[row, col];
            }
        }
    }

    public IEnumerable<IChessPiece> GetAllPieces()
    {
        return GetAllSquares()
            .Where(s => s.IsOccupied)
            .Select(s => s.OccupyingPiece); // Boxing to interface
    }
    

    public IEnumerable<IChessPiece> GetPieces(ChessPieceColor color)
    {
        return GetAllPieces().Where(p => p.Color == color); // Enum comparison with boxing
    }

    public Vector2 ScreenToBoardCoordinates(Vector2 screenPosition)
    {
        Vector2 localPosition = screenPosition - _transform.position;
        return new Vector2(
            (int)(localPosition.X / _squareSize),
            (int)(localPosition.Y / _squareSize)
        );
    }

    public bool IsValidPosition(int row, int col)
    {
        return row >= 0 && row < ChessProperties.boardSize && 
               col >= 0 && col < ChessProperties.boardSize;
    }

    // GameObject overrides
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        // Update all squares using polymorphism
        foreach (var square in GetAllSquares().Cast<IUpdateable>())
        {
            square.Update(gameTime);
        }
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        foreach (ChessSquare square in GetAllSquares().Cast<IDrawable>())
        {
            var worldPosition = _transform.position + 
                               new Vector2(square.column * _squareSize, square.row * _squareSize);
            
            // Custom draw method that respects world position
            DrawSquareAt(spriteBatch, square, worldPosition);
        }
    }


    private void DrawSquareAt(SpriteBatch spriteBatch, ChessSquare square, Vector2 position)
    {
        // Draw square
        var spriteRenderer = square.TryGetBehavior<SpriteRenderer>();
        spriteBatch.Draw(
            spriteRenderer._texture, 
            position, 
            null, 
            spriteRenderer.color, 
            0f, 
            Vector2.Zero, 
            Vector2.One, 
            SpriteEffects.None, 
            0f
        );

        // Draw piece if present
        if (square.IsOccupied && square.OccupyingPiece is GameObject pieceObject)
        {
            var pieceSpriteRenderer = pieceObject.TryGetBehavior<SpriteRenderer>();
            if (pieceSpriteRenderer != null)
            {
                spriteBatch.Draw(
                    pieceSpriteRenderer._texture,
                    position,
                    null,
                    pieceSpriteRenderer.color,
                    0f,
                    Vector2.Zero,
                    Vector2.One,
                    SpriteEffects.None,
                    0.1f // Layer above square
                );
            }
        }
    }
}