/**********************************************
 *****************Ray Young********************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Events;

namespace Game
{
    /// <summary>
    /// Holds relevant information about the next class
    /// </summary>
    struct NextClass
    {
        public string className;
        public int classHour;
    }
    
    /// <summary>
    /// The schedule keeps track of when classes will happen.  It can be referenced to find out what is happening at specific times or
    /// to find when is the next event happening in a particular day, given a particular time.
    /// </summary>
    class Schedule
    {
        /// <summary>
        /// The singleton instance
        /// </summary>
        static Schedule instance;

        /// <summary>
        /// The array that holds the scheduled items.  Each array element represents a specific hour on a specific day of the week.
        /// </summary>
        string[][] timeBlocks = new string[4][];

        private List<EventInfo> _timeBlocks;
        

        /// <summary>
        /// The number of days in the week
        /// </summary>
        const int NUM_DAYS = 4;

        /// <summary>
        /// The number of hours in the day.
        /// </summary>
        const int NUM_HOURS = 9;

        /// <summary>
        /// Number of minutes early a student can be to start a lecture
        /// format to change should be hmm
        /// </summary>
        const int EARLY_MINS = 30;
        
        /// <summary>
        /// Constructor.  Sets up the default empty values for each time block and currently hard-sets classes and labs to specific times.
        /// </summary>
        private Schedule()
        {
            _timeBlocks = EventInfoManager.MinigameEvents;


            for (int i = 0; i < NUM_DAYS; i++)
            {
                timeBlocks[i] = new string[NUM_HOURS];
                for (int j = 0; j < NUM_HOURS; j++)
                {
                    timeBlocks[i][j] = "No class";
                }
            }

            timeBlocks[0][1] = "Lecture 1";
            timeBlocks[0][5] = "Lab 1";
            timeBlocks[1][1] = "Lecture 2";
            timeBlocks[1][5] = "Lab 2";
            timeBlocks[2][1] = "Lecture 3";
            timeBlocks[2][5] = "Lab 3";
            timeBlocks[3][1] = "Lecture 4";
            timeBlocks[3][5] = "Lab 4";
        }

        /// <summary>
        /// Accessor for the singleton
        /// </summary>
        /// <returns>The singleton instance</returns>
        public static Schedule Get()
        {
            if (instance == null)
            {
                instance = new Schedule();
            }
            return instance;
        }

        /// <summary>
        /// Get the next class for the specified day and time, regardless of the time it occurs at.
        /// </summary>
        /// <param name="day">The current day</param>
        /// <param name="hour">The current hour</param>
        /// <returns></returns>
        public NextClass GetNextClass(int day, int hour)
        {
            NextClass nextClass = new NextClass();

            // Assume there is not class
            nextClass.className = "No class";

            // 8 is a magic number representing the time of the first block in the schedule (8am).  Should be updated to use a constant.
            for (int i = hour - 8; i < NUM_HOURS && i >= 0; i++)
            {
                if (timeBlocks[day][i] != "No class")
                {
                    nextClass.className = timeBlocks[day][i];
                    nextClass.classHour = i + 8;
                    break;
                }
            }
            return nextClass;
        }

        /// <summary>
        /// Determines if there is a class currently available and returns it if there is.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public string GetAvailableClass(int day, int hour, double minute)
        {
            // convert hour to index
            int hourIndex = hour - 8;

            // Start by assuming there is no class
            string className = "No class";

            // If the hour lies within school hours, check for a class
            if (hourIndex >= 0 && hourIndex < 8)
            {
                className = timeBlocks[day][hourIndex];
            }

            // If there is no class running right now, check to see if one is coming in the next 30 minutes.  This way, the player doesn't have to
            // wait for the exact moment the class starts.
            if (className == "No class" && minute >= 30 && hourIndex < 7)
            {
                className = timeBlocks[day][hourIndex + 1];
            }
            return className;
        }
    
        /// <summary>
        /// Same as above but uses our impelementation to test 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public EventInfo GetAvailableClass_SamRafiqVersion(int day, int hour, double minute)
        {
            // convert hour to index
            int hourIndex = hour - 8;
            double time = hour*100 + minute;
            // Start by assuming there is no class
            //string className = "No class";
            for (int i = 0; i < EventInfoManager.MinigameEvents.Count; i++)
            {
                int _day = EventInfoManager.MinigameEvents[i].Day;
                int startTime = EventInfoManager.MinigameEvents[i].StartTime;
                int endTime = EventInfoManager.MinigameEvents[i].EndTime;

                if (_day == day && time >= startTime - 100 && endTime - 100 >= time)
                {
                    return EventInfoManager.MinigameEvents[i];
                }
            }
            return null;
                // If the hour lies within school hours, check for a class
            //    if (hourIndex >= 0 && hourIndex < 8)
            //    {
            //        className = timeBlocks[day][hourIndex];
            //    }

            //// If there is no class running right now, check to see if one is coming in the next 30 minutes.  This way, the player doesn't have to
            //// wait for the exact moment the class starts.
            //if (className == "No class" && minute >= 30 && hourIndex < 7)
            //{
            //    className = timeBlocks[day][hourIndex + 1];
            //}
            //return className;
        }
    }
}
