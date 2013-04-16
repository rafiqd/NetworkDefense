using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Engine;
using Microsoft.Xna.Framework.Input;

//Ting Fung (Kevin) Ng
//Wing Lim (William) Cheng

namespace Game
{
    /// <summary>
    /// The sprite represents the mouse cursor.
    /// </summary>
    class MousePointerSprite : TPSprite
    {
        /// <summary>
        /// The movement speed
        /// </summary>
        int MoveSpeed = 5;

        /// <summary>
        /// The constructor sets the initial position.
        /// </summary>
        /// <param name="pos">The initial position to set</param>
        public MousePointerSprite(Vector2 pos) : base(TPEngine.Get().TextureManager.LoadTexture(@"art/MousePointer"))
        {
            Position = pos;
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
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                Position.Y += MoveSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Position.Y -= MoveSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Position.X += MoveSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Position.X -= MoveSpeed;
            }

            Position.X = Mouse.GetState().X;
            Position.Y = Mouse.GetState().Y;
        }
    }
}
