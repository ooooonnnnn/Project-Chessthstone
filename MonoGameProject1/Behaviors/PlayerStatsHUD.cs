using System;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Behaviors;

    public class PlayerStatsHUD : ToolTip
    {
        private int _mana => _player.mana;
        private Player _player;
        private bool isWhite => _player.isWhite;
        

        public PlayerStatsHUD(Player player) 
            : base("Player Stats HUD" ,"")
        {
            _player = player;
            // Subscribe to turn changes
            TurnManager.instance.OnTurnChanged +=_ => UpdateText();
        }

        public void UpdateText()
        {
            if (isWhite != TurnManager.instance.isWhiteTurn)
            {
                Text = "";
                return;
            }
            
            if (GamePhaseManager.instance.phase == GamePhase.Setup)
                Text = "Choose a piece to place";
            else if (GamePhaseManager.instance.phase == GamePhase.Gameplay)
                Text = $"Mana: {_mana}";
        }
    }