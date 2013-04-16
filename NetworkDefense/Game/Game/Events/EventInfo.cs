using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.Events
{
    /// <summary>
    /// Events include labs and lectures. This information is used in the schedule and clock classes
    /// to determine the correct order and times each minigame is played at.
    /// </summary>
    public class EventInfo
    {
        public int Id;
        public string Type;
        public string Name;
        public static int IdCounter;
        public string Text;
        public List<Texture2D> Images;
        public string Data;
        public int StartTime;
        public int EndTime;
        public int ArrivalHour;
        public double ArrivalMin;
        public int Day;

        static EventInfo()
        {
            IdCounter = 0;
        }

        public EventInfo()
        {
            Id = IdCounter++;
        }

        public EventInfo(string type, string name, string text, List<Texture2D> images
                            , string data, int startTime, int endTime, int day)
        {
            Type = type;
            Id = IdCounter++;
            Name = name;
            Text = text;
            Images = images;
            Data = data;
            StartTime = startTime;
            EndTime = endTime;
            Day = day;
        }
    }
}
