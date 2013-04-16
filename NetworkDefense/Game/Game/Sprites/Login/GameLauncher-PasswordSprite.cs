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
    /// class that contains all the methods and logic for drawing the password textbox
    /// </summary>
    class GameLauncher_PasswordSprite : TPSprite
    {
        /// <summary>
        /// texture used to hold a image that is used for drawing a border around a textbox
        /// </summary>
        Texture2D borderTexture;

        /// <summary>
        /// rectangle to hold the size of the password textbox
        /// </summary>
        Rectangle rectanglePassword;

        /// <summary>
        /// flag used to see what textbox is being selected right now
        /// </summary>
        bool isSelected = false;

        /// <summary>
        /// string that holds the value of the password
        /// </summary>
        string passwordLetter;

        /// <summary>
        /// class object for mousepointer
        /// </summary>
        GameLauncher_MousePointerSprite mousepointer;

        /// <summary>
        /// sprite used to draw the password textbox
        /// </summary>
        TPSprite rectanglePasswordSprite;

        /// <summary>
        /// sprite used to draw the email text box
        /// </summary>
        TPSprite rectangleEmailSprite;

        /// <summary>
        /// the string that will be grabbing the keystrokes
        /// </summary>
        private String inputStr;

        /// <summary>
        /// the password string that will grabbing the keystrokes
        /// </summary>
        private String passStr;

        /// <summary>
        /// used to the password and the label for password
        /// </summary>
        TPString password,
                 passwordLabel;

        /// <summary>
        /// previoius keyboard keystroke 
        /// </summary>
        KeyboardState previousKBState;

        /// <summary>
        /// previous mouse state
        /// </summary>
        MouseState previousMouseState;

        /// <summary>
        ///  The maximum length of the string
        /// </summary>
        const int MAX_LENGTH = 5000;

        /// <summary>
        /// The constructor sets the Email Sprite.
        /// </summary>
        public GameLauncher_PasswordSprite(GameLauncher_MousePointerSprite mouse, TPSprite rectangleSprite, string inputLetter)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/launcher/rectangleTexture"))
        {
            mousepointer = mouse;
            rectangleEmailSprite = rectangleSprite;

            passwordLetter = inputLetter;

            borderTexture = TPEngine.Get().TextureManager.LoadTexture(@"art/launcher/borderPixel");
            rectanglePassword = new Rectangle(525, 560, 200, 35);

            rectanglePasswordSprite = new TPSprite(@"art/launcher/rectangleTexture");
            rectanglePasswordSprite.Position = new Vector2(528, 563);

            passwordLabel = new TPString("Password: ");
            passwordLabel.Position = new Vector2(525, 530);
            passwordLabel.Scale = new Vector2(0.8f, 0.8f);
            passwordLabel.RenderColor = Color.Black;

            password = new TPString("");
            password.RenderColor = Color.Black;
            inputStr = "";
            passStr = "";

            password.Position = new Vector2(535, 560);
            password.Scale = new Vector2(0.7f, 0.7f);

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
                if (TPCollider.Test(mousepointer, rectanglePasswordSprite))
                {
                    isSelected = true;
                }
                else if (TPCollider.Test(mousepointer, rectangleEmailSprite))
                {
                    isSelected = false;
                }
            }
            HandleInput(gameTime, passwordLetter);

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
                        if (inputStr.Length > 0 || passStr.Length > 0)
                        {
                            // Strip the last character off the string
                            inputStr = inputStr.Substring(0, inputStr.Length - 1);
                            passStr = passStr.Substring(0, passStr.Length - 1);
                        }
                    }
                    else
                    {
                        if (inputStr.Length < MAX_LENGTH || passStr.Length < MAX_LENGTH)
                        {
                            // Add the character to the end off the string
                            inputStr += letter;
                            passStr += '*';
                        }
                    }
                }

                // Clear the TPString object and append the updated string
                password.Clear();

                if (inputStr.Length > 15 || passStr.Length > 15)
                {
                    password.Append(passStr.Substring(passStr.Length - 15));
                }
                else
                {
                    password.Append(passStr);
                }
            }
        }

        /// <summary>
        /// methods to get the password
        /// </summary>
        /// <returns>password</returns>
        public string getPasswordString()
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
        /// method to un set a textbox selection
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
            DrawRectangleBorder(batch, rectanglePassword, 3, Color.Black);
            rectanglePasswordSprite.Draw(batch);

            passwordLabel.Draw(batch);
            password.Draw(batch);
        }
    }
}
