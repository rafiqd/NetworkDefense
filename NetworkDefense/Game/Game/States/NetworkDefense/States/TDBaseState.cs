using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Engine;

namespace Game.States.TDsrc.TDStates
{
    /// <summary>
    /// Base state for the Tower defense game
    /// </summary>
    class TDBaseState : TPState
    {
        /// <summary>
        /// pushes the starting state of the tower defense game on
        /// </summary>
        protected override void Load()
        {
            base.Load();
            TPEngine.Get().State.PushState(new TDStartState(), true);
        }

        /// <summary>
        /// Updates this instance of this state
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws this state
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            base.Draw(batch);
        }

    }
}
