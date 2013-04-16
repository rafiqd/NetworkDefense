using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Engine;
using Microsoft.Xna.Framework;
using Engine.Collision;
using Microsoft.Xna.Framework.Input;

namespace Game.Sprites
{
    /// <summary>
    /// A non-player sprite.
    /// </summary>
    class StudentSprite : TPSprite
    {
        //maximum movemoent
        int startPosX = 500;
        int startPosY = 500;
        int prepareTime = 6;
        int ticX = 25;
        int ticY = 10;
        /// <summary>
        /// The constructor sets the initial position of the sprite and loads the texture via the parent-class constructor.
        /// </summary>
        public StudentSprite()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/student"))
        {
            Position.Y = 400;
            Position.X = 2000;
            Scale = new Vector2(.5f, .5f);

        }

        /// <summary>
        /// Updates the sprite's position with each iteration.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (gameTime.TotalGameTime.Seconds == prepareTime)//Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Position.X = startPosX;
                Position.Y = startPosY;
            }
            else if (gameTime.TotalGameTime.Seconds > prepareTime
                && gameTime.TotalGameTime.Milliseconds % 800 == 0
                && Position.Y > 460)
            {
                Position.X += ticX;
                Position.Y -= ticY;
                Scale /= 1.2f;
            }
        }
    }
}
