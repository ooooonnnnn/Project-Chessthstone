using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MonoGameProject1;

public static class AudioClips
{
    public static Game game;

    public static SoundEffect MoveSound{ get; private set; }
    public static SoundEffect HitSound{ get; private set; }
    public static SoundEffect DeathSound{ get; private set; }
    public static SoundEffect TeleportSound{ get; private set; }
    public static SoundEffect ClickSound{ get; private set; }
    public static SoundEffect AbilitySound{ get; private set; }
    public static Song BattlePhaseMusic{ get; private set; }

    public static void LoadAudio()
    {
        MoveSound = game.Content.Load<SoundEffect>("Sounds/chessMove");
        HitSound = game.Content.Load<SoundEffect>("Sounds/hitHurt");
        DeathSound = game.Content.Load<SoundEffect>("Sounds/chessDeath");
        TeleportSound = game.Content.Load<SoundEffect>("Sounds/chessTeleport");
        ClickSound = game.Content.Load<SoundEffect>("Sounds/blipSelect");
        AbilitySound = game.Content.Load<SoundEffect>("Sounds/powerUp1");
        BattlePhaseMusic = game.Content.Load<Song>("Sounds/8bit_Fight Against Evil");
    }
}