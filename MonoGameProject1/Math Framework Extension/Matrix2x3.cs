using Microsoft.Xna.Framework;

namespace MonoGameProject1;

/// <summary>
/// a 2x3 matrix (like Matrix3x2 from system.numerics
/// </summary>
public struct Matrix2x3
{
	public float[,] values;
	public float m11 {get => values[0,0]; set => values[0,0] = value; }
	public float m12 {get => values[0,1]; set => values[0,1] = value; }
	public float m13 {get => values[0,2]; set => values[0,2] = value; }
	public float m21 {get => values[1,0]; set => values[1,0] = value; }
	public float m22 {get => values[1,1]; set => values[1,1] = value; }
	public float m23 {get => values[1,2]; set => values[1,2] = value; }

	public Matrix2x3(float m11, float m12, float m13, float m21, float m22, float m23)
	{
		values = new float[2, 3];
		values[0, 0] = m11;
		values[0, 1] = m12;
		values[0, 2] = m13;
		values[1, 0] = m21;
		values[1, 1] = m22;
		values[1, 2] = m23;
	}

	public Matrix2x3()
	{
		values = new float[2, 3];
	}

	public static Matrix2x3 operator *(Matrix2x3 a, Matrix2x3 b)
	{
		Matrix2x3 output = new Matrix2x3();
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				for (int k = 0; k < 2; k++)
				{
					output.values[i, j] += a.values[i, k] * b.values[k, j];
				}
				if (j == 2) output.values[i, j] += a.values[i, 2];
			}
		}
		return output;
	}

	public static Vector2 operator *(Matrix2x3 a, Vector2 b)
	{
		return new Vector2(a.m11 * b.X + a.m12 * b.Y + a.m13, a.m21 * b.X + a.m22 * b.Y + a.m23);
	}

	public override string ToString()
	{
		return $"{m11}, {m12}, {m13}\n{m21}, {m22}, {m23}";
	}
}