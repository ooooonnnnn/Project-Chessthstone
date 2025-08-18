using System;

namespace MonoGameProject1.Behaviors;

public class ChessPieceFeedback : Behavior
{
    public override void Initialize()
    {
        Console.WriteLine("CHESS PIECE FEEDBACK INITIALIZED");
        if (gameObject is not ChessPiece chessPiece)
        {
            throw new Exception("ChessPieceFeedback can only be used with a Sprite GameObject.");
        }

        chessPiece.OnGetHit += () =>
        {
            Console.WriteLine("I TRIED SO HARD TO PLAY THE SOUND");
            AudioManager.PlaySound(AudioClips.HitSound);
            Console.WriteLine("I PLAYED THE SOUND");
        };
        chessPiece.OnDeath += _ => AudioManager.PlaySound(AudioClips.DeathSound, 0.7f);
        chessPiece.OnMove += () => AudioManager.PlaySound(AudioClips.MoveSound);
        chessPiece.OnTeleport += () => AudioManager.PlaySound(AudioClips.TeleportSound, 0.7f);
    }
}