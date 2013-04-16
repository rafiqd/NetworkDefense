using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Objects;

namespace Engine.UI
{
    public class TPOptionListItem : TPString
    {

        TPOptionListCallback m_Callback;

        public Color ColorTarget = Color.White;
        public float AlphaTarget { get; set; }

        public TPOptionListItem(string str)
            : this(str,   TPOptionListItem.DoNothingCallback)
        {
            AlphaTarget = 1.0f;
        }

        public TPOptionListItem(string str,   TPOptionListCallback callback)
            : base(str,TPEngine.Get().FontManager.LoadFont(@"fonts\testfont"))
        {
            m_Callback = callback;
        }

        public void Select()
        {
            m_Callback.Invoke();
        }


        public static void DoNothingCallback() { }
    }
        public delegate void TPOptionListCallback();
    
}
