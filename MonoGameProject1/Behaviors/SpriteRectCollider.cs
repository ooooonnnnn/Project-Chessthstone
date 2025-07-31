using System;
using System.Numerics;
using MonoGameProject1.Behaviors;
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
		
		Vector2? localSpacePtNullable = _transform.ToLocalSpace(point);
		if (localSpacePtNullable == null) return false;
		Vector2 localSpacePt = localSpacePtNullable.Value;
		
		//now check that the point is within the bounds
		bool result = localSpacePt.X >= 0 && localSpacePt.Y >= 0 && localSpacePt.X < _sprite.width && localSpacePt.Y < _sprite.height;
		
		return result;
	}
}