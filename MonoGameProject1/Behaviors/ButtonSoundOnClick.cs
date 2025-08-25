using System;

namespace MonoGameProject1.Behaviors;

public class ButtonSoundOnClick : Behavior
{
	public override void Initialize()
	{
		Button button = gameObject as Button;
		if (button == null)
			throw new Exception("ButtonSoundOnClick must be on a button");
		button.AddListener(PlayClickSound);
	}
	
	private void PlayClickSound()
	{
		AudioManager.PlaySound(AudioClips.ClickSound);
	}
}