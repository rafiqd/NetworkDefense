using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Sprites
{
    [Serializable]
    public class SpriteNameDictionary : Dictionary<char, Texture2D>
    {
        public void Load()
        {
            this['t'] = TPEngine.Get().TextureManager.LoadTexture(@"art\DemoArt\demotile");
            this['c'] = TPEngine.Get().TextureManager.LoadTexture(@"art\DemoArt\democylinder");
            this['p'] = TPEngine.Get().TextureManager.LoadTexture(@"art\DemoArt\demoperson");
        }
    }
}
