using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameProject1.Engine;
using MonoGameProject1.Scenes;

namespace MonoGameProject1;

public class Game1 : Game
{
	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;

	public Game1()
	{
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
		//Full screen
		// _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		// _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		// _graphics.IsFullScreen = false;  // Set to false for borderless
		// Window.IsBorderless = true;
		// _graphics.ApplyChanges();
	}

	protected override void Initialize()
	{
		// TODO: Add your initialization logic here
		//Setup managers
		TextureManager.game = this;
		GameManager.game = this;
		FontManager.game = this;
		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		// TODO: use this.Content to load your game content here
		TextureManager.LoadTextures();
		FontManager.LoadFonts();
		// End loading here
		
		SceneManager.ChangeScene(new TestBoardScene());
	}
	
	protected override void Update(GameTime gameTime)
	{
		if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
		    Keyboard.GetState().IsKeyDown(Keys.Escape))
			Exit();

		// TODO: Add your update logic here
		MouseInput.Update(gameTime);
		SceneManager.Update(gameTime);
		
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		// TODO: Add your drawing code here
		_spriteBatch.Begin();
		SceneManager.Draw(_spriteBatch);
		_spriteBatch.End();
		base.Draw(gameTime);
	}

	public static void Print(object obj) => Console.WriteLine(obj);
}