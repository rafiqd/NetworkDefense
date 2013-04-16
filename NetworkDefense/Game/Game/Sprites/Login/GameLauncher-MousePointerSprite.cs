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
    class GameLauncher_MousePointerSprite : TPSprite
    {
        /// <summary>
        /// texture used to hold the image of the mouse
        /// </summary>
        Texture2D mouse;

        /// <summary>
        /// The constructor sets the initial position of the sprite and loads the texture via the parent-class constructor.
        /// </summary>
        public GameLauncher_MousePointerSprite(Vector2 pos)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/launcher/borderPixel"))
        {
            Position = pos;
            mouse = TPEngine.Get().TextureManager.LoadTexture(@"art/launcher/mousePointer");
        }

        /// <summary>
        /// Updates the sprite.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running.</param>
        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Handle all input that directly relates to the sprite.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        private void HandleInput(GameTime gameTime)
        {

            Position.X = Mouse.GetState().X;
            Position.Y = Mouse.GetState().Y;

        }

        /// <summary>
        /// draws out the sprite
        /// </summary>
        /// <param name="batch">used to draw</param>
        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(mouse, Position, Color.White);
        }
    }
}
