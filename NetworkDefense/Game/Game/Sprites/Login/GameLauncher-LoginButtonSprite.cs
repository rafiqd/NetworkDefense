using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Engine.StateManagement;
using Microsoft.Xna.Framework.Input;
using Engine.Collision;
using Engine;
using System.Runtime.InteropServices;
using Game.States;
using DBCommService;

namespace Game.Sprites
{
    /// <summary>
    /// class that contains the logic and methods to draw out a login button
    /// </summary>
    class GameLauncher_LoginButtonSprite : TPSprite
    {

        /// <summary>
        /// Allows to display a message box
        /// </summary>
        /// <param name="hWnd">handle to the message box</param>
        /// <param name="text">text you want to display in the message box</param>
        /// <param name="caption">text you want to display on the title bar of the message box</param>
        /// <param name="type">the type of message box</param>
        /// <returns>returns a messagebox</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        /// <summary>
        /// string used to display the button text
        /// </summary>
        TPString buttonText;

        /// <summary>
        /// sprite that is used to draw out the button
        /// </summary>
        TPSprite loginButtonSprite;

        /// <summary>
        /// the email textbox object
        /// </summary>
        GameLauncher_EmailSprite emailSprite;

        /// <summary>
        /// the password textbox object
        /// </summary>
        GameLauncher_PasswordSprite passwordSprite;

        /// <summary>
        /// User object used for setting game score
        /// </summary>
        static User user = new User();

        /// <summary>
        /// string that contains the values of the email and password textboxes
        /// </summary>
        string email,
               password;

        /// <summary>
        /// status of the button
        /// </summary>
        ButtonStatus Status = ButtonStatus.Up;

        /// <summary>
        /// mouse sprite object, contains the logic for a mouse
        /// </summary>
        GameLauncher_MousePointerSprite mousePointer;

        /// <summary>
        /// used to store the previous mouse action
        /// </summary>
        MouseState previousMouseState;

        /// <summary>
        /// constructor, initializes all the components of login button
        /// </summary>
        /// <param name="gameEmailTextBox">the email textbox class object</param>
        /// <param name="gamePasswordTextBox">the password textbox class object</param>
        /// <param name="mouse">the mouse class object</param>
        /// <param name="text">the text on the login button</param>
        public GameLauncher_LoginButtonSprite(GameLauncher_EmailSprite gameEmailTextBox, GameLauncher_PasswordSprite gamePasswordTextBox, GameLauncher_MousePointerSprite mouse, string text)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/launcher/borderPixel"))
        {

            mousePointer = mouse;
            emailSprite = gameEmailTextBox;
            passwordSprite = gamePasswordTextBox;

            loginButtonSprite = new TPSprite(@"art/launcher/button-Up");

            buttonText = new TPString(text);
            buttonText.Scale = new Vector2(0.8f, 0.8f);
            buttonText.Position = new Vector2(515, 632);
            buttonText.RenderColor = Color.Black;


            previousMouseState = Mouse.GetState();

        }

        /// <summary>
        /// method to update the game
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Recognize a single click or button is held of the left mouse button
            if ((Mouse.GetState().LeftButton == ButtonState.Released || previousMouseState.LeftButton == ButtonState.Pressed) && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                if (TPCollider.Test(mousePointer, loginButtonSprite))
                {
                    Status = ButtonStatus.Down;

                    login();
                }
            }
            if (previousMouseState.LeftButton == ButtonState.Released && TPCollider.Test(mousePointer, loginButtonSprite))
            {
                Status = ButtonStatus.Up;
            }

            if (previousMouseState.LeftButton == ButtonState.Released && !TPCollider.Test(mousePointer, loginButtonSprite))
            {
                Status = ButtonStatus.Up;
            }

            previousMouseState = Mouse.GetState();
        }

        /// <summary>
        /// grabs the current user
        /// </summary>
        /// <returns>current user</returns>
        public static User getUser()
        {
            return user;
        }

        /// <summary>
        /// draws the sprites
        /// </summary>
        /// <param name="batch">used to draw</param>
        public override void Draw(SpriteBatch batch)
        {
            if (Status == ButtonStatus.Down)
            {
                loginButtonSprite = new TPSprite(@"art/launcher/button-Down");
                loginButtonSprite.Position = new Vector2(470, 625);
                loginButtonSprite.Draw(batch);
            }
            else
            {
                loginButtonSprite = new TPSprite(@"art/launcher/button-Up");
                loginButtonSprite.Position = new Vector2(470, 625);
                loginButtonSprite.Draw(batch);
            }


            buttonText.Draw(batch);
        }

        /// <summary>
        /// method to login into the game
        /// </summary>
        public void login()
        {
            try
            {
                email = emailSprite.getEmailString();
                password = passwordSprite.getPasswordString();

                if (email == "" || password == "")
                {
                    MessageBox(new IntPtr(0), "Missing fields!", "Error", 0);
                }
                else
                {
                    using (DBCommServiceClient client = new DBCommServiceClient())
                    {
                        user = client.GetUserDetails(email, password);

                        if (user == null)
                        {
                            MessageBox(new IntPtr(0), "Email or Password is wrong", "Authentication Error", 0);
                        }
                        else
                        {
                            //Load the main menu
                            TPEngine.Get().State.ChangeState(new MainMenu());
                        }
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                //display the error message
                MessageBox(new IntPtr(0), "All information must be filled out!", ex.Message, 0);
            }
        }

    }
}
