using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Engine;

namespace Game.Sprites
{
    /// <summary>
    /// Peter
    /// The bullet hole class. represent where the player shoots
    /// </summary>
    class BulletHole : TPSprite
    {
        /// <summary>
        /// Initialize the initial properties of the sprite
        /// </summary>
        public BulletHole()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/bullethole"))
        {
            Scale = new Microsoft.Xna.Framework.Vector2(.2f, .2f);
        }

        /// <summary>
        /// Updates the sprite's state.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running.</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (gameTime.TotalGameTime.Milliseconds % 2000 == 0)
            {
                Alive = false;
            }
        }
    }
}
