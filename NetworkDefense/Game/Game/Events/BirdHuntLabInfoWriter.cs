using System;

/*
 * Authors: Sam and Rafiq
 */
namespace Game.Events
{
    /// <summary>
    /// Bird Hunt implementation of LabInfoWriter
    /// </summary>
    class BirdHuntLabInfoWriter : EventInfoManager
    {

        public override EventInfo Write()
        {
            
            EventInfo lectureInfo = new EventInfo();
            lectureInfo.Type = "lab";
            lectureInfo.Name = "birdhunt";
            lectureInfo.Day = 2;
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
