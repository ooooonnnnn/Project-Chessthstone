using System;

namespace MonoGameProject1;

public static class QuickRandom
{
	private static Random random = new();

	public static int NextInt(int minInclusive, int maxExclusive) => random.Next(minInclusive, maxExclusive);
	
	public static float NextFloat(float minInclusive, float maxExclusive) => 
		(float)random.NextDouble() * (maxExclusive - minInclusive) + minInclusive;
}