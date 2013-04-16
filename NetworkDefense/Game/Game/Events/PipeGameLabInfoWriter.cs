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
    /// Pipe game implementation of EventInfoManager for the lab.
    /// </summary>
    class PipeGameLabInfoWriter : EventInfoManager
    {

        public override EventInfo Write()
        {
            EventInfo lectureInfo = new EventInfo();
            lectureInfo.Type = "lab";
            lectureInfo.Name = "pipegame";
            lectureInfo.Day = 1;
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
