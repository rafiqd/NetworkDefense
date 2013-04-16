using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Game.Sprites
{
    /// <summary>
    /// Class used for creating a countdown timer
    /// </summary>
    class WordSearch_CountDownTimer
    {
        /// <summary>
        /// start of the timer
        /// </summary>
        private int startCount;

        /// <summary>
        /// when end count == 0, means countdown is finished
        /// </summary>
        private int endCount;

        /// <summary>
        /// getter and setter for displaying the countdown
        /// </summary>
        public int displayValue { get; private set; }

        /// <summary>
        /// getter and setter to activiate the countdown timer
        /// </summary>
        public Boolean isActive { get; private set; }

        /// <summary>
        /// getter and setter to check to see if the countdown has finished or not
        /// </summary>
        public Boolean isComplete { get; private set; }

        /// <summary>
        /// constructor to intialize all the getters and setters
        /// </summary>
        public WordSearch_CountDownTimer()
        {
            this.isActive = false;
            this.isComplete = false;
            this.displayValue = 0;
            this.startCount = 0;
            this.endCount = 0;
        }

        /// <summary>
        /// sets the timer to how many seconds you want it to count down from
        /// </summary>
        /// <param name="gameTime">time of the game</param>
        /// <param name="seconds">amount of seconds you want to start at</param>
        public void set(GameTime gameTime, int seconds)
        {
            this.startCount = (int)gameTime.TotalGameTime.TotalSeconds;
            this.endCount = (this.startCount + seconds);
            this.isActive = true;
            this.displayValue = this.endCount;
        }

        /// <summary>
        /// increments the end count, used for if a player finds a words he can add seconds to the countdown
        /// </summary>
        /// <param name="seconds">amount of seconds to increment the timer by</param>
        public void incrementTimer(int seconds)
        {
            this.endCount += seconds;
        }

        /// <summary>
        /// checks the timer to see if the countdown has reached zero or not
        /// </summary>
        /// <param name="gameTime">time of the game</param>
        /// <returns>if the countdown has finished or not</returns>
        public Boolean checkTimer(GameTime gameTime)
        {

            this.displayValue = (this.endCount - (int)gameTime.TotalGameTime.TotalSeconds);

            if (this.isComplete == false)
            {
                if (this.endCount < (int)gameTime.TotalGameTime.TotalSeconds)
                {
                    this.isComplete = true;
                }
            }

            return this.isComplete;
        }
    }
}
