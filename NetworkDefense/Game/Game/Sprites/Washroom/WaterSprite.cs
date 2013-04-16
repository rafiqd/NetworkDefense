using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Engine;
using Microsoft.Xna.Framework.Input;

namespace Game
{
    /// <summary>
    /// The player sprite.
    /// </summary>
    class WaterSprite : TPSprite
    {
        /// <summary>
        /// Boorean variable for remain water's display
        /// </summary>
        public bool onScreen = false;

        /// <summary>
        /// The constructor sets the initial position and initial score.
        /// </summary>
        /// <param name="pos">The initial position to set</param>
        public WaterSprite()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/waterAmount"))
        {
            this.Alive = false;
            Position = new Vector2(50, 100);
            Scale = new Vector2(1.0f, 1.5f); 

        }
    }
}
