using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Engine;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace Game.Sprites
{
    /// <summary>
    /// class that holds methods to draw lines
    /// </summary>
    public class WordSearch_LineBatch : TPSprite
    {
        /// <summary>
        /// Variable to hold the image for a line
        /// </summary>
        Texture2D lineTexture;

        /// <summary>
        /// The constructor sets the texture of the line.
        /// </summary>
        public WordSearch_LineBatch()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/1x1WhitePixel"))
        {
            lineTexture = TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/1x1WhitePixel");
        }

        /// <summary>
        /// Draw a line into a SpriteBatch
        /// </summary>
        /// <param name="spriteBatch">used to draw the line</param>
        /// <param name="color">The line color</param>
        /// <param name="startP">Start Point</param>
        /// <param name="endP">End Point</param>
        public void DrawLine(SpriteBatch spriteBatch, Color color, Vector2 startP, Vector2 endP)
        {
            DrawLine(spriteBatch, color, startP, endP, 0);
        }

        /// <summary>
        /// Draw a line into a SpriteBatch
        /// </summary>
        /// <param name="spriteBatch">used to draw the line</param>
        /// <param name="color">The line color</param>
        /// <param name="startP">Start Point</param>
        /// <param name="endP">End Point</param>
        /// <param name="Layer">Layer or Z position</param>
        public void DrawLine(SpriteBatch spriteBatch, Color color, Vector2 startP, Vector2 endP, float Layer)
        {
            float angle = (float)Math.Atan2(endP.Y - startP.Y, endP.X - startP.X);
            float length = (endP - startP).Length();

            spriteBatch.Draw(lineTexture, startP, null, color,
                       angle, Vector2.Zero, new Vector2(length, 2),
                       SpriteEffects.None, Layer);
        }
    }
}
