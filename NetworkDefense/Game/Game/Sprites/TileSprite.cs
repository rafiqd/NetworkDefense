/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Sprites
{
    /// <summary>
    /// TileSprite represents a single isometric tile in the game world.  The drawing aspect of the tile is used only for
    /// debugging, unless we end up implementing mouse controls, in which case only the tile we are pointing at will be rendered.
    /// </summary>
    public class TileSprite : IsometricSprite
    {
        /// <summary>
        /// The outlining frame of the tile
        /// </summary>
        Texture2D frameTexture;

        /// <summary>
        /// The previous tile in a breadth first search (used for path-finding)
        /// </summary>
        public TileSprite leader { set; get; }

        /// <summary>
        /// Whether or not the tile has been visited int he BFS
        /// </summary>
        public bool visited { set; get; }

        /// <summary>
        /// The constructor for TileSprite
        /// </summary>
        /// <param name="pos">The screen position of the sprite</param>
        /// <param name="sprite">The player sprite</param>
        /// <param name="mapPos">The grid position of the sprite</param>
        public TileSprite(Vector2 pos, PlayerSprite sprite, Point mapPos)
            : base(pos, "tilefull", sprite, mapPos)
        {
            frameTexture = TPEngine.Get().TextureManager.LoadTexture(@"art/alpha/tile");
            Position = pos;
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="batch">The sprite batch to draw to</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            batch.Draw(m_Texture, worldPosition + (playerSprite.Position - playerSprite.roomPosition), new Color(255, 255, 255, 100));
            batch.Draw(frameTexture, worldPosition + (playerSprite.Position - playerSprite.roomPosition), Color.White);
        }
    }
}
