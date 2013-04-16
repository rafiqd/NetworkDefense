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
    /// Pipe game implementation of EventInfoManager for the lecture.
    /// </summary>
    class PipeGameLectureInfoWriter : EventInfoManager
    {

        public override EventInfo Write()
        {
            EventInfo lectureInfo = new EventInfo();
            lectureInfo.Type = "lecture";
            lectureInfo.Name = "pipegame";

            lectureInfo.Text = "(PLACEHOLDER TEXT)\n" +
                               "Welcome to Discrete Math!\n In this class, you will learn how to connect computers to the Wifi."
                               + "\n Let me give you a hint. Your mark is dependent on how quickly you solve the task."
                               + "\n See you in lab!";
            lectureInfo.Day = 1;
            lectureInfo.StartTime = 900;
            lectureInfo.EndTime = 1100;
            MinigameEvents.Add(lectureInfo);
            return lectureInfo;
        }

        public override EventInfo Read(EventInfo evenInfo)
        {
            throw new NotImplementedException();
        }

        public override void WriteDb(EventInfo lectureInfo)
        {
            throw new NotImplementedException();
        }

        public override EventInfo ReadDb(EventInfo evenInfo)
        {
            throw new NotImplementedException();
        }
    }
}
