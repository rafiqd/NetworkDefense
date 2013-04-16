/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Engine.Objects;
using Engine.StateManagement;
using Game.Area;

namespace Game.Sprites
{
    /// <summary>
    /// UsableIsometricSprite is the base class for all isometric sprites that can be activated by the player
    /// </summary>
    public class UsableIsometricSprite : IsometricSprite
    {
        /// <summary>
        /// Determines if the sprite is available for usage
        /// </summary>
        public bool isAvailable { get; set; }

        /// <summary>
        /// Determines if the sprite faces left (so far all do).
        /// </summary>
        public bool facesLeft;

        /// <summary>
        /// Determines if the sprite has special functionality, and can then override other functionality
        /// </summary>
        protected bool special = false;

        /// <summary>
        /// Prints this string when the sprite can be activated
        /// </summary>
        protected TPString usability = new TPString("Press E to use.");

        /// <summary>
        /// The name of the area that the sprite is in
        /// </summary>
        protected string areaName;

        /// <summary>
        /// Constructor for UsableIsometricSprite
        /// </summary>
        /// <param name="worldPos">The position of the sprite in the world</param>
        /// <param name="typeName">The type of the sprite</param>
        /// <param name="sprite">The player sprite</param>
        /// <param name="mapPos">The grid position of the sprite</param>
        /// <param name="faces">Whether or not the sprite faces left</param>
        /// <param name="area">The area the sprite exist in</param>
         public UsableIsometricSprite(Vector2 worldPos, string typeName, PlayerSprite sprite, Point mapPos, bool faces, string area)
            : base(worldPos, typeName, sprite, mapPos)
        {
            facesLeft = faces;
            isAvailable = false;
            usability.Scale *= 0.7f;
            areaName = area;
            usability.RenderColor = Color.Blue;
        }

        /// <summary>
        /// Update the sprite
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Special indicates that the behavoir is defines elsewhere, but we sill want to use the TPSprite base update.
            if(!special)
            {
                // If its available and the player is close, show that its available with a message and a color change.
                if (((facesLeft && playerSprite.currentPosition.Y == mapPosition.Y + 1 && playerSprite.currentPosition.X == mapPosition.X)
                    || (!facesLeft && playerSprite.currentPosition.Y == mapPosition.Y && playerSprite.currentPosition.X == mapPosition.X))
                    && playerSprite.motionState == PlayerMotionState.Still)
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
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="batch">The sprite batch to draw to</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            if (!special)
            {
                base.Draw(batch);
            }
            // If availabe, draw the usability message
            if (isAvailable)
            {
                usability.Position = worldPosition + (playerSprite.Position - playerSprite.roomPosition) + new Vector2(200, 0);
                usability.Draw(batch);
            }
        }

        // Override to specify what state to return on usage.
        public virtual TPState GetNewState()
        {
            return null;
        }
    }
}
