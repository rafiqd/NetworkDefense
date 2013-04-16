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
    /// Bird Hunt implementation of LectureInfoManager
    /// </summary>
    class BirdHuntLectureInfoWriter : EventInfoManager
    {

        public override EventInfo Write()
        {
            
            EventInfo lectureInfo = new EventInfo();
            lectureInfo.Type = "lecture";
            lectureInfo.Name = "birdhunt";
            
            //lectureInfo.Images.Add();
            lectureInfo.Text = "(PLACEHOLDER TEXT)\n" +
                               "Welcome to Web Development!\nIf you want to do well in your first lab,\n" +
                               "bring ammo!! See you at 1pm!";
            lectureInfo.Day = 2;
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
