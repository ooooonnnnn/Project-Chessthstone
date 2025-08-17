using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MonoGameProject1;

public static class AudioClips
{
    public static Game game;

    public static SoundEffect HitSound{ get; private set; }
    public static SoundEffect DeathSound{ get; private set; }

    public static void LoadAudio()
    {
        HitSound = game.Content.Load<SoundEffect>("Sounds/hitHurt");
        DeathSound = game.Content.Load<SoundEffect>("Sounds/chessDeath");
    }
}