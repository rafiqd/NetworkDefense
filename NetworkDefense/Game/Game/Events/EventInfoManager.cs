using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.Events
{
    /// <summary>
    /// Manages a static MinigameEvents list of info for Lectures and provides methods for each Event to
    /// Read, Write in the game and from/to the database.
    /// </summary>
    abstract class EventInfoManager
    {
        public static List<EventInfo> MinigameEvents;

        static EventInfoManager()
        {
            MinigameEvents = new List<EventInfo>();
        }
        //Ensure MinigameLectures.Add(lectureInfo) is called at the end of the Write method.

        public abstract EventInfo Read(EventInfo evenInfo);
        public abstract EventInfo Write();
        public abstract EventInfo ReadDb(EventInfo evenInfo);
        public abstract void WriteDb(EventInfo eventInfo);
    }
}
