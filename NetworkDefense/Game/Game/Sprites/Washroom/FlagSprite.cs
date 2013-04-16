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
    class FlagSprite : TPSprite
    {
        /// <summary>
        /// Boolean variable to know dispayed or not
        /// </summary>
        public bool onScreen = false;

        /// <summary>
        /// The constructor sets the initial position and initial score.
        /// </summary>
        /// <param name="pos">The initial position to set</param>
        public FlagSprite()
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/flag"))
        {
            //countMove = 0;
            this.Alive = false;
            Position = new Vector2(-50, -50);

        }
    }
}
