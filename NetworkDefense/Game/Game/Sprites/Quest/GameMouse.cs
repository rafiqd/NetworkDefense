using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Engine;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game
{
    /// <summary>
    /// Cyril
    /// GameMouse sprite used in EmailDetailState and EmailViewState
    /// </summary>
    class GameMouse : TPSprite
    {

        /// <summary>
        /// The constructor to set the mouse texture
        /// </summary>
        /// <param name="pos">The initial position to set</param>
        public GameMouse(Vector2 pos)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/MousePointer"))
        {

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

    }
}
