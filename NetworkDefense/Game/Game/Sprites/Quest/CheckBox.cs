using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Engine;
using Microsoft.Xna.Framework;
using Engine.Collision;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game.Sprites
{
    /// <summary>
    /// Cyril
    /// A checkbox sprite. ticked if the quest is done
    /// </summary>
    class CheckBox : TPSprite
    {
        /// <summary>
        /// constructor used to initialize the textbox image and its properties
        /// </summary>
        /// <param name="checkBox">the type of textbox to spawn</param>
        public CheckBox(string checkBox)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/Quest/" + checkBox))
        {
            Scale = new Vector2(1, 1);
        }

        /// <summary>
        /// Updates the sprite's position with each iteration.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Globals.Paused)
            {
                base.Update(gameTime);
            }
        }

        /// <summary>
        /// load the necessary items that needed to display on screen
        /// </summary>
        protected override void Load()
        {
            base.Load();
            AffectedByDrag = false;
        }
    }
}
