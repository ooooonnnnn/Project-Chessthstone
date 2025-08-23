using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public static class Tween
{
    /// <summary>
    /// Object moves at a constant speed from start to target.
    /// </summary>
    public static async Task MoveLinear(Transform obj, Vector2 targetPosition, float duration, int fps = 60)
    {
        Vector2 startPosition = obj.parentSpacePos;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += 1f / fps;
            float weight = elapsed / duration;
            obj.parentSpacePos = Vector2.Lerp(startPosition, targetPosition, weight);
            await Task.Delay(1 / fps * 1000);
        }

        obj.parentSpacePos = targetPosition;
    }
    
    /// <summary>
    /// Object starts moving slowly and accelerates towards the target.
    /// </summary>
    public static async Task MoveExponential(Transform obj, Vector2 targetPosition, float duration, int fps = 60)
    {
        Vector2 startPosition = obj.parentSpacePos;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += 1f / fps;
            float weight = elapsed / duration;
            weight = weight * weight;
            obj.parentSpacePos = Vector2.Lerp(startPosition, targetPosition, weight);
            await Task.Delay(1 / fps * 1000);
        }

        obj.parentSpacePos = targetPosition;
    }

    /// <summary>
    /// Object accelerates and decelerates smoothly.
    /// </summary>
    public static async Task MoveSmooth(Transform obj, Vector2 targetPosition, float duration, int fps = 60)
    {
        Vector2 startPosition = obj.parentSpacePos;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += 1f / fps;
            float weight = elapsed / duration;
            weight = weight * weight * (3f - 2f * weight); // Smoothstep function
            obj.parentSpacePos = Vector2.Lerp(startPosition, targetPosition, weight);
            await Task.Delay(1 / fps * 1000);
        }

        obj.parentSpacePos = targetPosition;
    }
}