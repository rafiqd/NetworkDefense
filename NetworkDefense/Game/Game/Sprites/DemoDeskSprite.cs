using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Engine;
using Game.States;
using Engine.StateManagement;

namespace Game.Sprites
{
    class DemoDeskSprite : UsableIsometricSprite
    {
        public DemoDeskSprite(Vector2 pos, DemoPlayerSprite sprite, Point mapPos, bool facesLeft)
            : base(pos, "desk", sprite, mapPos, facesLeft)
        {

        }

        public override TPState GetNewState()
        {
            return new TestState();
        }
    }
}
