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
    /// A non-player sprite.
    /// </summary>
    class WordSearch_MouseSprite : TPSprite
    {
        
        /// <summary>
        /// A sprite that will hold the 1x1 White Pixel, used for accurate Collision Detection
        /// </summary>
        TPSprite mouse;

        /// <summary>
        /// The constructor sets the initial position of the sprite and loads the texture via the parent-class constructor.
        /// </summary>
        public WordSearch_MouseSprite(Vector2 pos)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/WordSearchPointer"))
        {
            Position = pos;
            mouse = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/1x1WhitePixel"));
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
            mouse.Position.X = Mouse.GetState().X;
            mouse.Position.X = Mouse.GetState().Y;

        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="batch">used to draw the sprite</param>
        public override void Draw(SpriteBatch batch)
        {
            mouse.Draw(batch);
            base.Draw(batch);
        } 
    }
}
