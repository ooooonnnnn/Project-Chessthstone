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
	private GameObject _logoSprite, _nineSliced;
	private Transform transform;

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
		var lightSquareTexture = Content.Load<Texture2D>("Images/LightSquare");
		var darkSquareTexture = Content.Load<Texture2D>("Images/DarkSquare");
		var chessBoard = new ChessBoard("MainBoard", lightSquareTexture, darkSquareTexture, 64);
		chessBoard.TryGetBehavior<Transform>().position = new Vector2(100, 100);


		_nineSliced = new GameObject("NineSliced");
		NineSliced nineSlicedBehavior = new NineSliced(_roundedSquare, 40, 58, 40, 58);
		transform = new Transform();
		_nineSliced.AddBehaviors([nineSlicedBehavior, transform]);
		transform.position = GraphicsDevice.Viewport.Bounds.Center.ToVector2();
		transform.origin = new Vector2(50, 50);
		
		_drawables.Add(_nineSliced);
		_updatables.Add(_nineSliced);
		_drawables.Add(chessBoard);
		_updatables.Add(chessBoard);

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
		
		//TEST
		KeyboardState state = Keyboard.GetState();
		if (state[Keys.Up] == KeyState.Down)
		{
			transform.scale.Y += 0.01f;
		}
		if (state[Keys.Down] == KeyState.Down)
		{
			transform.scale.Y -= 0.01f;
		}
		if (state[Keys.Right] == KeyState.Down)
		{
			transform.scale.X += 0.01f;
		}
		if (state[Keys.Left] == KeyState.Down)
		{
			transform.scale.X -= 0.01f;
		}
		if (state[Keys.Space] == KeyState.Down)
		{
			transform.rotation += 0.01f;
		}
		if (state[Keys.W] == KeyState.Down)
		{
			transform.position.Y -= 1f;
		}
		if (state[Keys.S] == KeyState.Down)
		{
			transform.position.Y += 1f;
		}
		if (state[Keys.A] == KeyState.Down)
		{
			transform.position.X -= 1f;
		}
		if (state[Keys.D] == KeyState.Down)
		{
			transform.position.X += 1f;
		}
		if (state[Keys.NumPad8] == KeyState.Down)
		{
			transform.origin.Y -= 0.5f;
		}
		if (state[Keys.NumPad2] == KeyState.Down)
		{
			transform.origin.Y += 0.5f;
		}
		if (state[Keys.NumPad4] == KeyState.Down)
		{
			transform.origin.X -= 0.5f;
		}
		if (state[Keys.NumPad6] == KeyState.Down)
		{
			transform.origin.X += 0.5f;
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