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
    /// Button contians a text on it. It also has a button action that holds a function to be
    /// executed when the button is clicked.
    /// </summary>
    class ButtonSprite : TPSprite
    {
        private TPString buttonText;
        private Rectangle drawRect;
        private TPLayer currentLayer;
        private Color textColor;
        private Action buttonAction;
        private Texture2D mouseOverTexture;
        private bool centerText;
        private bool mouseOver;
        private bool disabled;


        //Mouse related
        MouseState currentMouseState;
        MouseState lastMouseState;

        /// <summary>
        /// Get and set the button action by a function pointer.
        /// </summary>
        public Action ButtonAction
        {
            get { return buttonAction; }
            set { buttonAction = value; }
        }

        /// <summary>
        /// A boolean that specific whether the button text is algin center or not
        /// </summary>
        public bool CenterText
        {
            get { return centerText; }
            set
            {
                centerText = value;
                if (buttonText != null)
                    buttonText.centerText = value;
                setTextAlignment();
            }
        }

        /// <summary>
        /// A boolean that specific whether the button is disabled or not.
        /// If so, the button is disappeared on the screen.
        /// </summary>
        public bool Disabled
        {
            get { return disabled; }
            set { disabled = value; if (value) buttonText.Clear(); }
        }

        /// <summary>
        /// Constructor that takes in two textures, a rect structure, a text, and a layer.
        /// </summary>
        /// <param name="texture">Main texture of the button</param>
        /// <param name="mouseTexture">The texture appears when the mouse pointer is on it</param>
        /// <param name="rect">The rectangle that specifics the position and size of the button</param>
        /// <param name="text">The button text</param>
        /// <param name="layer">The layer that this button will be put on</param>
        public ButtonSprite(Texture2D texture, Texture2D mouseOverTexture, Rectangle rect, string text, TPLayer layer)
            : base(texture)
        {
            Position.X = rect.X;
            Position.Y = rect.Y;
            this.mouseOverTexture = mouseOverTexture; 
            drawRect = rect;
            buttonText = new TPString(text);
            textColor = Color.White;
            currentLayer = layer;
            centerText = false;
            mouseOver = false;
            disabled = false;
            setText();
            drawColor = Color.SlateGray;
        }

        /// <summary>
        /// Set the button text on the button.
        /// </summary>
        private void setText()
        {
            setTextAlignment();
            buttonText.centerText = centerText;
            buttonText.RenderColor = textColor;
            currentLayer.AddEntity(buttonText);
        }

        /// <summary>
        /// Set the text alignment(center or left) on the button.
        /// </summary>
        private void setTextAlignment()
        {
            if (centerText)
            {
                buttonText.Position.X = drawRect.X + drawRect.Width / 2;
                buttonText.Position.Y = drawRect.Y + drawRect.Height / 2;
            }
            else
            {
                buttonText.Position.X = Position.X + drawRect.Width / 10;
                buttonText.Position.Y = Position.Y + drawRect.Height / 2 - buttonText.HalfSize.Y;
            }
        }

        /// <summary>
        /// Check for mouseover and mouseclick on the button when the button is not disabled.
        /// </summary>
        /// <param name="gameTime">Time of the game</param>
        public override void Update(GameTime gameTime)
        {
            if (!disabled && !Asleep)
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
        }

        /// <summary>
        /// Draw the mouseOverTexture when the mouse pointer is on the non-disabled button.
        /// </summary>
        /// <param name="batch">SpriteBatch for drawing the button</param>
        public override void Draw(SpriteBatch batch)
        {
            if (!disabled && !Asleep)
            {
                //Change the sprite colour to black if mouseover
                if (!mouseOver)
                    batch.Draw(m_Texture, drawRect, null, drawColor, Rotation, RotationOrigin, Effect, 1.0f);
                else
                    batch.Draw(mouseOverTexture, drawRect, null, drawColor, Rotation, RotationOrigin, Effect, 1.0f);
            }
        }
    }
}
