using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;

namespace Engine.Collision
{
    /// <summary>
    /// TPCollider provides very simple texture-overlap collision that provides inaccurate collision detection.
    /// This can be used as a main collison detection method for low-resolution collisions (such as picking up items)
    /// or as a stepping-stone collision detection for high-resolution collisions (such as car collisions).
    /// </summary>
    public static class TPCollider
    {
        /// <summary>
        /// Test to see if the textures of two sprites overlap.
        /// </summary>
        /// <param name="s1">The first sprite.</param>
        /// <param name="s2">The second sprite</param>
        /// <returns>Whether or not the textures of the two sprites overlap (ie. they collide)</returns>
        public static bool Test(TPSprite s1, TPSprite s2)
        {
            return (GetRect(s1).Intersects(GetRect(s2)));
        }

        /// <summary>
        /// Returns a rectangle that describes the area of overlap of the textures.
        /// </summary>
        /// <param name="s1">The first sprite.</param>
        /// <param name="s2">The second sprite.</param>
        /// <returns>The rectangle describing the overlap area.</returns>
        public static Rectangle GetOverlap(TPSprite s1, TPSprite s2)
        {
            Rectangle A = GetRect(s1);
            Rectangle B = GetRect(s2);
            int top = Math.Max(A.Top, B.Top);
            int bottom = Math.Min(A.Bottom, B.Bottom);
            int left = Math.Max(A.Left, B.Left);
            int right = Math.Min(A.Right, B.Right);

            return new Rectangle(left, top, right - left, bottom - top);
        }

        /// <summary>
        /// Transforms the sprite's texture into a rectangle that encompassed the same area.
        /// </summary>
        /// <param name="s">The sprite.</param>
        /// <returns>The rectangle that the sprite occupies.</returns>
        public static Rectangle GetRect(TPSprite s)
        {
            return new Rectangle((int)s.Position.X, (int)s.Position.Y, (int)(s.Width * s.Scale.X), (int)(s.Height * s.Scale.Y));
        }

        public static bool PerPixelTest(TPSprite spriteA, Color[] dataA, TPSprite spriteB, Color[] dataB)
        {
            Rectangle overlap;
            // Find the bounds of the rectangle intersection
            overlap = TPCollider.GetOverlap(spriteA, spriteB);

            // Check every point within the intersection bounds
            for (int y = overlap.Top; y < overlap.Bottom; y++)
            {
                for (int x = overlap.Left; x < overlap.Right; x++)
                {
                    int xipos;
                    int yipos;
                    if (spriteA.Position.X < 0)
                    {
                        xipos = (int)(x - Math.Ceiling(spriteA.Position.X));
                    }
                    else
                    {
                        xipos = (int)(x - Math.Floor(spriteA.Position.X));
                    }
                    if (spriteA.Position.Y < 0)
                    {
                        yipos = (int)(y - Math.Ceiling(spriteA.Position.Y));
                    }
                    else
                    {
                        yipos = (int)(y - Math.Floor(spriteA.Position.Y));
                    }
                    int dataAIndex = (int)(xipos + (yipos * spriteA.Width));
                    int dataBIndex = (int)((x - Math.Floor(spriteB.Position.X)) + (y - Math.Floor(spriteB.Position.Y)) * spriteB.Width);
                    // Get the color of both pixels at this point
                    Color colorA = dataA[dataAIndex];
                    Color colorB = dataB[dataBIndex];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }
    }
}
