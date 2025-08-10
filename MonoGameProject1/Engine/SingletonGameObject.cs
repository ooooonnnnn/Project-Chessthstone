using System.Collections.Generic;

namespace MonoGameProject1;

/// <summary>
/// A gameobject that is also a singleton. 
/// </summary>
public class SingletonGameObject<T> : GameObject where T : SingletonGameObject<T>
{
	public SingletonGameObject(string name) : base(name){}

	public SingletonGameObject(string name, List<Behavior> behaviors) : base(name, behaviors){}
}