using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MonoGameProject1;

public static class AudioManager
{
    public static Game game;
    
    private static float _masterVolume = 1f;
    public static float MasterVolume
    {
        get => _masterVolume;
        set => _masterVolume = Math.Clamp(value, 0f, 1f);
    }
    private static float _randomPitchShift = 0.1f;

    public static float randomPitchShift
    {
        get => _randomPitchShift;
        set => _randomPitchShift = Math.Clamp(value, -1f, 1f);
    }

    public static void PlaySound(SoundEffect sound, float volume = 1f)
    {
        volume = Math.Clamp(volume, 0f, 1f);
        SoundEffectInstance soundInstance = sound.CreateInstance();
        if (soundInstance == null) return;
        soundInstance.Volume = MasterVolume * volume;
        soundInstance.Pitch = QuickRandom.NextFloat(-_randomPitchShift, _randomPitchShift);
        soundInstance.Play();
    }
}