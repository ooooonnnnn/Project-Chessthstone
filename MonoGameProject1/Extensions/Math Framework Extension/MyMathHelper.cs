using System;

namespace MonoGameProject1;

public static class MyMathHelper
{
	const float PI = Single.Pi;
	public static void CalcCosAndSin(float rot, out float cos, out float sin)
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