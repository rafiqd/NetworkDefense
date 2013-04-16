﻿/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Engine.Objects;
using Game.States;
using Engine.StateManagement;

namespace Game.Sprites
{
    /// <summary>
    /// The bed is in the dorm room and allows the player to recuperate by sleeping.  Sleeping will skip the game forward
    /// by a specified amount of time and provide benefits according to the amount of time spent sleeping.
    /// </summary>
    class VendorSprite : UsableIsometricSprite
    {
        /// <summary>
        /// Constructor for VendorSprite
        /// </summary>
        /// <param name="worldPos">The position of the vendor in the world</param>
        /// <param name="sprite">The player sprite</param>
        /// <param name="mapPos">The position of the sprite on the map</param>
        /// <param name="areaName">The name of the area the sprite is in</param>
        public VendorSprite(Vector2 worldPos, PlayerSprite sprite, Microsoft.Xna.Framework.Point mapPos, string areaName)
            : base(worldPos, "vendor", sprite, mapPos, false, areaName)
        {
            special = true;
            usability = new TPString("Press E to shop.");
            usability.RenderColor = Color.Blue;
            usability.Scale *= 0.7f;
        }

        /// <summary>
        /// Updates the sprite according to the game time
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Update(GameTime gameTime)
        {
            // If its available and the player is close, show that its available with a message and a color change.
            if ((playerSprite.currentPosition.Y == mapPosition.Y && playerSprite.currentPosition.X == mapPosition.X + 1))
            {
                drawColor = new Color(0, 255, 0, 255);
                isAvailable = true;
                playerSprite.ActiveObject = this;
            }
            else
            {
                drawColor = Color.White;
                isAvailable = false;
            }
        }

        /// <summary>
        /// Gets a new VendorState when the vendor is activated
        /// </summary>
        /// <returns></returns>
        public override TPState GetNewState()
        {
            return new VendorState();
        }

        /// <summary>
        /// Draw draws the sprite
        /// </summary>
        /// <param name="batch">The sprite batch to draw to</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            if (Alive && m_Texture != null)
            {
                batch.Draw(m_Texture, worldPosition + (playerSprite.Position - playerSprite.roomPosition), null, drawColor, Rotation, RotationOrigin, Scale, Effect, 1.0f);
                //Render children. Interesting side effect is that the way this is implemented, children can have children and so on infinitely. 
            }
            if (isAvailable)
            {
                usability.Position = worldPosition + (playerSprite.Position - playerSprite.roomPosition) + new Vector2(200, 0);
                usability.Draw(batch);
            }
        }
    }
}