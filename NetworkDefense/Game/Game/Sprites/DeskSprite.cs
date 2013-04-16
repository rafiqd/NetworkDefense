using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Events;
using Game.States;
using Microsoft.Xna.Framework;
using Engine;
using Engine.StateManagement;
using Engine.Objects;
using Game.Area;

namespace Game.Sprites
{
    class LectureDeskSprite : UsableIsometricSprite
    {
        string classString;
        public LectureDeskSprite(Vector2 pos, PlayerSprite sprite, Point mapPos, bool facesLeft, string areaName)
            : base(pos, "desk", sprite, mapPos, facesLeft, areaName)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            EventInfo _class = ((Clock)TPEngine.Get().SpriteDictionary["MainGameClock"]).GetClass();
            usability.Clear();
            if (_class == null || _class.Type != "lecture" && _class.Type != "lab")
            {
                usability.Append("No classes available");
                isAvailable = false;
                drawColor = Color.White;
            }
            else
            {
                usability.Append("Press E to start " + _class.Name);
            }
        }

        public override TPState GetNewState()
        {
            Clock clock = (Clock)TPEngine.Get().SpriteDictionary["MainGameClock"];
            EventInfo eventinfo = clock.GetClass();
            if (eventinfo == null)
                return null;
            if (eventinfo.Type == "lecture")
                return new LectureState(eventinfo);
            if (eventinfo.Type == "lab")
                switch(eventinfo.Name)
                {
                    case "birdhunt":
                        TPEngine.Get().State.PushState(new MyState(), true);
                        return new BirdMenuState();
                }
                //return new LabState(eventinfo);

            return null;
        }
    }
}
