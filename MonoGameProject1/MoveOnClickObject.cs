using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameProject1;

public class MoveOnClickObject: ClickableSprite
{
    private bool isSelected = false;
    private Vector2 _clickOriginPosition;

    public MoveOnClickObject(string name, Texture2D texture, Rectangle sourceRectangle = default) : base(name, texture, sourceRectangle)
    {
        _clickOriginPosition = transform.position; // Store the original position
        
        var toggleSelection = ToggleSelection;
        var deselect = Deselect;
        AddLeftClickListener(toggleSelection);
        AddRightClickListener(deselect);
    }

    /// <summary>
    /// Toggle the selection state of the object. If already selected, remain at the current position.
    /// </summary>
    protected virtual void ToggleSelection()
    {
        if (isSelected)
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            _clickOriginPosition = mousePosition;
        }
        isSelected = !isSelected;
    }
    
    /// <summary>
    /// Undo the selection and return to the original click position.
    /// </summary>
    protected virtual void Deselect()
    {
        isSelected = false;
        transform.position = _clickOriginPosition; // Reset to original position
    }
    
    private void FollowMouse()
    {
        MouseState mouseState = Mouse.GetState();
        Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

        Vector2 direction = mousePosition - _clickOriginPosition;
        float distance = direction.Length();
        if (distance > 0)
        {
            direction.Normalize();
            float moveDistance = (float)System.Math.Sqrt(distance);
            transform.position = _clickOriginPosition + direction * moveDistance;
        }
        else
        {
            transform.position = _clickOriginPosition;
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (isSelected)
        {
            FollowMouse();
        }
        base.Update(gameTime);
    }
}