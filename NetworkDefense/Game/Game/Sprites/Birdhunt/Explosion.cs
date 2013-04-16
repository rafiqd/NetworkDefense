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
    /// Peter
    /// a group of sprites represents the explosion if the target is hit
    /// </summary>
    class Explosion : TPSprite
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
        /// indicate if the sprite is first time loaded
        /// </summary>
        private bool firstRun;

        /// <summary>
        /// constructor set the direction and scale for each of the sprite
        /// </summary>
        public Explosion()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/feather1"))
        {
            onScreen = false;
            directions.Add(new Vector2(1, 2));
            directions.Add(new Vector2(-1, 2));
            directions.Add(new Vector2(1, -2));
            directions.Add(new Vector2(3, 2));
            directions.Add(new Vector2(1, -3));
            directions.Add(new Vector2(4, 1));
            directions.Add(new Vector2(-2, 1));
            directions.Add(new Vector2(-3, -3));
            directions.Add(new Vector2(-2, -1));
            directions.Add(new Vector2(-4, 2));
            Scale = new Vector2(.05f, .05f);
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
                }

                if (gameTime.TotalGameTime.Milliseconds > startTime + 500)
                {
                    onScreen = false;
                }
            }
        }
        /// <summary>
        /// Draw function.
        /// </summary>
        /// <param name="batch">draw multiple sprites with the similar properties</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            if (onScreen)
            {
                foreach (Vector2 pos in positions)
                {
                    batch.Draw(m_Texture, pos, null, drawColor, Rotation, RotationOrigin, Scale, Effect, 1.0f);
                }
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
            positions.Add(start);
            positions.Add(start);
            positions.Add(start);
            positions.Add(start);
            positions.Add(start);
            positions.Add(start);
            positions.Add(start);
            positions.Add(start);
            positions.Add(start);
            positions.Add(start);
        }
    }
}
