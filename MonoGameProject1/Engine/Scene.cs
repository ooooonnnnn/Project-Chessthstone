using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = MonoGameProject1.IDrawable;

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
		private set
		{
			_gameObjects = value;
			if (value == null)
			{
				return;
			}
			foreach (var gameObject in _gameObjects)
			{
				gameObject.parentScene = this;
			}
		}
	}

	/// <summary>
	/// Adds the gameobjects and all of their children to the scene
	/// </summary>
	public void AddGameObjects(List<GameObject> gameObjects)
	{
		foreach (GameObject gameObject in gameObjects)
		{
			DoAddGameObject(gameObject);
			foreach (var child in gameObject.GetAllChildren())
			{
				DoAddGameObject(child);
			}
		}
	}

	private void DoAddGameObject(GameObject gameObject)
	{
		gameObject.parentScene = this;
		if (_gameObjects == null) _gameObjects = new();
		_gameObjects.Add(gameObject);
		if (isLoaded)
		{
			SceneManager.AddGameObject(gameObject);
		}
	}
	
	/// <summary>
	/// Do your initialization here. NOT IN THE CONSTRUCTOR
	/// </summary>
	public abstract void Initialize();

	/// <summary>
	/// Removes and disposes a gameobject and it's children
	/// </summary>
	/// <param name="parent">object to remove</param>
	public void RemoveGameObjectAndChildren(GameObject parent)
	{
		foreach (GameObject child in parent.GetAllChildren())
		{
			RemoveGameObject(child);
		}
		RemoveGameObject(parent);
	}

	/// <summary>
	/// Removes and disposes a gameobject.
	/// </summary>
	/// <param name="go"></param>
	/// <exception cref="Exception">if the scene hasn't set the _gameObjects field</exception>
	public void RemoveGameObject(GameObject go)
	{
		if (_gameObjects == null) 
			throw new Exception($"Tried to remove {go.name} from a scene {this} that has no gameobjects.");
		_gameObjects.Remove(go);
		go.Dispose();
		if (isLoaded) //remove it from the scene manager
		{
			SceneManager.RemoveGameObject(go);
		}
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