using System.Collections.Generic;
using MonoGameProject1.Engine;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// Includes data to transform from object space to screen space
/// </summary>
public class Transform : Behavior, IHierarchy<Transform>
{
	/// <summary>
	/// The position used for the transformation to world space.<br/>
	/// Changing this value moves the children
	/// </summary>
	private Vector2 _worldSpacePos = Vector2.Zero;
	public Vector2 worldSpacePos { 
		get => _worldSpacePos;
		set
		{
			//Move this
			_worldSpacePos = value;
			
			//Move the children
			foreach (Transform child in _children)
			{
				child.worldSpacePos = ToWorldSpace(child._parentSpacePos);
			}
			
			UpdateParentSpacePosition(value);
		} }

	private void UpdateParentSpacePosition(Vector2 worldSpacePosition)
	{
		//Update this parentSpacePos
		_parentSpacePos = parent?.ToLocalSpace(worldSpacePosition) ?? worldSpacePosition;
	}

	/// <summary>
	/// Position of this transform's origin in parent space (or world space if there's no parent)
	/// </summary>
	private Vector2 _parentSpacePos = Vector2.Zero;
	public Vector2 parentSpacePos
	{
		get => _parentSpacePos;
		set
		{
			_parentSpacePos = value;
			worldSpacePos = parent?.ToWorldSpace(value) ?? value;
		}
	}
	//TODO: rotation and scale should be split to world space and parent space
	public float rotation = 0;
	public Vector2 origin = Vector2.Zero;
	public Vector2 scale = Vector2.One;
	public void SetScaleFromFloat(float scaleFloat)
	{
		scale = new Vector2(scaleFloat, scaleFloat);
	}

	//Hierarchy
	private Transform _parent;
	public Transform parent
	{
		get => _parent;
		set
		{
			if (_parent == value) return;
			_parent?.RemoveChild(gameObject);
			_parent = value;
			_parent?.AddChild(gameObject);
			UpdateParentSpacePosition(worldSpacePos);
		}
	}
	
	private List<Transform> _children = new List<Transform>();
	public IReadOnlyList<Transform> children => _children;
	
	public override void Initialize() { }

	/// <summary>
	/// Transforms a vector from the transform's local space to world space
	/// </summary>
	/// <param name="localPos"></param>
	/// <returns></returns>
	public Vector2 ToWorldSpace(Vector2 localPos)
	{
		float scX = scale.X;
		float scY = scale.Y;
		float cos;
		float sin;
		MyMathHelper.CalcCosAndSin(rotation, out cos, out sin);
		float orgX = origin.X;
		float orgY = origin.Y;

		Matrix2x3 trans = new Matrix2x3(
			scX * cos, -scY * sin, -scX * orgX * cos + scY * orgY * sin + worldSpacePos.X,
			scX * sin, scY * cos, -scX * orgX * sin - scY * orgY * cos + worldSpacePos.Y);
		
		return trans * localPos;
	}
	
	/// <summary>
	/// Transforms a vector from world space to the transform's local space.<br/>
	/// If the scale is 0 on at least one of the axes, returns null. 
	/// </summary>
	/// <param name="worldPos"></param>
	/// <returns></returns>
	public Vector2? ToLocalSpace(Vector2 worldPos)
	{
		//apply the reverse transformations of the sprite to the point and the sprite, and check if the 
		//point is inside the rectangle of the sprite.
		
		//first construct the inverse transformation matrix
		if (scale.X == 0 || scale.Y == 0)
		{
			return null;
		}
		//TODO: maybe using `scale.X == 1 ? 1 : 1/scale.X` is faster when the scale equals 1
		float invScX = 1/scale.X;
		float invScY = 1/scale.Y;
		float posX = worldSpacePos.X;
		float posY = worldSpacePos.Y;
		float rot = rotation;
		float cos, sin;
		MyMathHelper.CalcCosAndSin(rot, out cos, out sin);
		Vector2 org = origin;

		Matrix2x3 invTrans = new Matrix2x3(
			cos * invScX,  sin * invScY, invScX * (-cos * posX - sin * posY) + org.X,
			-sin * invScY, cos * invScY, invScY * (sin * posX - cos * posY) + org.Y
			);
		
		//now inverse trasnform the query point, putting into sprite space
		return invTrans * worldPos;
	}

	public void AddChild(GameObject child)
	{
		Transform transform = child.TryGetBehavior<Transform>();
		AddChild(transform);
	}

	public void AddChild(Transform childTransform)
	{
		_children.Add(childTransform);
		childTransform.parent = this;
	}
	
	public void RemoveChild(GameObject child)
	{
		Transform transform = child.TryGetBehavior<Transform>();
		RemoveChild(transform);
	}

	public void RemoveChild(Transform transform)
	{
		_children.Remove(transform);
		transform.parent = null;
	}
}