using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine
{
    public static class TPMath
    {
        public static float GetRandomMirrorRange(float val)
        {
            return (float)(((TPEngine.Get().Rand.NextDouble() * val) * 2 )- (TPEngine.Get().Rand.NextDouble() * val)) ;
        }

        /// <summary>
        /// Returns true if the source is within the threshold distance from the target. 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static bool IsWithinRange(Vector2 source, Vector2 target, float threshold)
        {
            if ((Math.Abs(target.X - source.X) < threshold) && (Math.Abs(target.Y - source.Y) < threshold))
            {
                return true;
            }
            return false;


        }

        /// <summary>
        /// Gets a normal vector pointing in the direction of the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector2 GetDirectionVector(Vector2 source, Vector2 target)
        {
            Vector2 delta = new Vector2(target.X - source.X, target.Y - source.Y);
            TPMath.Normalize(ref delta);
            return delta;
        }

        /// <summary>
        /// Normalizes a vector. 
        /// </summary>
        /// <param name="vec"></param>
        public static void Normalize(ref Vector2 vec)
        {
            float denom = (float)Math.Sqrt((vec.X * vec.X) + (vec.Y * vec.Y));
            vec.X = vec.X / denom;
            vec.Y = vec.Y / denom;
        }

        public static void ShiftTowardsColor(ref Color color, ref Color target, byte scale = 1)
        {
            int r = 0;
            int g = 0;
            int b = 0;
            float a = color.A / 255f;
            if (color.R > target.R)
            {
                r -= (byte)(1 * scale);
            }
            if (color.R < target.R)
            {
                r += (byte)(1 * scale);
            }
            if (color.G > target.G)
            {
                g -= (byte)(1 * scale);
            }
            if (color.G < target.G)
            {
                g += (byte)(1 * scale);
            }
            if (color.B > target.B)
            {
                b -= (byte)(1 * scale);
            }
            if (color.B < target.B)
            {
                b += (byte)(1 * scale);
            }

            color = new Color(((color.R + r) / 255f) * a, ((color.G + g) / 255f) * a, ((color.B + b) / 255f) * a, a);

        }
    }
}
