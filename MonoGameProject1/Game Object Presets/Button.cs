using System;
using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

/// <summary>
/// Button with text and hover tinting. 
/// </summary>
public class Button : GameObject, ICallback
{
	public string text;
	public Texture2D texture;
	public Transform transform;
	
	public SpriteRenderer spriteRenderer { get; private set; }
	public ChangeTintWhenHover hoverTinting;
	
	private Clickable _clickable;

	/// <summary>
	/// A button with the default texture of a rounded square. 9-sliced scaling.
	/// </summary>
	/// <param name="name"></param>
	public Button(string name, string text = ""): base(name)
	{
		this.text = text;
		texture = TextureManager.GetDefaultButtonTexture();
		spriteRenderer = new NineSliced(texture, 40, 58, 40, 58, 2f);
		AddMyBehaviors();
	}

	public Button(string name, Texture2D texture, string text = "") : base(name)
	{
		this.text = text;
		this.texture = texture;
		spriteRenderer = new SpriteRenderer(texture);
		AddMyBehaviors();
	}
	
	//TODO: add constructor with manual ninesliced 

	private void AddMyBehaviors()
	{
		transform = new Transform();
		hoverTinting = new ChangeTintWhenHover();
		_clickable = new Clickable();
		AddBehaviors(new List<Behavior>
		{
			transform,
			spriteRenderer,
			new SpriteRectCollider(),
			new SenseMouseHover(),
			hoverTinting,
			_clickable
		});
	}
	
	public void AddListener(Action listener)
	{
		_clickable.OnClick += listener;
	}

	public void RemoveListener(Action listener)
	{
		_clickable.OnClick -= listener;
	}
}