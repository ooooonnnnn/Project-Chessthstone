using System;
using System.Collections.Generic;

namespace MonoGameProject1;

/// <summary>
/// A gameobject that is also a singleton. 
/// </summary>
public class SingletonGameObject<T> : GameObject
	where T : SingletonGameObject<T>
{
	public static T instance { get; protected set; }

	/// <summary>
	/// Creates the instance of the singleton.
	/// </summary>
	/// <returns>True if a new instance was created. False if it already existed</returns>
	public static bool Instantiate(string name, List<Behavior> behaviors = null)
	{
		if (instance != null)
			return false;

		instance = (T)(new SingletonGameObject<T>(name, behaviors));
		return true;
	}
	
	protected SingletonGameObject(string name, List<Behavior> behaviors = null) : base(name, behaviors) { }
	
	/// <summary>
	/// Clears the instance so a new one can be created
	/// </summary>
	public static void Destroy() => instance = null;
}