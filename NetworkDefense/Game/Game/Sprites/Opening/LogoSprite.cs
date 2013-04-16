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
    /// A Logo sprite.
    /// </summary>
    class LogoSprite : TPSprite
    {
        /// <summary>
        /// Timer control
        /// </summary>
        int prepareTime = 2;
        /// <summary>
        /// variables to scale logo
        /// </summary>
        float oldLogoWidth;
        float newLogoWidth;
        /// <summary>
        /// scale rate to display
        /// </summary>
        float scaleRate;
        /// <summary>
        /// The constructor sets the initial position of the sprite and loads the texture via the parent-class constructor.
        /// </summary>
        public LogoSprite()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/openning/newlogo"))
        {
            Position.X = Width / 2;
            Position.Y = Height / 2;
            scaleRate = 0;
            Scale = new Vector2(scaleRate, scaleRate);
            oldLogoWidth = 500;

        }

        /// <summary>
        /// Updates the logo changed size and location.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (gameTime.TotalGameTime.Seconds == prepareTime)//Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Scale = new Vector2(0.1f, 0.1f);
            }
            else if (gameTime.TotalGameTime.Seconds > prepareTime
                && gameTime.TotalGameTime.Milliseconds % 100 == 0
                && Position.X < 640)
            {
                Scale *= 1.1f;
                newLogoWidth = oldLogoWidth * 0.9f;
                Position.X += ((oldLogoWidth - newLogoWidth) / 3);
            }
        }
    }
}
