using System;
using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGameProject1;

/// <summary>
/// Includes data to transform from object space to screen space
/// </summary>
public class Transform : Behavior
{
	public Vector2 position = Vector2.Zero;
	public float rotation = 0;
	public Vector2 origin = Vector2.Zero;
	public Vector2 scale = Vector2.One;
	public void SetScale(Vector2 scaleVec)
	{
		scale = scaleVec;
	}
	public void SetScale(float scaleFloat)
	{
		scale = new Vector2(scaleFloat, scaleFloat);
	}
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
		CosAndSin(rotation, out cos, out sin);
		float orgX = origin.X;
		float orgY = origin.Y;
		
		Matrix3x2 trans = new Matrix3x2(
			scX * cos, scX * sin,
			-scY * sin, scY * cos,
			-scX * orgX * cos + scY*orgY*sin + position.X, -scX * orgX * sin - scY*orgY*cos + position.Y);
		
		//change the point to the System.Numerics type TODO: this is a hacky solution
		System.Numerics.Vector2 pt = new System.Numerics.Vector2(localPos.X, localPos.Y); 
		pt = System.Numerics.Vector2.Transform(pt, trans);
		return new Vector2(pt.X, pt.Y); //super hacky
	}

	const float PI = Single.Pi;
	public Vector2 ToLocalSpace(Vector2 worldPos)
	{
		//apply the reverse transformations of the sprite to the point and the sprite, and check if the 
		//point is inside the rectangle of the sprite.
		
		//first construct the inverse transformation matrix
		if (scale.X == 0 || scale.Y == 0)
		{
			throw new DivideByZeroException("Can't transform to local space with a scale of 0");
		}
		//TODO: maybe using `scale.X == 1 ? 1 : 1/scale.X` is faster when the scale equals 1
		float invScX = 1/scale.X;
		float invScY = 1/scale.Y;
		float posX = position.X;
		float posY = position.Y;
		float rot = rotation;
		float cos, sin;
		CosAndSin(rot, out cos, out sin);
		Vector2 org = origin;

		Matrix3x2 invTrans = new Matrix3x2(
			cos * invScX                               , -sin * invScY,
			sin * invScY                               , cos * invScY,
			invScX * (-cos * posX - sin * posY) + org.X, invScY * (sin * posX - cos * posY) + org.Y);
		
		//now inverse trasnform the query point, putting into sprite space
		//change the point to the System.Numerics type TODO: this is a hacky solution
		System.Numerics.Vector2 pt = new System.Numerics.Vector2(worldPos.X, worldPos.Y); 
		pt = System.Numerics.Vector2.Transform(pt, invTrans);
		return new Vector2(pt.X, pt.Y); //super hacky
	}

	private static void CosAndSin(float rot, out float cos, out float sin)
	{
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
	}
}