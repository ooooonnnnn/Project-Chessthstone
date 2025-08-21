using System.Collections.Generic;
using System.Linq;
using MonoGameProject1.Behaviors.Abstract;

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
		
		//TODO: add protection against loops in the tree. This probably requires a breadth first search
		IReadOnlyList<HierarchicalBehavior> hierarchicals = gameObject.hierarchicalBehaviors;
		foreach (var behavior in hierarchicals)
		{
			if (behavior.children.Count == 0 ) 
				continue;
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