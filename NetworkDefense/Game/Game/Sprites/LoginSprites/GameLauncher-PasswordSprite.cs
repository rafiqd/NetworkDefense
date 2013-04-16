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
    class GameLauncher_PasswordSprite : TPSprite
    {
        Texture2D borderTexture;
        Rectangle rectanglePassword;

        bool isSelected = false;
        string passwordLetter;

        GameLauncher_MousePointerSprite mousepointer;

        TPSprite rectanglePasswordSprite;
        TPSprite rectangleEmailSprite;

        private String inputStr;
        private String passStr;

        TPString password,
                 passwordLabel;

        KeyboardState previousKBState;
        MouseState previousMouseState;

        // The maximum length of the string
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

        public string getPasswordString() 
        {
            return inputStr;
        }

        public void selectMe()
        {
            isSelected = true;
        }

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
