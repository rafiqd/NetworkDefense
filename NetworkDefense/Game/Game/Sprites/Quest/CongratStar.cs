using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Engine.Objects;
using Microsoft.Xna.Framework;

namespace Game.Sprites
{
    /// <summary>
    /// Cyril
    /// a group of sprites to congratulate the player when he completes a request
    /// </summary>
    class CongratStar : TPSprite
    {
        /// <summary>
        /// the directions for each of the individual sprite
        /// </summary>
        List<Vector2> directions = new List<Vector2>();

        /// <summary>
        /// the positions for each of the individual sprite
        /// </summary>
        List<Vector2> positions = new List<Vector2>();

        /// <summary>
        /// indicate if the sprite is on screen or not
        /// </summary>
        private bool onScreen;

        /// <summary>
        /// the start time of the program
        /// </summary>
        int startTime;

        /// <summary>
        /// the flag to indicate if the sprite is just loaded
        /// </summary>
        private bool firstRun;

        /// <summary>
        /// the scale factor of the sprite 
        /// </summary>
        float scaleFactor = 0.005f;

        /// <summary>
        /// constructor to initialize the image and the properties of the sprite
        /// </summary>
        public CongratStar()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/Quest/star"))
        {
            onScreen = false;
            directions.Add(new Vector2(50, 50));
            directions.Add(new Vector2(-75, -75));
            directions.Add(new Vector2(25, 20));
            directions.Add(new Vector2(30, 40));
            directions.Add(new Vector2(-60, -60));
            directions.Add(new Vector2(40, 40));
            directions.Add(new Vector2(-20, 30));
            Scale = new Vector2(scaleFactor, scaleFactor);
        }

        /// <summary>
        /// Updates the sprite's position with each iteration.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (onScreen)
            {
                if (firstRun)
                {
                    startTime = gameTime.TotalGameTime.Milliseconds;
                    firstRun = false;
                }

                for (int i = 0; i < positions.Count; i++)
                {
                    positions[i] += directions[i];
                    Rotation += 0.01f;
                    scaleFactor = scaleFactor - 0.0002f;
                    this.Scale = new Vector2(scaleFactor, scaleFactor);
                }

                if (gameTime.TotalGameTime.Milliseconds > startTime + 4)
                {
                    onScreen = false;
                }
            }
        }
        /// <summary>
        /// Draw function.
        /// </summary>
        /// <param name="batch">all the sprite items that will be drawn on the screen</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            if (onScreen)
            {
                //foreach (Vector2 pos in positions)
                //{
                    batch.Draw(m_Texture, positions[0], null, drawColor, -Rotation, RotationOrigin, Scale, Effect, 1.0f);
                    batch.Draw(m_Texture, positions[1], null, drawColor, Rotation, RotationOrigin, Scale, Effect, 1.0f);
                    batch.Draw(m_Texture, positions[2], null, drawColor, -Rotation, RotationOrigin, Scale, Effect, 1.0f);
                    batch.Draw(m_Texture, positions[3], null, drawColor, Rotation, RotationOrigin, Scale, Effect, 1.0f);
                    batch.Draw(m_Texture, positions[4], null, drawColor, -Rotation, RotationOrigin, Scale, Effect, 1.0f);
                    batch.Draw(m_Texture, positions[5], null, drawColor, Rotation, RotationOrigin, Scale, Effect, 1.0f);
                    batch.Draw(m_Texture, positions[6], null, drawColor, -Rotation, RotationOrigin, Scale, Effect, 1.0f);
                //}
            }
        }

        /// <summary>
        /// set the initial position where the sprites group will spawn
        /// </summary>
        /// <param name="start">the initial position for all the sprites</param>
        public void SetStartPos(Vector2 start)
        {
            positions.Clear();
            onScreen = true;
            firstRun = true;
            positions.Add(new Vector2(900, 300));
            positions.Add(new Vector2(600, 600));
            positions.Add(new Vector2(400, 600));
            positions.Add(new Vector2(800, 200));
            positions.Add(new Vector2(300, 600));
            positions.Add(new Vector2(400, 400));
            positions.Add(new Vector2(800, 100));
        }
    }
}
