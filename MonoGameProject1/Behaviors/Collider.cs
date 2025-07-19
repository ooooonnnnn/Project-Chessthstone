using Microsoft.Xna.Framework;

namespace MonoGameProject1;

/// <summary>
/// Base clas for colliders. Requires Transform
/// </summary>
public abstract class Collider : Behavior
{
	public abstract bool PointOnCollider(Vector2 point);
	
	protected Transform _transform;

	public override void Initialize()
	{
		_transform = gameObject.TryGetBehavior<Transform>();
	}
}