using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameProject1;

public class Game1 : Game
{
	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;
	
	private List<MonoGameProject1.Content.IUpdateable> _updatables = new();
	private List<MonoGameProject1.Content.IDrawable> _drawables = new();

	private Texture2D _logo, _roundedSquare;

	public Game1()
	{
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize()
	{
		// TODO: Add your initialization logic here
		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		// TODO: use this.Content to load your game content here
		_roundedSquare = Content.Load<Texture2D>("Images/RoundedFilledSquare");
		GameObject clickableSprite = new GameObject("Sprite");
		NineSliced nineSlicedBehavior = new NineSliced(_roundedSquare, 40, 58, 40, 58);
		Clickable clickable = new Clickable();
		clickable.OnClick += Exit;
		clickableSprite.AddBehaviors([nineSlicedBehavior, clickable, new Transform(), new SpriteRectCollider(), new ChangeTintWhenHover(), new SenseMouseHover()]);
		
		
		_drawables.Add(clickableSprite);
		_updatables.Add(clickableSprite);
	}
	
	protected override void Update(GameTime gameTime)
	{
		if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
		    Keyboard.GetState().IsKeyDown(Keys.Escape))
			Exit();

		// TODO: Add your update logic here
		MouseInput.Update(gameTime);
		foreach (var updateable in _updatables)
		{
			updateable.Update(gameTime);
		}
		
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		// TODO: Add your drawing code here
		_spriteBatch.Begin();
		foreach (var drawable in _drawables)
		{
			drawable.Draw(_spriteBatch);
		}
		_spriteBatch.End();
		base.Draw(gameTime);
	}

	public static void Print(object obj) => Console.WriteLine(obj);
}