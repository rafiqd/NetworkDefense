using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Events;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.Scheduling
{
    /// <summary>
    /// The startings of the concept of weeks within the game. Unimplemented at this time.
    /// </summary>
    static class ScheduleWeek
    {
        public static List<EventInfo> EventList;

        static ScheduleWeek()
        {
            EventList = new List<EventInfo>();
        }

        /// <summary>
        /// Unimplemented.
        /// </summary>
        public static void GetScheduleFromDb()
        {

        }

        /// <summary>
        /// Unimplemented.
        /// </summary>
        public static void CreateNewSchedule()
        {

        }
    }
}
