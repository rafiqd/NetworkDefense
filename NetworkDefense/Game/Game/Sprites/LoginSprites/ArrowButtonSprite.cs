using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Engine.StateManagement;
using Microsoft.Xna.Framework.Input;

namespace Game.Sprites
{
    /// <summary>
    /// A button with an arrow texture. It also contains a button action that holds 
    /// a function to be executed when the button is clicked.
    /// </summary>
    class ArrowButtonSprite : TPSprite
    {
        private Rectangle drawRect;
        private Action buttonAction;

        //Mouse related
        MouseState currentMouseState;
        MouseState lastMouseState;
        bool mouseOver = false;

        /// <summary>
        /// Get and set the button's action.
        /// </summary>
        public Action ButtonAction
        {
            get { return buttonAction; }
            set { buttonAction = value; }
        }

        /// <summary>
        /// Constructor takes in texture and a rect structure.
        /// </summary>
        /// <param name="texture">The texture of the button</param>
        /// <param name="rect">The rect structure that contains the position and the size of the button</param>
        public ArrowButtonSprite(Texture2D texture, Rectangle rect)
            : base(texture)
        {
            Position.X = rect.X;
            Position.Y = rect.Y;
            drawRect = rect;
        }

        /// <summary>
        /// Check for mouseover and mouseclick on the button.
        /// </summary>
        /// <param name="gameTime">Time of the game</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // The active state from the last frame is now old 
            lastMouseState = currentMouseState;

            // Get the mouse state relevant for this frame 
            currentMouseState = Mouse.GetState();

            if (drawRect.X <= Mouse.GetState().X && (drawRect.X + drawRect.Width) >= Mouse.GetState().X &&
                drawRect.Y <= Mouse.GetState().Y && (drawRect.Y + drawRect.Height) >= Mouse.GetState().Y)
            {
                //Recognize a single click of the left mouse button
                if (buttonAction != null && lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                    buttonAction();

                mouseOver = true;
            }
            else
                mouseOver = false;
        }

        /// <summary>
        /// Draw the mouseOverTexture when the mouse pointer is on the button.
        /// </summary>
        /// <param name="batch">SpriteBatch for drawing the button</param>
        public override void Draw(SpriteBatch batch)
        {
            //Change the sprite colour to black if mouseover
            if(!mouseOver)
                batch.Draw(m_Texture, Position, null, drawColor, Rotation, RotationOrigin, Scale, Effect, 1.0f);
            else
                batch.Draw(m_Texture, Position, null, Color.Black, Rotation, RotationOrigin, Scale, Effect, 1.0f);

        }
    }
}
