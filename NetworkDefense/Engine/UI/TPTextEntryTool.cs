using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine.UI
{
    public class TPTextEntryTool : TPUIElement
    {
        TPTextEntrySlot[] slots = new TPTextEntrySlot[12];

        public TPTextEntryTool(int id)
        {
            Vector2 pos = new Vector2(id % 2 * 640, 0);
            if(id > 1)
            {
                pos.Y = 360;
            }
            for(int i = 0; i < 12; i++)
            {
                slots[i] = new TPTextEntrySlot(pos, id);
            }
        }
    }
}
