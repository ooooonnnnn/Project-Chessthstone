using System;
using System.Collections.Generic;
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
	/// <summary>
	/// The scene and it's gameobjects are loaded and active
	/// </summary>
	public bool isLoaded;
	
	private List<GameObject> _gameObjects;
	/// <summary>
	/// When setting this, the gameobjects are automatically linked to this scene (GameObject.parentScene field)
	/// </summary>
	public List<GameObject> gameObjects
	{
		get => _gameObjects;
		protected set
		{
			_gameObjects = value;
			foreach (var gameObject in _gameObjects)
			{
				gameObject.parentScene = this;
			}
		}
	}

	public GameObject AddGameObject(GameObject gameObject)
	{
		gameObject.parentScene = this;
		_gameObjects.Add(gameObject);
		if (isLoaded)
		{
			SceneManager.AddGameObject(gameObject);
		}
		return gameObject;
	}

	public void Dispose()
	{
		foreach (GameObject gameObject in gameObjects)
		{
			gameObject.Dispose();
		}
		
		gameObjects = null;
	}
}