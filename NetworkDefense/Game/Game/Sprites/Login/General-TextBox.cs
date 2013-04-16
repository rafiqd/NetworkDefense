using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Engine;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Engine.StateManagement;
using Engine.UI;
using Engine.Collision;

namespace Game.Sprites
{
    class General_TextBox : TPSprite
    {
        //texture for the border
        Texture2D borderTexture;

        //rectangle used for creating the border around the textbox
        Rectangle rectangle;

        //checks to see if the textbox has been selected or not
        bool isSelected = false;

        //Need some type of Mouse Sprite to used with collision code
        MousePointerSprite mousepointer;

        //Sprite to hold the texture of your textbox
        TPSprite rectangleTextBox;

        //used to store the keystrokes
        private String inputStr;

        //strings used to display the label and the text in the text box
        TPString text,
                 textLabel;

        //Grab previous key stroke
        KeyboardState previousKBState;

        //Grab previous mouse state ie. Pressed or Released
        MouseState previousMouseState;

        // The maximum length of the string
        const int MAX_LENGTH = 5000;

        /// <summary>
        /// The constructor sets the Email Sprite.
        /// Your going to have to provide your own textures for everything, rectangle texture is a 200 by 50 pixel white rectangle created in paint.
        ///                         Pass in your mouse sprite,        Pass in your textbox, Pos of textbox,  Color of textbox
        /// </summary>
        public General_TextBox(MousePointerSprite mouse, TPSprite rectTextBox, Vector2 rectPos, Color rectColor)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/rectangleTexture"))
        {

            //assigns the mouse, used to check for mouse collisions
            mousepointer = mouse;

            //set up the text box
            rectangleTextBox = rectTextBox;
            rectangleTextBox.Position = rectPos;
            rectangleTextBox.RenderColor = rectColor;

            //border pixel is a 1x1 white pixel
            borderTexture = TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/borderPixel");

            //set up rectangle used to draw border around textbox
            //Play these settings to get the border around your box right
            rectangle = new Rectangle((int)rectangleTextBox.Position.X - 3, (int)rectangleTextBox.Position.Y - 3, 200, 35);

            //Play with these settings to move, scale and change color of label and where the string is printed
            //           Label Text,   Scaling factor,            Position of Label,  Text Color
            setLabelText("Name", new Vector2(0.8f, 0.8f), new Vector2(100, 90), Color.Black);

            //       Scaling factor,         Position of String       Text Color
            setText(new Vector2(0.7f, 0.7f), new Vector2(58, 100), Color.Black);

            //assign nothing to the string for now
            inputStr = "";

            //remember the previous states for keyboard and mouse
            previousKBState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();
        }

        public void setText(Vector2 textScaler, Vector2 textPos, Color textColor)
        {
            //set up TPstring that displays the keystrokes on textbox
            text = new TPString("");
            text.RenderColor = textColor;
            text.Position = textPos;
            text.Scale = textScaler;
        }

        public void setLabelText(string labelString, Vector2 labelScaler, Vector2 labelPos, Color labelColor)
        {
            //set up TPstring that displays the label above the textbox
            textLabel = new TPString(labelString);
            textLabel.Position = labelPos;
            textLabel.Scale = labelScaler;
            textLabel.RenderColor = labelColor;
        }

        private void DrawRectangleBorder(SpriteBatch spriteBatch, Rectangle rectangleToDraw, int borderWidth, Color borderColor)
        {
            // Draw top line
            spriteBatch.Draw(borderTexture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, borderWidth), borderColor);
            // Draw left line
            spriteBatch.Draw(borderTexture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, borderWidth, rectangleToDraw.Height), borderColor);
            // Draw right line
            spriteBatch.Draw(borderTexture, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - borderWidth), rectangleToDraw.Y, borderWidth, rectangleToDraw.Height), borderColor);
            // Draw bottom line
            spriteBatch.Draw(borderTexture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y + rectangleToDraw.Height - borderWidth, rectangleToDraw.Width, borderWidth), borderColor);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Tests to see if you clicked within the textbox
            if (Mouse.GetState().LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                if (TPCollider.Test(mousepointer, rectangleTextBox))
                {
                    isSelected = true;
                }
                else if (TPCollider.Test(mousepointer, rectangleTextBox))
                {
                    isSelected = false;
                }
            }

            //grab the keystrokes
            HandleInput(gameTime);

            //remember previous states
            previousKBState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();
        }

        private void HandleInput(GameTime gameTime)
        {
            string letter;

            //if the textbox is currently selected...
            if (isSelected)
            {
                // Get keyboard character.
                letter = TPTypeManager.getChar(Keyboard.GetState(), previousKBState).ToString();

                // © denotes no character typed, ß denoted backspace typed.
                if (letter != "©")
                {
                    if (letter == "ß")
                    {
                        if (inputStr.Length > 0)
                        {
                            // Strip the last character off the string
                            inputStr = inputStr.Substring(0, inputStr.Length - 1);
                        }
                    }
                    else
                    {
                        if (inputStr.Length < MAX_LENGTH)
                        {
                            // Add the character to the end off the string
                            inputStr += letter;
                        }
                    }
                }

                // Clear the TPString object and append the updated string
                text.Clear();

                //allows only 15 characters to be shown at once, pushes right to left of screen
                if (inputStr.Length > 15)
                {
                    text.Append(inputStr.Substring(inputStr.Length - 15));
                }
                else
                {
                    text.Append(inputStr);
                }
            }
        }

        //return the actual string typed
        public string getTextBoxString()
        {
            return inputStr;
        }

        /// <summary>
        /// Return the input as a string
        /// </summary>
        /// <returns></returns>
        public string getinputString()
        {
            return inputStr;
        }

        /// <summary>
        /// Draws the sprite.
        /// </summary>
        /// <param name="spriteBatch">used to draw out the sprite.</param>
        public override void Draw(SpriteBatch batch)
        {
            //draw the border
            DrawRectangleBorder(batch, rectangle, 3, Color.Black);

            //draw the textbox
            rectangleTextBox.Draw(batch);

            //draw the label
            //textLabel.Draw(batch);

            //draw the text
            text.Draw(batch);
        }
    }
}
