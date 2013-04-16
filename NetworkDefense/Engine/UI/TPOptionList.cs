using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine.UI
{
    public class TPOptionList : TPUIElement
    {
        List<TPOptionListItem> m_Items = new List<TPOptionListItem>();
        private SpriteFont m_Font;
        private int m_Index = 0;

        private const int DELAY = 100;
        private int m_Delay;


        private static readonly Color COLOR_SELECTED = Color.Red;
        private static readonly Color COLOR_UNSELECTED = Color.Gray;
        private static readonly Vector2 SCALE_SELECTED = new Vector2(1f, 1f);
        private static readonly Vector2 SCALE_UNSELECTED = new Vector2(0.5f, 0.5f);
        private const float START_SCALE = 2;

        public int OffsetY { get; set; }


        public TPOptionList(  int offsetY = 75) 
        {
            m_Font = TPEngine.Get().FontManager.LoadFont(@"fonts\testfont");
            OffsetY = offsetY;
            this.Position.X = 220;
            this.Position.Y = 170;
        }

        public void AddItem(TPOptionListItem item)
        {
            item.Position.Y = this.Position.Y + OffsetY * m_Items.Count;
            item.Position.X = this.Position.X;
            item.Scale.X = item.Scale.Y = START_SCALE;
            m_Items.Add(item);
        }

        public void MoveUp()
        {
            if (m_Delay <= 0)
            {
                m_Index++;
                if (m_Index >= m_Items.Count)
                {
                    m_Index = m_Items.Count - 1;
                }
                m_Delay = DELAY;
            }
        }

        public void MoveDown()
        {
            if (m_Delay <= 0)
            {
                m_Index--;
                if (m_Index < 0)
                {
                    m_Index = 0;
                }
                m_Delay = DELAY;
            }

        }

        public void Select()
        {
            m_Items[m_Index].Select();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (m_Delay > 0)
            {
                m_Delay -= gameTime.ElapsedGameTime.Milliseconds;
            }

            for (int i = 0; i < m_Items.Count; i++)
            {
                m_Items[i].Update(gameTime);
                if (i == m_Index)
                {
                    m_Items[i].ColorTarget = COLOR_SELECTED;
                    if (!TPMath.IsWithinRange(m_Items[i].Scale, SCALE_SELECTED, 0.05f))
                    {
                        m_Items[i].Scale += TPMath.GetDirectionVector(m_Items[i].Scale, SCALE_SELECTED) / 10;
                    }

                }
                else
                {
                    m_Items[i].ColorTarget = COLOR_UNSELECTED;
                    if (!TPMath.IsWithinRange(m_Items[i].Scale, SCALE_UNSELECTED, 0.05f))
                    {
                        m_Items[i].Scale += TPMath.GetDirectionVector(m_Items[i].Scale, SCALE_UNSELECTED) / 10;
                    }
                }
                m_Items[i].Position.Y = this.Position.Y + OffsetY * i;
                m_Items[i].Position.X = this.Position.X;
                TPMath.ShiftTowardsColor(ref m_Items[i].RenderColor, ref m_Items[i].ColorTarget, 5);
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (TPOptionListItem item in m_Items)
            {
                item.Draw(batch);
            }
        }


    }
}
