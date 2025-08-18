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
		
		foreach (GameObject gameObject in scene.gameObjects)
		{
			AddGameObjectNoDuplicates(gameObject);
		}
		
		_currentOpenScenes.Add(scene);
		scene.Initialize();
		scene.isLoaded = true;
	}

	/// <summary>
	/// Adds a GameObject to be drawn and updated
	/// </summary>
	/// <param name="gameObject"></param>
	public static void AddGameObject(GameObject gameObject)
	{
		if (gameObject.parentScene == null || !_currentOpenScenes.Contains(gameObject.parentScene))
		{
			throw new Exception($"{gameObject.name} is not part of an active scene " +
			                    $"(make sure to use Scene.AddGameObject to add a new GameObject");
		}
		
		AddGameObjectNoDuplicates(gameObject);
	}

	/// <summary>
	/// Removes a gameobject from the game. Don't call this directly, because Scene calls this. <br/>Instead, do
	/// gameObject.parentScene.RemoveGameObject(gameObject)
	/// </summary>
	/// <param name="gameObject">The object to remove</param>
	public static void RemoveGameObject(GameObject gameObject)
	{
		_gameObjects.Remove(gameObject);
	}

	/// <summary>
	/// Adds the gameObject if it's not in _gameObjects
	/// </summary>
	private static void AddGameObjectNoDuplicates(GameObject gameObject)
	{
		if (!_gameObjects.Contains(gameObject))
		{
			_gameObjects.Add(gameObject);
			gameObject.Start();
		}
	}

	/// <summary>
	/// Removes a specific scene. Disposes of all GameObjects in the scene
	/// </summary>
	/// <param name="scene">the scene to remove</param>
	public static void RemoveScene(Scene scene)
	{
		foreach (GameObject gameObject in scene.gameObjects)
		{
			_gameObjects.Remove(gameObject);
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
		Console.WriteLine("Changing scene");
		
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
	}

	/// <summary>
	/// Draws all GameObjects
	/// </summary>
	/// <param name="spriteBatch"></param>
	public static void Draw(SpriteBatch spriteBatch)
	{
		foreach (GameObject gameObject in _gameObjects) gameObject.Draw(spriteBatch);
	}
}