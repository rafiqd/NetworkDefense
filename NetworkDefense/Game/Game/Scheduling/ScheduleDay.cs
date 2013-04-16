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
    /// The startings of the concept of days within the game. Unimplemented at this time.
    /// The concept of day is currently represented inside the Schedule class.
    /// </summary>
    class ScheduleDay
    {
        public List<EventInfo> Lectures;
        public List<EventInfo> Labs;

        public ScheduleDay(List<EventInfo> lectures, List<EventInfo> labs)
        {
            Lectures = lectures;
            Labs = labs;
        }
    }
}
