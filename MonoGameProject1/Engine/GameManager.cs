using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameProject1;

public static class GameManager
{
	public static Game game;
	
	public static void ExitGame() => game.Exit();
	public static GraphicsDevice Graphics => game.GraphicsDevice;
}