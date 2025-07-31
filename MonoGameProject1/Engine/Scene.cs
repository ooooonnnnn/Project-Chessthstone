using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = MonoGameProject1.Content.IDrawable;
using IUpdateable = MonoGameProject1.Content.IUpdateable;

namespace MonoGameProject1;

/// <summary>
/// Base class for scenes. Has an array of GameObjects which get added to the game loop by the scene manager.<br/>
/// When inheriting from this class, make sure all GameObjects you create are in gameObjects, otherwise they will not show. 
/// </summary>
public abstract class Scene : IDisposable
{
	public GameObject[] gameObjects { get; protected set; }
	
	public void Dispose()
	{
		foreach (GameObject gameObject in gameObjects)
		{
			gameObject.Dispose();
		}
		
		gameObjects = null;
	}
}