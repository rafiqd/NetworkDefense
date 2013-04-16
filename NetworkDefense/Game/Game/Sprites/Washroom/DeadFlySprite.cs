using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Engine;
using Microsoft.Xna.Framework.Input;

namespace Game
{
    class DeadFlySprite : TPSprite
    {
        public DeadFlySprite(Vector2 pos)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/deadFly"))
        {
            Position = pos;
        }
    }
}
