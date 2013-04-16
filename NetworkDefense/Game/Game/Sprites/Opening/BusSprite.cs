using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Engine;
using Microsoft.Xna.Framework;
using Engine.Collision;
using Microsoft.Xna.Framework.Input;
using Game.States;
using DBCommService;

namespace Game.Sprites
{
    /// <summary>
    /// A Bus sprite.
    /// </summary>
    class BusSprite : TPSprite
    {
        /// <summary>
        /// Bus speed for animation
        /// </summary>
        int moveSpeed = 5;
        /// <summary>
        /// Timer control
        /// </summary>
        int prepareTime = 2;
       
        /// <summary>
        /// The constructor sets the initial position of the sprite and loads the texture via the parent-class constructor.
        /// </summary>
        public BusSprite()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/openning/bus"))
        {
            Position.Y = 400;
            Position.X = 1300;
        }

        /// <summary>
        /// Updates the sprite's position with each iteration.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (gameTime.TotalGameTime.Seconds < prepareTime)//Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                return;
            }
            if (Position.X != 100)
                Position.X -= moveSpeed;
        }
    }
}
