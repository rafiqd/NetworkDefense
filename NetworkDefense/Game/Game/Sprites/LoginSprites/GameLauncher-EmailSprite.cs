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
    class GameLauncher_EmailSprite : TPSprite
    {
        Texture2D borderTexture;
        Rectangle rectangleEmail;

        bool isSelected = false;
        string emailLetter;

        GameLauncher_MousePointerSprite mousepointer;
        TPSprite rectangleEmailSprite;
        TPSprite rectanglePasswordSprite;

        private String inputStr;

        TPString email,
                 emailLabel;

        KeyboardState previousKBState;
        MouseState previousMouseState;

        // The maximum length of the string
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

        public string getEmailString()
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
            DrawRectangleBorder(batch, rectangleEmail, 3, Color.Black);
            rectangleEmailSprite.Draw(batch);

            emailLabel.Draw(batch);
            email.Draw(batch);
        }
    }
}
