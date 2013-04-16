/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Microsoft.Xna.Framework;
using Engine.Objects;
using Microsoft.Xna.Framework.Input;
using Engine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Game.States
{
    /// <summary>
    /// SleepState allows the player to choose an amount of time to sleep for and provides benefits according to
    /// the amount of time slept.  Player can sleep up to 8 hours.
    /// </summary>
    class StudyState : TPState
    {
        /// <summary>
        /// Question the player how long to sleep for
        /// </summary>
        TPString durationPrompt = new TPString("How long do you want to study? (Max: 4 hours. Esc to cancel.)");

        /// <summary>
        /// String representing the number of hours to sleep for.
        /// </summary>
        TPString hoursString = new TPString("2 hours");

        /// <summary>
        /// The number of hours to sleep for.
        /// </summary>
        int hoursToStudy = 2;

        /// <summary>
        /// The maximum number of hours to sleep.
        /// </summary>
        const int MAX_HOURS = 4;

        /// <summary>
        /// The previous keyboard state
        /// </summary>
        KeyboardState previousKeyboardState;

        /// <summary>
        /// The background
        /// </summary>
        TPSprite back = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/alpha/books"));

        /// <summary>
        /// Fade out
        /// </summary>
        Texture2D fade = TPEngine.Get().TextureManager.LoadTexture(@"art/alpha/fade");

        /// <summary>
        /// The gameTime millisecond the player started to sleep.
        /// </summary>
        double studyTime;

        /// <summary>
        /// The amount of time spent sleeping
        /// </summary>
        double timeStudying = 0;

        /// <summary>
        /// Whether or not the player is currently sleeping.
        /// </summary>
        bool studying = false;

        /// <summary>
        /// Constructor for SleepState
        /// </summary>
        public StudyState()
        {
            TPLayer layer = new TPLayer(layers);
            layer.AddEntity(back);
            layer.AddEntity(hoursString);
            layer.AddEntity(durationPrompt);

            hoursString.RenderColor = Color.Blue;
            hoursString.Position = new Vector2(200, 200);

            durationPrompt.RenderColor = Color.Blue;
            durationPrompt.Position = new Vector2(175, 175);

            previousKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Updates the state
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleInput(gameTime);

            hoursString.Clear();
            hoursString.Append(hoursToStudy.ToString() + " hours");

            previousKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Handles player input
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        private void HandleInput(GameTime gameTime)
        {
            if (!studying)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
                {
                    if (hoursToStudy < MAX_HOURS)
                    {
                        hoursToStudy++;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
                {
                    if (hoursToStudy > 1)
                    {
                        hoursToStudy--;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    TPEngine.Get().State.PopState();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    studyTime = gameTime.TotalGameTime.TotalMilliseconds;
                    TPSprite clock;
                    TPEngine.Get().SpriteDictionary.TryGetValue("MainGameClock", out clock);
                    ((Clock)clock).Advance(hoursToStudy, 0);
                    studying = true;
                    PlayerMeters.PlayerMeters.SetSanity(4 * hoursToStudy);
                }
            }
            else
            {
                timeStudying = gameTime.TotalGameTime.TotalMilliseconds - studyTime;
                if (timeStudying > 5000)
                {
                    TPEngine.Get().State.PopState();
                }
            }
        }

        /// <summary>
        /// Draws the state
        /// </summary>
        /// <param name="batch">The sprite batch to draw to</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            batch.GraphicsDevice.Clear(Color.Black);
            base.Draw(batch);

            if (studying)
            {
                float fadeVal = (float)(timeStudying / 5000);
                Color color = new Color(0, 0, 0, fadeVal);
                batch.Begin();
                batch.Draw(fade, Vector2.Zero, color);
                batch.End();
            }
        }
    }
}
