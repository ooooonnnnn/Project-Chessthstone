using Microsoft.Xna.Framework;
using IUpdateable = MonoGameProject1.Content.IUpdateable;

namespace MonoGameProject1.Behaviors;

/// <summary>
/// Follows a target transform. Requires a transform
/// </summary>
public class FollowTransform(Transform followTarget, Vector2 offset) : Behavior, IUpdateable
{
	public Transform followTarget = followTarget;
	public Vector2 offset = offset;
	private Transform _transform;
	
	public override void Initialize()
	{
		_transform = gameObject.TryGetBehavior<Transform>();
	}
	
	public void Update(GameTime gameTime)
	{
		_transform.parentSpacePos = followTarget.worldSpacePos + offset;
	}

}