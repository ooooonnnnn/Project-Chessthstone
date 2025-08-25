using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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
    private static float _musicVolume = 0.2f;
    public static float MusicVolume
    {
        get => _musicVolume;
        set => _musicVolume = Math.Clamp(value, 0f, 1f);
    }
    public static float randomPitchShift
    {
        get => _randomPitchShift;
        set => _randomPitchShift = Math.Clamp(value, -1f, 1f);
    }


    public static void PlaySound(SoundEffect sound)
    {
        PlaySound(sound, _masterVolume);
    }
    
    public static void PlaySound(SoundEffect sound, float volume)
    {
        volume = Math.Clamp(volume, 0f, 1f);
        SoundEffectInstance soundInstance = sound.CreateInstance();
        if (soundInstance == null) return;
        soundInstance.Volume = MasterVolume * volume;
        soundInstance.Pitch = QuickRandom.NextFloat(-_randomPitchShift, _randomPitchShift);
        soundInstance.Play();
    }

    public static void PlaySong(Song song)
    {
        PlaySong(song, _masterVolume * _musicVolume);
    }
    
    public static void PlaySong(Song song, float volume)
    {
        MediaPlayer.Play(song);
        MediaPlayer.Volume = volume;
        MediaPlayer.IsRepeating = true;
    }
}