/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Game.Events;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Microsoft.Xna.Framework;
using Game.Saving;
using Game.Quests;

namespace Game
{
    class Clock : TPSprite
    {
        /// <summary>
        /// The current hour in game
        /// </summary>
        public int currentHour;

        /// <summary>
        ///  The current minute in game
        /// </summary>
        public double currentMinute;

        /// <summary>
        /// If it is morning or not in game
        /// </summary>
        public bool morning = true;

        /// <summary>
        /// The string that will print on the screen representing the time in 12hour format
        /// </summary>
        TPString printableTime = new TPString("8:00 AM");

        /// <summary>
        /// The string that will print on the screen informing of the next class to take place or the current available class.
        /// </summary>
        TPString nextClass = new TPString("Lecture 1 - 9:00 AM (1:00)");

        /// <summary>
        /// The current day of week in game.
        /// </summary>
        public int currentDay;

        /// <summary>
        /// The current week in the term.
        /// </summary>
        public int currentWeek;

        public bool flag;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hour">starting hour</param>
        /// <param name="minute">starting minute</param>
        /// <param name="day">starting day</param>
        public Clock(int week, int hour, double minute, int day)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/alpha/point"))
        {
            printableTime.Position = new Vector2(800, 50);
            printableTime.RenderColor = Color.Lime;
            nextClass.Position = new Vector2(50, 50);
            nextClass.RenderColor = Color.Lime;
            currentDay = day;
            currentHour = hour;
            currentMinute = minute;
            currentWeek = week;
            flag = false;

            // Is it morning?
            if (currentHour >= 12)
            {
                morning = false;
            }
        }

        /// <summary>
        /// Update loop
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // Minutes go by 3 game minutes per real time second
            currentMinute += (double)gameTime.ElapsedGameTime.Milliseconds / 300;

            // If over 60, then an hour has passed
            if (currentMinute >= 60)
            {
                currentMinute = currentMinute - 60;
                currentHour++;
                SaveCharacterData.Save();

                // If over 24, then a day has passed
                if (currentHour == 24)
                {
                    currentHour = 0;
                    currentDay++;
                    morning = true;
                    QuestChecking.SignalNewQuestDay();
                }
                // no longer morning
                else if (currentHour == 12)
                {
                    morning = false;
                }

                // currently have 4 days in a week, if day is 4, then go back to beginning of week (0 as it is an array index)
                //if (currentDay == 4)
                //{
                //    currentDay = 0;
                //}
            }
            if(currentDay == 4 && !flag)
            {
                currentDay = 0;
                currentWeek++;
                flag = true;
            }

            printableTime.Clear();

            // 0 is 12:00 AM
            if (currentHour == 0)
            {
                printableTime.Append("12:");
            }
            // Adjust for 12 hour clock format
            else if (currentHour > 12)
            {
                int hour = currentHour - 12;
                printableTime.Append(hour + ":");
            }
            else
            {
                printableTime.Append(currentHour + ":");
            }

            // adjust for correct two-figure format
            if (currentMinute < 10)
            {
                printableTime.Append('0');
            }

            // Minute is floating point to allow better accuracy, adjust to display as a whole value.
            printableTime.Append(Math.Floor(currentMinute) + " ");
            printableTime.Append((morning) ? "AM " : "PM ");
            printableTime.Append("Day: " + (currentDay + 1) + " ");
            printableTime.Append("Week: " + currentWeek);

            nextClass.Clear();

            // Get the next class from the Schedule singleton
            NextClass nextClassValues = Schedule.Get().GetNextClass(currentDay, currentHour);
            nextClass.RenderColor = Color.Lime;
            nextClass.Append(nextClassValues.className);

            // pick a scale that looks good, reset to this size in case the pulsation below has ended
            nextClass.Scale = new Vector2(1f, 1f);

            // If there is a class coming, print it out!
            if(nextClassValues.className != "No class")
            {
                nextClass.Append(" - " + ((nextClassValues.classHour > 12) ? nextClassValues.classHour - 12 : nextClassValues.classHour) + ":00");
            }

            // Tell the player when they are late for a class
            if (nextClassValues.classHour == currentHour && currentHour != 0)
            {
                nextClass.RenderColor = Color.Red;
                nextClass.Append(" (Late: +" + Math.Floor(currentMinute) + ")");
            }
            // Tell the player if there is a class happening within the next hour
            else if (nextClassValues.classHour == currentHour + 1)
            {
                nextClass.Append(" (Hurry: " + (60 -Math.Floor(currentMinute)) + ")");
            }

            // Pulsate the text size to get the player attention when a class is very soon.
            if (nextClassValues.classHour == currentHour + 1 && currentMinute > 30 || nextClassValues.classHour == currentHour)
            {
                float scale = (float)Math.Sin((gameTime.TotalGameTime.Milliseconds / 1000d) * 2 * Math.PI);
                nextClass.Scale = new Vector2(1 + scale/ 20, 1 + scale / 20);
            }

            if (currentWeek == 5)
            {
                SaveCharacterData.Save();
                TPEngine.Get().State.PushState(new EndingState(), true);
            }
        }

        /// <summary>
        /// Set the current time.  This would happen at the end of a lab or lecture.
        /// </summary>
        /// <param name="hour">The updated hour</param>
        /// <param name="minute">The updated minute</param>
        /// <param name="morn">Whether or not its morning</param>
        public void Set(int hour, int minute, bool morn)
        {
            currentHour = hour;
            currentMinute = minute;
            morning = morn;
        }

        /// <summary>
        /// Advances the clock a set amount of time (for example, when sleeping).
        /// </summary>
        /// <param name="hours">The number of hours to advance the time by</param>
        public void Advance(int hours, int minutes)
        {
            currentMinute += minutes;
            if (currentMinute >= 60)
            {
                currentMinute -= 60;
                currentHour++;
            }
            currentHour += hours;
            if (currentHour >= 24)
            {
                currentHour = currentHour - 24;
                currentDay++;
                morning = true;
                QuestChecking.SignalNewQuestDay();
            }
            else if (currentHour >= 12)
            {
                morning = false;
            }
            if (currentDay >= 4)
            {
                currentDay = 0;
                currentWeek++;
            }
        }

        /// <summary>
        /// Gets the current hour;
        /// </summary>
        /// <returns>The current hour</returns>
        public int GetHour()
        {
            return currentHour;
        }

        /// <summary>
        /// Gets the current minute.
        /// </summary>
        /// <returns>The current minute</returns>
        public double GetMinute()
        {
            return currentMinute;
        }

        /// <summary>
        /// Get the currently available class (if there is one).
        /// </summary>
        /// <returns>The currently evailable class.</returns>
        public EventInfo GetClass()
        {
            return Schedule.Get().GetAvailableClass_SamRafiqVersion(currentDay, currentHour, currentMinute);
        }

        /// <summary>
        /// Draw the strings to top layer on the screen.
        /// </summary>
        /// <param name="batch">The sprite batch to draw with</param>
        public override void Draw(SpriteBatch batch)
        {
            printableTime.Draw(batch);
            nextClass.Draw(batch);
        }
    }
}
