using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Game.Sprites
{
    /// <summary>
    /// Sprite that represents the mouse 
    /// </summary>
    class TDPointer: TPSprite
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pos">current position vector</param>
        public TDPointer(Vector2 pos)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/alpha/tdpointer"))
        {
            Position = pos;
        }

        /// <summary>
        /// Updates the mouse sprite
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public override void Update(GameTime gameTime)
        {
            Position.X = Mouse.GetState().X;
            Position.Y = Mouse.GetState().Y;
            base.Update(gameTime);
        }

    }
}
