using Engine.StateManagement;
using Game.Events;
using Microsoft.Xna.Framework.Input;
using Engine;
using Engine.Objects;
using Microsoft.Xna.Framework;

namespace Game.States
{
    /// <summary>
    /// The actual state that is used by all minigames to convey Lecture information. Interacts with the clock.
    /// </summary>
    class LectureState : TPState
    {
        TPString s;
        TPLayer layer;
        KeyboardState prevState;
        public Clock clock;

        /// <summary>
        /// The EventInfo holds information that is displayed on the LectureState, this is used
        /// by all Lectures.
        /// </summary>
        /// <param name="info">info contains all data for the Lecture</param>
        public LectureState(EventInfo info)
        {
            s = new TPString(info.Name + "\n" + info.Text);
            prevState = Keyboard.GetState();
            layer = new TPLayer(this.layers);
            layer.AddEntity(s);
            s.Position = new Vector2(200, 200);
            TPSprite temp;
            if (!TPEngine.Get().SpriteDictionary.TryGetValue("MainGameClock", out temp))
            {
                //clock = new Clock(8, 0.0d, 0);
                TPEngine.Get().SpriteDictionary.Add("MainGameClock", clock);
            }
            else
            {
                clock = (Clock)temp;
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.X) && prevState.IsKeyUp(Keys.X))
            {
                if(clock.GetHour() < 11)
                    clock.Set(11,0,true);

                TPEngine.Get().State.PopState();
            }
            prevState = Keyboard.GetState();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            batch.GraphicsDevice.Clear(Color.Black);
            base.Draw(batch);
        }
    }
}
