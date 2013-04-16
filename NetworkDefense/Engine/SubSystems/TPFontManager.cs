using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.SubSystems
{

    public class TPFontManager
    {
        Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
        TPGame game;

        public TPFontManager(TPGame game)
        {
            this.game = game;
        }

        public SpriteFont LoadFont(string name)
        {
            if (fonts.ContainsKey(name))
            {
                return fonts[name];
            }
            else
            {
                SpriteFont font = game.Content.Load<SpriteFont>(name);
                fonts.Add(name, font);
                return font;
            }
        }

    }

}
