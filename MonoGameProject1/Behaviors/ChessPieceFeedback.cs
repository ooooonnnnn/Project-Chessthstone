using System;

namespace MonoGameProject1.Behaviors;

public class ChessPieceFeedback : Behavior
{
    public override void Initialize()
    {
        if (gameObject is not ChessPiece chessPiece)
        {
            throw new Exception("ChessPieceFeedback can only be used with a Sprite GameObject.");
        }

        chessPiece.OnHit += () => AudioManager.PlaySound(AudioClips.HitSound);
        chessPiece.OnDeath += _ => AudioManager.PlaySound(AudioClips.DeathSound);
    }
}