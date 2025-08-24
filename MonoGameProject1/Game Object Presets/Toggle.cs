using System;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

/// <summary>
/// A button that can be toggled on and off. Don't use AddListener to react to on/off changes, Instead use OnToggled.
/// </summary>
public class Toggle : Button
{
	public event Action<bool> OnToggled;
	public bool isOn
	{
		get => _isOn;
		set
		{
			bool oldVal = _isOn;
			_isOn = value;
			HandleStateChanged(oldVal);
		}
	}
	public Color onTint = Color.White;
	public Color offTint = Color.Gray;
	public Color offHoverTint = Color.Gray * 1.2f;
	public Color onHoverTint = Color.LightGray;
	
	
	private bool _isOn = true;
	private bool _canBeSwitchedOff;
	
	/// <summary>
	/// Creates a toggle with the default nine sliced texture. 
	/// </summary>
	/// <param name="text">Optional text to appear on the button</param>
	/// <param name="canBeSwitchedOff">set to false to disallow toggling off by clicking</param>
	public Toggle(string name, string text = "", bool canBeSwitchedOff = true) : base(name, text)
	{
		_canBeSwitchedOff = canBeSwitchedOff;
		_clickable.RemoveAllListeners();
		_clickable.OnClick += HandleClick;
		UpdateGraphicsAndClickable();
	}

	private void HandleClick()
	{
		if (!_canBeSwitchedOff && _isOn)
			return;
		
		isOn = !isOn;
	}

	private void HandleStateChanged(bool oldState)
	{
		if (_isOn == oldState)
			return;

		UpdateGraphicsAndClickable();
		
		OnToggled?.Invoke(_isOn);
	}

	private void UpdateGraphicsAndClickable()
	{
		if (_isOn)
		{
			hoverTinting.originalTint = onTint;
			hoverTinting.tintWhenHover = onHoverTint;
			hoverTinting.tintWhenMouseDown = onTint;

			if (_canBeSwitchedOff)
				return;
			hoverTinting.SetActive(false);
			_clickable.SetActive(false);
		}
		else
		{
			hoverTinting.originalTint = offTint;
			hoverTinting.tintWhenHover = offHoverTint;
			hoverTinting.tintWhenMouseDown = offTint;

			if (_canBeSwitchedOff) 
				return;
			hoverTinting.SetActive(true);
			_clickable.SetActive(true);
		}
	}

	public override void Dispose()
	{
		base.Dispose();
		OnToggled = null;
	}
}