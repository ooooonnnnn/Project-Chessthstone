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
                text = "";
                return;
            }
            
            if (GamePhaseManager.instance.phase == GamePhase.Setup)
                text = "Choose a piece to place";
            else if (GamePhaseManager.instance.phase == GamePhase.Gameplay)
                text = $"Mana: {_mana}";
        }

        public override void Start()
        {
            base.Start();
            _player.OnTeamPieceChosen += _ => text = "Choose spawn location";
        }
    }