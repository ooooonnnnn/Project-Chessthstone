using Microsoft.Xna.Framework;

namespace MonoGameProject1;

/// <summary>
/// Math functions I couldn't find in XNA
/// </summary>
public static class MyMathHelper
{
	/// <summary>
	/// Left handed transformation of a 2d vector using a 2x3 matrix (represents a 3x3 matrix with the last row as 0 0 1)
	/// </summary>
	/// <param name="vector">the vector to transform</param>
	/// <param name="matrix">the transformation matrix</param>
	/// <returns></returns>
	public static Vector2 LeftTransform2D(Vector2 vector, Matrix2x3 matrix)
	{
		Vector2 output = new Vector2();
		output.X = vector.X * matrix.m11 + vector.Y * matrix.m21 + matrix.m13;
		output.Y = vector.X * matrix.m21 + vector.Y * matrix.m22 + matrix.m23;
		return output;
	}
}