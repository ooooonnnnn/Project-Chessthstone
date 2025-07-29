using System;
using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGameProject1;

	/// <summary>Collider that fits the rectangle of a sprite, including rotations, scale, etc... <br/>
	///Requires the SpriteRenderer and Transform behaviors</summary>
public class SpriteRectCollider : Collider
{
	private SpriteRenderer _sprite;
	
	const float PI = Single.Pi;
	
	public override void Initialize()
	{
		base.Initialize();
		_sprite = gameObject.TryGetBehavior<SpriteRenderer>();
	}

	public override bool PointOnCollider(Vector2 point)
	{
		//apply the reverse transformations of the sprite to the point and the sprite, and check if the 
		//point is inside the rectangle of the sprite.
		
		if (_transform.scale.X == 0 || _transform.scale.Y == 0) return false;
		point = _transform.ToLocalSpace(point);
		
		//now check that the point is within the bounds
		return point.X >= 0 && point.Y >= 0 && point.X < _sprite.width && point.Y < _sprite.height;
	}
}