using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Extensions;

public static class Extensions
{
	public static Vector2 ToVector2(this Point point) => new(point.X, point.Y);

	public static Point Origin(this Rectangle rect) => new(rect.X, rect.Y);

	public static T Random<T>(this IEnumerable<T> list)
	{
		int count = list.Count();
		return list.ToArray()[QuickRandom.NextInt(0, count)];
	}
}