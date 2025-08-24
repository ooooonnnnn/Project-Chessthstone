using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameProject1.Engine;
using MonoGameProject1.Scenes;
using MonoGameProject1.Settings;

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
		//_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		//_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		_graphics.PreferredBackBufferWidth = GraphicsSettings.windowSize.X;
		_graphics.PreferredBackBufferHeight = GraphicsSettings.windowSize.Y;
		_graphics.IsFullScreen = false;
		Window.IsBorderless = true;
		_graphics.ApplyChanges();
	}

	protected override void Initialize()
	{
		// TODO: Add your initialization logic here
		//Setup managers
		TextureManager.game = this;
		GameManager.game = this;
		FontManager.game = this;
		AudioClips.game = this;
		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		// TODO: use this.Content to load your game content here
		TextureManager.LoadTextures();
		FontManager.LoadFonts();
		AudioClips.LoadAudio();
		// End loading here
		
		SceneManager.ChangeScene(new MainMenuScene());
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
		GraphicsDevice.Clear(GraphicsSettings.backGroundColor);

		// TODO: Add your drawing code here
		_spriteBatch.Begin(SpriteSortMode.BackToFront);
		SceneManager.Draw(_spriteBatch);
		_spriteBatch.End();
		
		base.Draw(gameTime);
	}

	public static void Print(object obj) => Console.WriteLine(obj);
}