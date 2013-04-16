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
    /// Tower Defense implementation of LectureInfoManager
    /// </summary>
    class TdLabInfoWriter : EventInfoManager
    {
        private string[] colors = { "blue", "green", "yellow", "purple", "red" };
        private string BonusTowerColor;
        private string ColorMod1;
        private string ColorMod2;
        private int DmgMod1;
        private int DmgMod2;
        private EventInfo lectureInfo;

        public override EventInfo Write()
        {
            EventInfo lectureInfo = new EventInfo();
            lectureInfo.Type = "lab";
            lectureInfo.Name = "networkdefense";

            lectureInfo.Day = 0;
            lectureInfo.StartTime = 1300;
            lectureInfo.EndTime = 1500;
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
