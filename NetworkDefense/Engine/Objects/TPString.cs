using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine.Objects
{
    public class TPString : TPVisibleEntity
    {
        /// <summary>
        /// Our internal string representation of this class.
        /// </summary>
        protected StringBuilder m_CurrentString = new StringBuilder();
        /// <summary>
        /// The font that this class uses.
        /// </summary>
        private SpriteFont m_SpriteFont;
        /// <summary>
        /// Used to get the length of this sprite in pixel units. 
        /// </summary>
        public Vector2 Size { get { return m_SpriteFont.MeasureString(m_CurrentString); }  }
        /// <summary>
        /// Determines whether or not the string will be centered around the position, or drawn starting at the position.
        /// </summary>
        public bool centerText { get; set; }
        /// <summary>
        /// A half size getter used for convienince. This allows us to have center alignment very easily. 
        /// </summary>
        public Vector2 HalfSize { get { return Size / 2; } }
        /// <summary>
        /// Property for our internal font field. 
        /// </summary>
        public SpriteFont Font
        {
            get { return m_SpriteFont; }
            set { m_SpriteFont = value; }
        }
        public TPString(string inString) : this(inString, Color.White) { }
        /// <summary>
        /// Ctor. This uses a default XNA font.
        /// </summary>
        /// <param name="inString">String we wish to display.</param>
        /// <param name="color">Color of the string.</param>
        public TPString(string inString, Color color):this(inString,TPEngine.Get().FontManager.LoadFont(@"fonts\testfont"),color) { }
        /// <summary>
        /// Ctor. Uses a default color black.
        /// </summary>
        /// <param name="inString">String we wish to display.</param>
        /// <param name="font">The font we wish to use.</param>
        public TPString(string inString,SpriteFont font)
            : this(inString,font,Color.Black) { }
        /// <summary>
        /// Ctor. FUlly customized version.
        /// </summary>
        /// <param name="inString">String we wish to display.</param>
        /// <param name="font">The font we wish to use.</param>
        /// <param name="color">Color of the string.</param>
        public TPString(string inString, SpriteFont font, Color color) 
        {
            m_CurrentString.Append(inString);
            Font = font;
            this.RenderColor = color;
            centerText = false;
        }
        /// <summary>
        /// Append function. Will appent a string to the end of this string.
        /// </summary>
        /// <param name="inString"></param>
        public void Append(string inString)
        {
            m_CurrentString.Append(inString);
        }
        /// <summary>
        /// Append function. Char version.
        /// </summary>
        /// <param name="inChar"></param>
        public void Append(char inChar)
        {
            m_CurrentString.Append(inChar.ToString());
        }
        /// <summary>
        /// Append function. TPString version.
        /// </summary>
        /// <param name="inChar"></param>
        public void Append(TPString inTPString)
        {
            m_CurrentString.Append(inTPString.ToString());
        } 
        /// <summary>
        /// Repalcement, very inefficeint, will need to make a clear extension method for stringbuilder
        /// </summary>
        /// <param name="inString"></param>
        public void Replace(string inString)
        {
            m_CurrentString = new StringBuilder(inString);
        }
        /// <summary>
        /// ToString override.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_CurrentString.ToString();
        }
        /// <summary>
        /// Clears the current string.
        /// </summary>
        public void Clear()
        {
            m_CurrentString.Clear();
        }
        public override void Draw(SpriteBatch batch)
        {
            //batch.DrawString(m_SpriteFont, this.ToString(), this.position, RenderColor);
            //if (Visible)
            //{
            if (centerText)
            {
                batch.DrawString(m_SpriteFont, this.ToString(), this.Position, this.RenderColor, this.Rotation, HalfSize, this.Scale, SpriteEffects.None, 1.0f);
            }
            else
            {
                batch.DrawString(m_SpriteFont, this.ToString(), this.Position, this.RenderColor, this.Rotation, Vector2.Zero, this.Scale, SpriteEffects.None, 1.0f);
            }
            //}
        }
    }
}
