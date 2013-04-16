/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Events;
using Game.PipeGame;
using Game.States;
using Game.States.TDsrc.TDStates;
using Microsoft.Xna.Framework;
using Engine;
using Engine.StateManagement;
using Engine.Objects;
using Game.Area;

namespace Game.Sprites
{
    /// <summary>
    /// LabDeskSprite provides access to labs in the main game.
    /// </summary>
    class LabDeskSprite : UsableIsometricSprite
    {
        /// <summary>
        /// The name of the current class, if there is one available.
        /// </summary>
        string classString;

        /// <summary>
        /// Constructor for LabDeskSprite
        /// </summary>
        /// <param name="pos">The screen position of the sprite</param>
        /// <param name="sprite">the player sprite</param>
        /// <param name="mapPos">the position of the sprite within the map</param>
        /// <param name="facesLeft">Whether or not the sprite faces left</param>
        /// <param name="areaName">The name of the area the sprite exists in</param>
        public LabDeskSprite(Vector2 pos, PlayerSprite sprite, Point mapPos, bool facesLeft, string areaName)
            : base(pos, "computerdesk", sprite, mapPos, facesLeft, areaName)
        {

        }

        /// <summary>
        /// Updates the sprite according to the amount of time that has passed
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            EventInfo _class = ((Clock)TPEngine.Get().SpriteDictionary["MainGameClock"]).GetClass();
            usability.Clear();
            if (_class == null || _class.Type != "lab")
            {
                usability.Append("No classes available");
                isAvailable = false;
                drawColor = Color.White;
            }
            else
            {
                usability.Append("Press E to start " + _class.Name);
            }
        }

        /// <summary>
        /// Gets the state of the available lab
        /// </summary>
        /// <returns>The available state.</returns>
        public override TPState GetNewState()
        {
            Clock clock = (Clock)TPEngine.Get().SpriteDictionary["MainGameClock"];
            EventInfo eventinfo = clock.GetClass();
            if (eventinfo == null)
                return null;
            if (eventinfo.Type == "lab")
            {
                // Set the clock to the end of the lecture block
                clock.Set(15,0,false);

                switch (eventinfo.Name)
                {
                    case "birdhunt":
                        return new BirdHuntState();
                    case "pipegame":
                        TPEngine.Get().State.PushState(new PipeGame_GameState(), true); //game then start
                        return new PipeGame_StartState();
                    case "networkdefense":
                        return new TDBaseState();
                    case "wordsearch":
                        return new WordSearch_MainState();
                }
                //return new LabState(eventinfo);
            }
            return null;
        }
    }
}
