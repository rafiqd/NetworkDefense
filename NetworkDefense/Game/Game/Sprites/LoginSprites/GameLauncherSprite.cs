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
    
    public enum ButtonStatus
    {
        Up,
        Down
    }
    
    /// <summary>
    /// The player sprite.
    /// </summary>
    class GameLauncherSprite : TPSprite
    {     
        /// <summary>
        /// The constructor sets the LauncherWindow.
        /// </summary>
        public GameLauncherSprite(Vector2 pos)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/launcher/Logo2"))
        {
            Position = pos;
        }

        /// <summary>
        /// Draws the sprite.
        /// </summary>
        /// <param name="spriteBatch">used to draw out the sprite.</param>
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }
    }
}
