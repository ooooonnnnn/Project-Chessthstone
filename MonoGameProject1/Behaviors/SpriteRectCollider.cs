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
		
		//first construct the inverse transformation matrix
		if (_transform.scale.X == 0 || _transform.scale.Y == 0) return false;
		//TODO: maybe using `_sprite.scale.X == 1 ? 1 : 1/_sprite.scale.X` is faster when the scale equals 1
		float invScX = 1/_transform.scale.X;
		float invScY = 1/_transform.scale.Y;
		float posX = _transform.position.X;
		float posY = _transform.position.Y;
		float rot = _transform.rotation;
		float cos, sin;
		switch (rot % (2*PI))
		{
			case 0:
				cos = 1;
				sin = 0;
				break;
			case PI/2:
				cos = 0;
				sin = 1;
				break;
			case PI:
				cos = -1;
				sin = 0;
				break;
			case 3*PI/2:
				cos = 0;
				sin = -1;
				break;
			default:
				cos = MathF.Cos(rot);
				sin = MathF.Sin(rot);
				break;
		}
		Vector2 org = _transform.origin;

		Matrix3x2 invTrans = new Matrix3x2(
			cos * invScX                               , -sin * invScY,
			sin * invScY                               , cos * invScY,
			invScX * (-cos * posX - sin * posY) + org.X, invScY * (sin * posX - cos * posY) + org.Y);
		
		//now inverse trasnform the query point, putting into sprite space
		//change the point to the System.Numerics type TODO: this is a hacky solution
		System.Numerics.Vector2 pt = new System.Numerics.Vector2(point.X, point.Y); 
		pt = System.Numerics.Vector2.Transform(pt, invTrans);
		point = new Vector2(pt.X, pt.Y); //super hacky
		
		//now check that the point is within the bounds
		return point.X >= 0 && point.Y >= 0 && point.X < _sprite.width && point.Y < _sprite.height;
	}
}