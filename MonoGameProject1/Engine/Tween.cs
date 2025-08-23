using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

public enum TweenType
{
    Linear,
    Exponential,
    CounterExponential,
    Smooth
}

public class FloatController
{
    public Action<float> HandleFloatChange = value => { };
}

public static class Tween
{
    public static async Task Move(Transform obj, Vector2 targetPosition, float duration,
        TweenType tweenType = TweenType.Linear, int fps = 60)
    {
        Vector2 startPosition = obj.parentSpacePos;
        float elapsed = 0f;
        duration = 1000 * duration;

        while (elapsed < duration)
        {
            elapsed += 1000f / fps;
            float weight = elapsed / duration;
            switch (tweenType)
            {
                case TweenType.Exponential:
                    weight *= weight;
                    break;
                case TweenType.CounterExponential:
                    weight /= weight;
                    break;
                case TweenType.Smooth:
                    weight = weight * weight * (3f - 2f * weight); // Smoothstep function
                    break;
                case TweenType.Linear:
                default:
                    break;
            }

            obj.parentSpacePos = Vector2.Lerp(startPosition, targetPosition, weight);
            await Task.Delay(TimeSpan.FromSeconds(1f / fps));
        }

        obj.parentSpacePos = targetPosition;
    }
    
    
    public static async Task TweenFloat(FloatController controller, TweenType tweenType = TweenType.Linear, float startingValue = 0f, float targetValue = 1f, float duration = 1f, int fps = 60)
    {
        float elapsed = 0f;
        duration = 1000 * duration;

        while (elapsed < duration)
        {
            elapsed += 1000f / fps;
            float weight = elapsed / duration;
            switch (tweenType)
            {
                case TweenType.Exponential:
                    weight *= weight;
                    break;
                case TweenType.CounterExponential:
                    weight /= weight;
                    break;
                case TweenType.Smooth:
                    weight = weight * weight * (3f - 2f * weight); // Smoothstep function
                    break;
                case TweenType.Linear:
                default:
                    break;
            }

            controller.HandleFloatChange(float.Lerp(startingValue, targetValue, weight));
            await Task.Delay(TimeSpan.FromSeconds(1f / fps));
        }

        controller.HandleFloatChange(targetValue);
    }

}