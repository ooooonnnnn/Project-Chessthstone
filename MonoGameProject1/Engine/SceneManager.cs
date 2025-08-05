using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameProject1;

/// <summary>
/// Handles scene creation and destruction
/// </summary>
//TODO: Add support for gameobjects that come with their own children. 
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
			_gameObjects.Add(gameObject);
			gameObject.Start();
		}
		
		_currentOpenScenes.Add(scene);
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