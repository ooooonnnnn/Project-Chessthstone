using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameProject1;

/// <summary>
/// Handles scene creation and destruction
/// </summary>
public static class SceneManager
{
	private static List<Scene> _currentOpenScenes = new();
	private static List<GameObject> _gameObjects = new();
	private static Queue<GameObject> _gameObjectsToAdd = new();
	private static Queue<GameObject> _gameObjectsToRemove = new();
	
	/// <summary>
	/// Adds all objects in a scene without destroying the current one
	/// </summary>
	/// <param name="scene">the new scene to add</param>
	public static void AddScene(Scene scene)
	{
		if (scene.gameObjects == null)
		{
			throw new Exception($"{scene.GetType()} didn't define gameObjects.");
		}
		
		Console.WriteLine("Adding scene");
		scene.Initialize();
		
		foreach (GameObject gameObject in scene.gameObjects)
		{
			AddGoToAddQueue(gameObject);
		}
		
		_currentOpenScenes.Add(scene);
		scene.isLoaded = true;
	}

	/// <summary>
	/// Adds a GameObject to be drawn and updated. Waits until the end of Update/Draw to add it.
	/// </summary>
	/// <param name="gameObject"></param>
	public static void AddGameObject(GameObject gameObject)
	{
		if (gameObject.parentScene == null || !_currentOpenScenes.Contains(gameObject.parentScene))
		{
			throw new Exception($"{gameObject.name} is not part of an active scene " +
			                    $"(make sure to use Scene.AddGameObject to add a new GameObject");
		}
		
		_gameObjectsToAdd.Enqueue(gameObject);
	}

	/// <summary>
	/// Removes a gameobject from the game. Don't call this directly, because Scene calls this. <br/>Instead, do
	/// gameObject.parentScene.RemoveGameObject(gameObject)
	/// </summary>
	/// <param name="gameObject">The object to remove</param>
	public static void RemoveGameObject(GameObject gameObject)
	{
		_gameObjectsToRemove.Enqueue(gameObject);
	}

	/// <summary>
	/// Adds the gameObject to the add queue
	/// </summary>
	private static void AddGoToAddQueue(GameObject gameObject)
	{
		_gameObjectsToAdd.Enqueue(gameObject);
	}

	/// <summary>
	/// Removes a specific scene. Disposes of all GameObjects in the scene
	/// </summary>
	/// <param name="scene">the scene to remove</param>
	public static void RemoveScene(Scene scene)
	{
		Console.WriteLine("Removing scene");
		
		foreach (GameObject gameObject in scene.gameObjects)
		{
			_gameObjectsToRemove.Enqueue(gameObject);
		}

		scene.Dispose();
		
		_currentOpenScenes.Remove(scene);
		scene.isLoaded = false;
	}

	/// <summary>
	/// Removes all scenes and loads a new one
	/// </summary>
	/// <param name="scene">the scene to add</param>
	public static void ChangeScene(Scene scene)
	{
		foreach (Scene sceneToRemove in _currentOpenScenes.ToList())
		{
			RemoveScene(sceneToRemove);
		}
		
		AddScene(scene);
	}

	/// <summary>
	/// Updates all gameobjects
	/// </summary>
	public static void Update(GameTime gameTime)
	{
		foreach (GameObject gameObject in _gameObjects) gameObject.Update(gameTime);
		AddAndRemoveAllQueued();
	}

	/// <summary>
	/// Draws all GameObjects
	/// </summary>
	/// <param name="spriteBatch"></param>
	public static void Draw(SpriteBatch spriteBatch)
	{
		foreach (GameObject gameObject in _gameObjects) gameObject.Draw(spriteBatch);
		AddAndRemoveAllQueued();
	}

	/// <summary>
	/// Removes all objects in _gameObjectsToRemove.
	/// Then adds all objects in _gameObjectsToAdd that aren't in _gameObjects and Starts them.
	/// </summary>
	private static void AddAndRemoveAllQueued()
	{
		while (_gameObjectsToRemove.Count > 0)
		{
			GameObject gameObject = _gameObjectsToRemove.Dequeue();
			_gameObjects.Remove(gameObject);
		}
		
		while (_gameObjectsToAdd.Count > 0)
		{
			GameObject gameObject = _gameObjectsToAdd.Dequeue();
			if (_gameObjects.Contains(gameObject)) 
				continue;
			_gameObjects.Add(gameObject);
			gameObject.Start();
		}
	}
}