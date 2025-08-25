using System;

namespace MonoGameProject1.Behaviors;

public class ToggleSoundOnToggle : Behavior
{
	public override void Initialize()
	{
		Toggle toggle = gameObject as Toggle;
		if (toggle == null)
			throw new Exception("ToggleSoundOnToggle must be on a toggle");
		toggle.OnClickAccepted += () => PlaySound();	
	}
	
	private void PlaySound()
	{
		AudioManager.PlaySound(AudioClips.ClickSound);
	}
}