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

    class GameLauncher_LoginButtonSprite: TPSprite
    {

        //Allows to display a message box
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        TPString buttonText;
        TPSprite loginButtonSprite;
        GameLauncher_EmailSprite emailSprite;
        GameLauncher_PasswordSprite passwordSprite;

        static User user = new User();

        string email,
               password;

        ButtonStatus Status = ButtonStatus.Up;

        GameLauncher_MousePointerSprite mousePointer;
        MouseState previousMouseState;

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

        public static User getUser() 
        {
            return user;
        }

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
