using System;

namespace MonoGameProject1;

/// <summary>
/// Describes methods for adding and removing listeners (Actions) to the objects' callback
/// </summary>
public interface ICallback
{
	public void AddListener(Action listener);
	public void RemoveListener(Action listener);
}