using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Engine.UI;

namespace Game.UI
{
    public class TPTextEntrySlot : TPUIElement
    {
        static Char[] characters = { ' ', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        static Color[] colors = { Color.Red, Color.LightGreen, Color.Yellow, Color.Blue };
        Color color;
        public TPString[] strings = new TPString[3];
        int index = 0;
        
        public TPTextEntrySlot(Vector2 p, int id)
        {
            color = colors[id];
            for (int i = 0; i < 3; i++)
            {
                strings[i] = new TPString("", color);
            }
            strings[0].RenderColor.A = 100;
            strings[0].Position = new Vector2(p.X, p.Y - 20);
            strings[1].Position = new Vector2(p.X, p.Y);
            strings[2].Position = new Vector2(p.X, p.Y + 20);
            strings[2].RenderColor.A = 100;
            Alive = false;
        }

        public void DownPressed()
        {
            if (index > 0)
            {
                index--;
                RefreshSlot();
            }
        }

        public void UpPressed()
        {
            if (index < 25)
            {
                index++;
                RefreshSlot();
            }
        }

        public void RefreshSlot()
        {
            for (int i = 0; i < 3; i++)
            {
                strings[i].Clear();
                if ((index - 1 + i) >= 0 && (index - 1 + i) < 25)
                    strings[i].Append(characters[index - 1 + i]);
            }
        }

        public void SetInactive()
        {
            strings[0].Clear();
            strings[2].Clear();
        }

        public int GetIndex()
        {
            return index;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            foreach (TPString s in strings)
                s.Draw(batch);
        }
    }
}
