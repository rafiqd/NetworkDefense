using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Engine.Objects;

namespace Engine.UI
{
        public delegate void TPRollingTextOnSelect();

    public class TPRollingTextMenuItem : TPString
    {
        private TPRollingTextOnSelect m_cbOnselect;
        public Vector2 PositionTarget = Vector2.Zero;
        public float AlphaTarget = 0;
        public float Alpha = 0;
        public Color ColorTarget = Color.Red;
        public Vector2 ScaleTarget = new Vector2(1, 1);

        public TPRollingTextMenuItem(string label, TPRollingTextOnSelect callback)
            : base(label, TPEngine.Get().FontManager.LoadFont(@"fonts\testfont_large"))
        {
            m_cbOnselect = callback;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Selects the item 
        /// </summary>
        public void Select()
        {
            m_cbOnselect.Invoke();
        }

    }
}
