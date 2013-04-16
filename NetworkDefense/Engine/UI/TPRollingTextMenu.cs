using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine.UI
{
    public class TPRollingTextMenu : TPUIElement
    {
        private readonly Vector2 SCALE_MINUTE = new Vector2(0.05f, 0.05f);
        private readonly Vector2 SCALE_SMALL = new Vector2(0.33f, 0.33f);
        private readonly Vector2 SCALE_MEDIUM = new Vector2(0.5f, 0.5f);
        private readonly Vector2 SCALE_LARGE = new Vector2(1f, 1f);

        private const int DELAY = 100;
        private int m_Delay;

        private readonly float[] ALPHAS = new float[] { 0, 0.1f, 0.45f, 1.0f };

        private List<TPRollingTextMenuItem> m_MenuItemSet = new List<TPRollingTextMenuItem>();
        private int m_Index;
        private float[] OFFSETS = new float[7];

        public TPRollingTextMenu()
            : this(-100f, -70f, -50f, 0f, 50f, 70f, 100f)
        {

        }

        public TPRollingTextMenu(float pos1, float pos2, float pos3, float pos4, float pos5, float pos6, float pos7) 
        {
            OFFSETS[0] = pos1;
            OFFSETS[1] = pos2;
            OFFSETS[2] = pos3;
            OFFSETS[3] = pos4;
            OFFSETS[4] = pos5;
            OFFSETS[5] = pos6;
            OFFSETS[6] = pos7;
        }

        /// <summary>
        /// Sets color.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            foreach (TPRollingTextMenuItem item in m_MenuItemSet)
            {
                item.ColorTarget.R = color.R;
                item.ColorTarget.G = color.G;
                item.ColorTarget.B = color.B;

            }
        }


        public void AddItem(string label, TPRollingTextOnSelect callback)
        {
            TPRollingTextMenuItem item = new TPRollingTextMenuItem(label, callback);
            item.Position.X = this.Position.X + TPEngine.Get().FontManager.LoadFont(@"fonts\testfont").MeasureString(item.ToString()).X / 4;
            item.Position.Y = this.Position.Y;
            m_MenuItemSet.Add(item);
            if (m_MenuItemSet.Count == 1)
            {
                m_Index = 0;
            }
            SetTargets();
        }

        public void AddItem(string label)
        {
            this.AddItem(label, new TPRollingTextOnSelect(TPRollingTextMenu.DoNothingCallback));
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_Delay > 0)
            {
                m_Delay -= gameTime.ElapsedGameTime.Milliseconds;
            }
            foreach (TPRollingTextMenuItem item in m_MenuItemSet)
            {
                item.Update(gameTime);
                if (!TPMath.IsWithinRange(item.Position, item.PositionTarget, 5.0f))
                {
                    item.Velocity = TPMath.GetDirectionVector(item.Position, item.PositionTarget) * 5;
                }
                else
                {
                    item.Velocity = Vector2.Zero;
                }

                if (item.Alpha < item.AlphaTarget)
                {
                    item.Alpha += 0.01f;
                }

                if (item.Alpha > item.AlphaTarget)
                {
                    item.Alpha -= 0.01f;
                }
                item.RenderColor = new Color((item.ColorTarget.R / 255) * item.Alpha, (item.ColorTarget.G / 255) * item.Alpha, (item.ColorTarget.B / 255) * item.Alpha, item.Alpha);

                if (!TPMath.IsWithinRange(item.Scale, item.ScaleTarget, 0.1f))
                {
                    item.Scale += TPMath.GetDirectionVector(item.Scale, item.ScaleTarget) / 20;
                }
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
                SetTargets();
                m_Delay = DELAY;
            }
        }


        public void MoveUp()
        {
            if (m_Delay <= 0)
            {
                m_Index++;
                if (m_Index > m_MenuItemSet.Count - 1)
                {
                    m_Index = m_MenuItemSet.Count - 1;
                }
                SetTargets();
                m_Delay = DELAY;
            }
        }

        private void SetTargets()
        {
            for (int i = 0; i < m_MenuItemSet.Count; i++)
            {
                if (i < m_Index - 2)
                {
                    m_MenuItemSet[i].PositionTarget.Y = this.Position.Y + OFFSETS[0];
                    m_MenuItemSet[i].AlphaTarget = ALPHAS[0];
                    m_MenuItemSet[i].ScaleTarget = SCALE_MINUTE;
                }
                if (i == m_Index - 2)
                {
                    m_MenuItemSet[i].PositionTarget.Y = this.Position.Y + OFFSETS[1];
                    m_MenuItemSet[i].AlphaTarget = ALPHAS[1];
                    m_MenuItemSet[i].ScaleTarget = SCALE_SMALL;
                }
                if (i == m_Index - 1)
                {
                    m_MenuItemSet[i].PositionTarget.Y = this.Position.Y + OFFSETS[2];
                    m_MenuItemSet[i].AlphaTarget = ALPHAS[2];
                    m_MenuItemSet[i].ScaleTarget = SCALE_MEDIUM;

                }
                if (i == m_Index)
                {
                    m_MenuItemSet[i].PositionTarget.Y = this.Position.Y + OFFSETS[3];
                    m_MenuItemSet[i].AlphaTarget = ALPHAS[3];
                    m_MenuItemSet[i].ScaleTarget = SCALE_LARGE;
                    //m_MenuItemSet[i].RenderColor = Color.White;
                }
                if (i == m_Index + 1)
                {
                    m_MenuItemSet[i].PositionTarget.Y = this.Position.Y + OFFSETS[4];
                    m_MenuItemSet[i].AlphaTarget = ALPHAS[2];
                    m_MenuItemSet[i].ScaleTarget = SCALE_MEDIUM;
                }
                if (i == m_Index + 2)
                {
                    m_MenuItemSet[i].PositionTarget.Y = this.Position.Y + OFFSETS[5];
                    m_MenuItemSet[i].AlphaTarget = ALPHAS[1];
                    m_MenuItemSet[i].ScaleTarget = SCALE_SMALL;
                }
                if (i > m_Index + 2)
                {
                    m_MenuItemSet[i].PositionTarget.Y = this.Position.Y + OFFSETS[6];
                    m_MenuItemSet[i].AlphaTarget = ALPHAS[0];
                    m_MenuItemSet[i].ScaleTarget = SCALE_MINUTE;
                }
                m_MenuItemSet[i].PositionTarget.X = this.Position.X + TPEngine.Get().FontManager.LoadFont(@"fonts\testfont").MeasureString(m_MenuItemSet[i].ToString()).X / 4;
            }
        }


        public void Select()
        {
            m_MenuItemSet[m_Index].Select();
        }

        /// <summary>
        /// Manual override. 
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            foreach (TPRollingTextMenuItem item in m_MenuItemSet)
            {
                item.Draw(batch);
            }

            m_MenuItemSet[m_Index].Draw(batch);
        }

        private static void DoNothingCallback() { }

    }
}
