using System;
using System.Threading.Tasks;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// Adds a cooldown to a button. Can only be used on Button game objects
/// </summary>
public class ButtonCooldownOnClick(int cooldownMs = 2000) : Behavior
{
	public int cooldownMs = cooldownMs;
	
	public override void Initialize()
	{
		if (gameObject is not Button button)
			throw new InvalidOperationException(
				$"ButtonCooldownOnClick can only be on Buttons and {gameObject.name} is not");
		
		button.AddListener(() => Cooldown(button));
	}
	
	private async Task Cooldown(Button btn)
	{
		btn.SetClickable(false);
		await Task.Delay(2000);
		btn.SetClickable(true);
	}
}