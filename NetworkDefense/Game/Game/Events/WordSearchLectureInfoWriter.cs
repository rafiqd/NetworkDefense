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
    /// Word Search implementation of EventInfoManager for the lecture.
    /// </summary>
    class WordSearchLectureInfoWriter : EventInfoManager
    {
       
        public override EventInfo Write()
        {
            EventInfo lectureInfo = new EventInfo();
            lectureInfo.Type = "lecture";
            lectureInfo.Name = "wordsearch";

            lectureInfo.Text = "(PLACEHOLDER TEXT)\n" +
                               "Welcome to Business Communications!\n In this class, you want to find yourself some words!"
                               + "\n Blah Blah PLACEHOLDER TEXT!!"
                               + "\n See you in lab!";
            lectureInfo.Day = 3;
            lectureInfo.StartTime = 900;
            lectureInfo.EndTime = 1100;
            MinigameEvents.Add(lectureInfo);
            return lectureInfo;
        }

        public override EventInfo Read(EventInfo evenInfo)
        {
            throw new NotImplementedException();
        }

        public override EventInfo ReadDb(EventInfo evenInfo)
        {
            throw new NotImplementedException();
        }

        public override void WriteDb(EventInfo eventInfo)
        {
            throw new NotImplementedException();
        }
    }
}
