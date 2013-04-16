/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Engine;

namespace Game.Sprites
{
    /// <summary>
    /// IsometricSprite is the base sprite class for rendering in an isometric perspective for the main game
    /// </summary>
    public class IsometricSprite : TPSprite
    {    
        /// <summary>
        /// The position to draw the sprite in with respect to the player sprite
        /// </summary>
        public Vector2 worldPosition;

        /// <summary>
        /// The position of the sprite in the area
        /// </summary>
        public Point mapPosition = new Point();

        /// <summary>
        /// The player sprite
        /// </summary>
        public PlayerSprite playerSprite;

        /// <summary>
        /// An array of colors for the texture.  This array allows for pixel-perfect collisions.  This may not end up being used.
        /// </summary>
        public Color[] colors;

        /// <summary>
        /// Constructor for IsometricSprite
        /// </summary>
        /// <param name="worldPos">The world position</param>
        /// <param name="typeName">The name of the type of sprite, used to load the correct texture</param>
        /// <param name="sprite">The player sprite</param>
        /// <param name="mapPos">The isometric coordinates</param>
        public IsometricSprite(Vector2 worldPos, string typeName, PlayerSprite sprite, Point mapPos)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/alpha/" + typeName))
        {
            worldPosition = worldPos;
            playerSprite = sprite;
            mapPosition = mapPos;

            colors = new Color[Width * Height];
            GetTexture().GetData(colors);
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="batch">The sprite batch to draw to</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            // if ((Visible) && (m_Texture != null))
            if (Alive && m_Texture != null)
            {
                batch.Draw(m_Texture, worldPosition + (playerSprite.Position - playerSprite.roomPosition), null, drawColor, Rotation, RotationOrigin, Scale, Effect, 1.0f);
                //Render children. Interesting side effect is that the way this is implemented, children can have children and so on infinitely. 
            }
        }
    }
}
