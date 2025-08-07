using System.Collections.Generic;
using System.Linq;

namespace MonoGameProject1;

public static class GameObjectExtensions
{
	/// <summary>
	/// Gets all children (children of children etc.) form all hierarchical behaviors of a GameObject <br/>
	/// Not including the given GameObject
	/// </summary>
	private static GameObject head = null; //top of the tree
	public static HashSet<GameObject> GetAllChildren(this GameObject gameObject)
	{
		HashSet<GameObject> result;
		if (head == null)
		{
			head = gameObject;
			result = new();
		}
		else
		{
			result = [gameObject];
		}
		
		
		IReadOnlyList<IHierarchy> hierarchicals = gameObject.hierarchicalBehaviors;
		foreach (var behavior in hierarchicals)
		{
			result.UnionWith(behavior.children.Aggregate(
				new HashSet<GameObject>(), (current, child) =>
				{
					current.UnionWith(child.GetAllChildren());
					return current;
				}));
		}

		if (head == gameObject)
		{
			head = null;
		}
		return result;
	}
}