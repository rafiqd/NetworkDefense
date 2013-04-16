using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Engine.Objects;
using Microsoft.Xna.Framework.Graphics;
using Game.States.TDsrc.Management;
using Microsoft.Xna.Framework.Input;
using Game.States.TDsrc.Towers;


namespace Game.States.TDsrc.UI
{
    /// <summary>
    /// type of function that is passed to a button
    /// </summary>
    /// <param name="name"></param>
    delegate void OnclickFunction(string name);

    /// <summary>
    /// Class represents a button on the screen
    /// </summary>
    class Button : TPSprite
    {
        /// <summary>
        /// if the button is clicked
        /// </summary>
        public bool clicked;

        /// <summary>
        /// name of the button
        /// </summary>
        public string Name;

        /// <summary>
        /// function the button calls when clicked.
        /// </summary>
        public OnclickFunction onClick;

        /// <summary>
        /// if the button is visible
        /// </summary>
        public bool visible;

        /// <summary>
        /// Button constructor
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <param name="Buttons">texture of the button</param>
        /// <param name="name">name of the button</param>
        /// <param name="function">function the button calls when clicked</param>
        public Button(int x, int y, Texture2D Buttons, string name, OnclickFunction function)
            :base(Buttons)
        {
            Name = name;
            m_Texture = Buttons;
            Position.X = x;
            Position.Y = y;
            TowerDefenseManager.TDLayers[4].AddEntity(this);
            onClick = function;
        }

        /// <summary>
        /// Updates this button
        /// </summary>
        /// <param name="gametime">current game time</param>
        /// <param name="keyboard">keyboard state</param>
        /// <param name="mouse">mouse state</param>
        public void update(GameTime gametime, KeyboardState keyboard, MouseState mouse)
        {
            if (!visible)
                return;

            if (mouse.LeftButton == ButtonState.Pressed &&
                !clicked &&
                mouse.X < Position.X + m_Texture.Width && mouse.X > Position.X &&
                mouse.Y < Position.Y + m_Texture.Height && mouse.Y > Position.Y )
            {
                onClick(Name);
                clicked = true;
            }

            if (mouse.LeftButton == ButtonState.Released)
                clicked = false;
        }

        /// <summary>
        /// draws this button
        /// </summary>
        /// <param name="batch">current sprite batch</param>
        public override void Draw(SpriteBatch batch)
        {
            if(visible)
            batch.Draw(m_Texture, Position, Color.White);
        }

    }
}
