using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

/// <summary>
/// A player in the game.
/// </summary>
public class Player : GameObject
{
    /// <summary>
    /// Mana to pay for activated abilities
    /// </summary>
    public int mana
    {
        get => _mana;
        set
        {
            _mana = value;
            OnManaChanged?.Invoke(_mana);
            Console.WriteLine($"{name} has {_mana} mana");
        }
    }
    public event Action <int> OnManaChanged; 

    private int _mana;

    /// <summary>
    /// Color. White goes first
    /// </summary>
    public bool isWhite { get; }

    /// <summary>
    /// Is this player taking its turn rn?
    /// </summary>
    public bool isPlayerActive => TurnManager.instance.isWhiteTurn == isWhite;

    /// <summary>
    /// The pieces the player starts with and places when the game starts
    /// </summary>
    public List<ChessPiece> teamPieces;

    private ChessPiece _pieceToPlace;

    /// <summary>
    /// The pieces that are currently on the board
    /// </summary>
    private List<ChessPiece> _alivePieces { get; } = new();

    public IReadOnlyList<ChessPiece> pieces => _alivePieces;

    public ChessBoard board
    {
        get
        {
            if (_board == null)
                throw new Exception($"Board not set for {name}");
            return _board;
        }
        set => _board = value;
    }

    private ChessBoard _board;

    /// <summary>
    /// Handles what happens when a square is clicked: <br/>
    /// Nothing selected and empty square => Spawn a new piece. <br/>
    /// Piece selected and square is valid move => move <br/>
    /// Piece selected and square is valid attack => attack
    /// </summary>
    public async void HandleSquareClicked(ChessSquare square)
    {
        Console.WriteLine("Player.handleSquareClicked");
        if (GamePhaseManager.instance.phase == GamePhase.Setup)
        {
            //try placing a piece
            //pass the turn if successful
            if (TryPlacePiece(square))
                TurnManager.instance.ChangeTurn();
            return;
        }

        if (_selectedActivePiece == null) //no piece selected, try select
        {
            if (square.occupyingPiece == null)
                return;
            if (square.occupyingPiece.isWhite != isWhite) //piece doesn't belongs to this player => can't select it
                return;

            _selectedActivePiece = square.occupyingPiece;
            //Test: show all possible attacks
            foreach (Point move in _selectedActivePiece.GetAttackCoordList())
            {
                board.squares[move.X, move.Y].spriteRenderer.color = Color.Red;
            }

            //Test: show all possible moves
            foreach (Point move in _selectedActivePiece.GetMoveCoordList())
            {
                board.squares[move.X, move.Y].spriteRenderer.color = Color.Green;
            }
            
            square.spriteRenderer.color = Color.Yellow;
        }
        else //piece selected => move or attack or deselect
        {
            if (square.occupyingPiece == _selectedActivePiece)
            {
                DeselectAll();
            }
            else
            {
                Point squareCoords = new Point(square.column, square.row);
                if (_selectedActivePiece.GetMoveCoordList().Contains(squareCoords))
                {
                    await _selectedActivePiece.MoveToSquare(square);
                }
                else if (_selectedActivePiece.GetAttackCoordList().Contains(squareCoords))
                {
                    await _selectedActivePiece.AttackPieceOnSquare(square);
                }
                else
                {
                    Console.WriteLine("Square not in possible moves, try again");
                }
                
                if (!DoIHaveMovesLeft())
                {
                    TurnManager.instance.ChangeTurn();
                }
            }
        }
    }

    private bool DoIHaveMovesLeft()
    {
        foreach (var piece in _alivePieces)
        {
            if (piece.ActionPoints > 0)
                return true;
            if (piece.ability is not ActivatedAbility ability) continue;
            if (ability.manaCost <= mana)
                return true;
        }

        Console.WriteLine("The player has no moves left");
        return false;
    }

    private ChessPiece _selectedActivePiece;

    //TODO: this should probably be in another class like ChessBoard
    private bool TryPlacePiece(ChessSquare square)
    {
        if (_pieceToPlace == null)
            return false;
        //Can't place if the square is occupied
        if (square.occupyingPiece != null)
            return false;
        //check square is on my side
        bool onMySide = isWhite
            ? square.column >= ChessProperties.boardSize / 2
            : square.column < ChessProperties.boardSize / 2;
        if (!onMySide)
            return false;

        _alivePieces.Add(_pieceToPlace);
        teamPieces.Remove(_pieceToPlace);
        _pieceToPlace.OnDeath += pieceToRemove =>
        {
            Console.WriteLine($"Removing {pieceToRemove.name}");
            _alivePieces.Remove(pieceToRemove);
        };
        foreach (var child in _pieceToPlace.transform.children)
        {
            try
            {
                child.TryGetBehavior<PieceOverlay>();
                child.SetActive(true);
            }
            catch
            {
            }
        }

        _pieceToPlace.transform.SetScaleFromFloat(square.transform.worldSpaceScale.X);
        _pieceToPlace.TeleportToSquare(square);

        _pieceToPlace = null;

        return true;
    }

    /// <summary>
    /// A player in the game.
    /// </summary>
    public Player(string name, bool isWhite) : base(name)
    {
        this.isWhite = isWhite;
        TurnManager.instance.OnTurnChanged += isItMyTurn => DeselectAll();
    }

    /// <summary>
    /// Attempts to choose a piece from the team 
    /// </summary>
    public void TryChooseTeamPiece(ChessPiece piece)
    {
        if (isPlayerActive && GamePhaseManager.instance.phase == GamePhase.Setup)
        {
            if (teamPieces.Contains(piece))
            {
                _pieceToPlace = piece;
            }
        }
    }

    public void TryActivateAbility()
    {
        ActivatedAbility activatedAbility = _selectedActivePiece?.ability as ActivatedAbility;
        if (activatedAbility == null)
        {
            Console.WriteLine("No piece selected OR no ability");
            return;
        }

        if (mana < activatedAbility.manaCost)
        {
            Console.WriteLine("Not enough mana to activate ability");
            return;
        }

        mana -= activatedAbility.manaCost;
        activatedAbility.Activate(null);
        //Inform trigger manager
        TriggerManager.instance.UpdateStateAndTryTrigger(isWhite);
        DeselectAll();
    }

    public void DeselectAll()
    {
        _selectedActivePiece = null;
        foreach (ChessSquare square in board.squares)
        {
            square.spriteRenderer.color = Color.White;
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        OnManaChanged = null;
    }
}
