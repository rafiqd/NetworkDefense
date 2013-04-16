/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Events;
using Game.States;
using Microsoft.Xna.Framework;
using Engine;
using Engine.StateManagement;
using Engine.Objects;
using Game.Area;

namespace Game.Sprites
{
    /// <summary>
    /// LectureDeskSprite provides access to lectures in the main game.
    /// </summary>
    class StudyDeskSprite : UsableIsometricSprite
    {
        /// <summary>
        /// The name of the current class, if there is one available.
        /// </summary>
        string classString;

        /// <summary>
        /// Constructor for LectureDeskSprite
        /// </summary>
        /// <param name="pos">The screen position of the sprite</param>
        /// <param name="sprite">the player sprite</param>
        /// <param name="mapPos">the position of the sprite within the map</param>
        /// <param name="facesLeft">Whether or not the sprite faces left</param>
        /// <param name="areaName">The name of the area the sprite exists in</param>
        public StudyDeskSprite(Vector2 pos, PlayerSprite sprite, Point mapPos, bool facesLeft, string areaName)
            : base(pos, "desk", sprite, mapPos, facesLeft, areaName)
        {
            isAvailable = true;
            usability.Clear();
            usability.Append("Press E to study");
        }

        /// <summary>
        /// Updates the sprite according to the amount of time that has passed
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Gets the state of the available lab
        /// </summary>
        /// <returns>The available state.</returns>
        public override TPState GetNewState()
        {
            return new StudyState();
        }
    }
}
