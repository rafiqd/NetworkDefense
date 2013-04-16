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
    /// <summary>
    /// class that contains all the logic and method to draw out the email text box
    /// </summary>
    class GameLauncher_EmailSprite : TPSprite
    {
        /// <summary>
        /// texture that contains the image for the border
        /// </summary>
        Texture2D borderTexture;

        /// <summary>
        /// rectangle used to store the positions of the textbox
        /// </summary>
        Rectangle rectangleEmail;

        /// <summary>
        /// flag to denote if a textbox has been selected or not
        /// </summary>
        bool isSelected = false;

        /// <summary>
        /// string that contains the string value of the email textbox
        /// </summary>
        string emailLetter;

        /// <summary>
        /// class object used for displaying a mouse pointer
        /// </summary>
        GameLauncher_MousePointerSprite mousepointer;

        /// <summary>
        /// sprite object for drawing out the email textbox
        /// </summary>
        TPSprite rectangleEmailSprite;

        /// <summary>
        /// sprite object for drawing out the password textbox
        /// </summary>
        TPSprite rectanglePasswordSprite;

        /// <summary>
        /// string that store the keystrokes
        /// </summary>
        private String inputStr;

        /// <summary>
        /// used to display the email string and the email label string
        /// </summary>
        TPString email,
                 emailLabel;

        /// <summary>
        /// stores the previous keystroke
        /// </summary>
        KeyboardState previousKBState;

        /// <summary>
        /// stores the previous mouse state
        /// </summary>
        MouseState previousMouseState;

        /// <summary>
        ///  The maximum length of the string
        /// </summary>
        const int MAX_LENGTH = 5000;

        /// <summary>
        /// The constructor sets the Email Sprite.
        /// </summary>
        public GameLauncher_EmailSprite(GameLauncher_MousePointerSprite mouse, TPSprite rectangleSprite, string inputLetter)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/launcher/rectangleTexture"))
        {
            mousepointer = mouse;
            rectanglePasswordSprite = rectangleSprite;

            emailLetter = inputLetter;

            borderTexture = TPEngine.Get().TextureManager.LoadTexture(@"art/launcher/borderPixel");
            rectangleEmail = new Rectangle(525, 480, 200, 35);

            rectangleEmailSprite = new TPSprite(@"art/launcher/rectangleTexture");
            rectangleEmailSprite.Position = new Vector2(528, 483);

            emailLabel = new TPString("Email: ");
            emailLabel.Position = new Vector2(525, 450);
            emailLabel.Scale = new Vector2(0.8f, 0.8f);
            emailLabel.RenderColor = Color.Black;

            email = new TPString("");
            email.RenderColor = Color.Black;
            inputStr = "";

            email.Position = new Vector2(535, 480);
            email.Scale = new Vector2(0.7f, 0.7f);

            previousKBState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();
        }

        /// <summary>
        /// method used to draw a border around a textbox
        /// </summary>
        /// <param name="spriteBatch">used to draw</param>
        /// <param name="rectangleToDraw">rectangle used to draw the border around</param>
        /// <param name="borderWidth">thickness of the border</param>
        /// <param name="borderColor">border color</param>
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

        /// <summary>
        /// method to update the game, contains logic for collision
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Mouse.GetState().LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                if (TPCollider.Test(mousepointer, rectangleEmailSprite))
                {
                    isSelected = true;
                }
                else if (TPCollider.Test(mousepointer, rectanglePasswordSprite))
                {
                    isSelected = false;
                }
            }

            HandleInput(gameTime, emailLetter);

            previousKBState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();
        }

        /// <summary>
        /// method to grab all the inputs from the keyboard
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="letter">contains the character of the keystroke</param>
        private void HandleInput(GameTime gameTime, string letter)
        {
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
                email.Clear();

                if (inputStr.Length > 15)
                {
                    email.Append(inputStr.Substring(inputStr.Length - 15));
                }
                else
                {
                    email.Append(inputStr);
                }
            }
        }

        /// <summary>
        /// methods to get the email
        /// </summary>
        /// <returns>email</returns>
        public string getEmailString()
        {
            return inputStr;
        }

        /// <summary>
        /// method to set a textbox selection
        /// </summary>
        public void selectMe()
        {
            isSelected = true;
        }

        /// <summary>
        /// method to untset a textbox selection
        /// </summary>
        public void unselectMe()
        {
            isSelected = false;
        }

        /// <summary>
        /// Draws the sprite.
        /// </summary>
        /// <param name="spriteBatch">used to draw out the sprite.</param>
        public override void Draw(SpriteBatch batch)
        {
            DrawRectangleBorder(batch, rectangleEmail, 3, Color.Black);
            rectangleEmailSprite.Draw(batch);

            emailLabel.Draw(batch);
            email.Draw(batch);
        }
    }
}
