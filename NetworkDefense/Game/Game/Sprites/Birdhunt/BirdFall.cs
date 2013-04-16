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
    /// the sprite represents if the target is hit and fall down
    /// </summary>
    class BirdFall : TPSprite
    {
        /// <summary>
        /// keep track of which face the bird is facing when falling down
        /// </summary>
        public int face;

        /// <summary>
        /// the sprite represents the bird falling facing forward the screen
        /// </summary>
        public Texture2D fallUp;

        /// <summary>
        /// the sprite represents the bird falling facing the opposite side of the screen
        /// </summary>
        public Texture2D fallDown;

        /// <summary>
        /// the sprite represents the bird falling facing left
        /// </summary>
        public Texture2D fallLeft;

        /// <summary>
        /// the sprite represents the bird falling facing right
        /// </summary>
        public Texture2D fallRight;

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
        /// constructor initialize the textures to draw
        /// </summary>
        /// <param name="fallup">name of the image for bird falling facing forward</param>
        /// <param name="fallright">name of the image for bird falling facing right</param>
        /// <param name="falldown">name of the image for bird falling facing backward</param>
        /// <param name="fallleft">name of the image for bird falling facing left</param>
        public BirdFall(string fallup, string fallright, string falldown, string fallleft)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/" + fallup))
        {
            onScreen = false;
            fallUp = TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/" + fallup);
            fallDown = TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/" + falldown);
            fallLeft = TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/" + fallleft);
            fallRight = TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/" + fallright);
            Scale = new Vector2(.8f, .8f);
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
                if (gameTime.TotalGameTime.Milliseconds % 100 == 0)
                {
                    if (face == 0)
                    {
                        this.m_Texture = fallUp;
                        face = 1;
                    }
                    else if (face == 1)
                    {
                        this.m_Texture = fallRight;
                        face = 2;
                    }
                    else if (face == 2)
                    {
                        this.m_Texture = fallDown;
                        face = 3;
                    }
                    else
                    {
                        this.m_Texture = fallLeft;
                        face = 0;
                    }
                }
                if (Position.Y + m_Texture.Height > 600)
                {
                    onScreen = false;
                }
                Position.Y += 7;
            }
        }

        /// <summary>
        /// Draw function.
        /// </summary>
        /// <param name="batch">represents the sprites that will be drawn</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            if (onScreen)
            {
                batch.Draw(m_Texture, Position, null, drawColor, Rotation, RotationOrigin, Scale, Effect, 1.0f);
            }
        }

        /// <summary>
        /// set the initial position and properties of the sprite
        /// </summary>
        /// <param name="start">initial position of the sprite</param>
        public void SetSprite(Vector2 start)
        {
            Position = start;
            onScreen = true;
            firstRun = true;
        }
    }
}
