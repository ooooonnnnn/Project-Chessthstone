using System;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Behaviors;

    public class PlayerStatsHUD : ToolTip
    {
        private int _mana;
        private bool _isWhite;
        

        public PlayerStatsHUD(bool isWhite, int initialMana, int width = 200, int padding = 50) 
            : base("24K gold Labubu" ,$"Mana: {initialMana}", width, padding)
        {
            _mana = initialMana;
            _isWhite = isWhite;
            // Subscribe to turn changes
            TurnManager.instance.OnTurnChanged += OnTurnChanged;
        }

        private void OnTurnChanged(bool isWhite)
        {
            if (isWhite == _isWhite)
            {
                // If it's the player's turn, set text color to white
                SetTextColor(Color.White);
            }
            else
            {
                // If it's not the player's turn, set text color to gray
                SetTextColor(Color.Gray);
            }
        }


        // Updates the mana text dynamically
        public void UpdateMana(int mana)
        {
            _mana = mana;
            Text = $"Mana: {_mana}";
        }

        // Respond to turn change events


        // Cleanup event subscription when destroyed
        ~PlayerStatsHUD()
        {
            TurnManager.instance.OnTurnChanged -= OnTurnChanged;
        }

        // Helper: expose text color from ToolTip
        private void SetTextColor(Color color)
        {
            if (_textRenderer != null)
                _textRenderer.color = color;
        }
    }