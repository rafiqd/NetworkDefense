using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Game.States.TDsrc.Management;
using Microsoft.Xna.Framework.Input;


namespace Game.States.TDsrc.UI
{
    
    /// <summary>
    /// Window class holds buttons 
    /// </summary>
    class Window : TPSprite
    {
        /// <summary>
        /// is the window visible
        /// </summary>
        public bool Visible;

        /// <summary>
        /// list of the buttons the window holds
        /// </summary>
        public List<Button> ButtonList;

        /// <summary>
        /// x offset of the buttons being placed
        /// </summary>
        int Xoffset;

        /// <summary>
        /// y offset of the buttons being placed
        /// </summary>
        int Yoffset;

        /// <summary>
        /// Window constructor
        /// </summary>
        /// <param name="xpos">x position to build window at</param>
        /// <param name="ypos">y position to build window at</param>
        /// <param name="visible">start off as visible or invisible</param>
        /// <param name="texture">texture of the window</param>
        public Window(int xpos, int ypos, bool visible, Texture2D texture)
            :base(texture)
        {
            Position.X = xpos;
            Position.Y = ypos;
            Visible = visible;
            ButtonList = new List<Button>();
            m_Texture = texture;
            TowerDefenseManager.TDLayers[3].AddEntity(this);
            WindowList.Add(this);
            Xoffset = 10 + (int)Position.X;
            Yoffset = 10 + (int)Position.Y;
        }

        /// <summary>
        /// Adds a button(s) to the window
        /// </summary>
        /// <param name="Buttons">the texture for the buttons</param>
        /// <param name="names">the name of the buttons(currently use for switch statements, see TowerBuilder</param>
        /// <param name="functions">the function the button should call when clicked</param>
        public void addButton( Texture2D button, string name, OnclickFunction function)
        {
            if (name != "BuyButton")
            {
                if (Xoffset + button.Width > Position.X + m_Texture.Width)
                {
                    Xoffset = 10 + (int)Position.X;
                    Yoffset += button.Width + 10;
                }
                ButtonList.Add(new Button(Xoffset, Yoffset, button, name, function));
                Xoffset += button.Width + 10;
            }
            else // specific case for the buy button to go to the right
            {
                ButtonList.Add(new Button(m_Texture.Width - 10 - button.Width, (int)Position.Y + 10, button, name, function));
            }


        }

        /// <summary>
        /// updates this window and all it's buttons
        /// </summary>
        /// <param name="gametime">current game time</param>
        /// <param name="keyboard">keyboard state</param>
        /// <param name="mouse">mouse state</param>
        public void update(GameTime gametime, KeyboardState keyboard, MouseState mouse)
        {
            // window should update all of it's buttons
            for (int i = 0; i < ButtonList.Count; ++i)
            {
                ButtonList[i].visible = Visible;
                ButtonList[i].update(gametime, keyboard, mouse);
            }
        }

        /// <summary>
        /// Draws this window
        /// </summary>
        /// <param name="batch">current spritebatch</param>
        public override void Draw(SpriteBatch batch)
        {
            if(Visible)
                batch.Draw(m_Texture, Position, Color.White);
        }

        #region static methods and variables

        /// <summary>
        /// List of all the windows
        /// </summary>
        public static List<Window> WindowList;

        /// <summary>
        /// static initalization
        /// </summary>
        static Window()
        {
            WindowList = new List<Window>();
        }

        /// <summary>
        /// Updates all the windows.
        /// </summary>
        /// <param name="gametime"></param>
        public static void Update(GameTime gametime, KeyboardState keyboard, MouseState mouse)
        {
            for (int i = 0; i < WindowList.Count; ++i)
            {
                WindowList[i].update(gametime, keyboard, mouse);
            }
        }


#endregion 
    }
}
