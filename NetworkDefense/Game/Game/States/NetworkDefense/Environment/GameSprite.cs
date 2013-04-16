using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Engine;

namespace Game.States.TDsrc.Environment
{
    class GameSprite : TPSprite
    {
        public GameSprite(Vector2 pos)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art /[TPEngine]/[mytexturename]"))
        {
            Position = pos;
        }

    }
}
