using Microsoft.Xna.Framework;

namespace MonoGameProject1.Extensions;

public static class Extensions
{
	public static Vector2 ToVector2(this Point point) => new(point.X, point.Y);

	public static Point Origin(this Rectangle rect) => new(rect.X, rect.Y);
}