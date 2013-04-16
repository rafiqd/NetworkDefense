using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Engine.StateManagement;
using Microsoft.Xna.Framework.Input;
using Engine;
using Engine.Collision;
using System.Diagnostics;
using System.Threading;

namespace Game.Sprites
{
    /// <summary>
    /// class that contains all the methods and logic for drawing out a register button and accessing the website
    /// </summary>
    class GameLauncher_RegisterButtonSprite : TPSprite
    {
        /// <summary>
        /// used to display the text on the button
        /// </summary>
        TPString buttonText;

        /// <summary>
        /// used to store the image of the button
        /// </summary>
        TPSprite registerButtonSprite;

        /// <summary>
        /// the default position is up on the button
        /// </summary>
        ButtonStatus Status = ButtonStatus.Up;

        /// <summary>
        /// the mouse pointer, used for collision logic
        /// </summary>
        GameLauncher_MousePointerSprite mousePointer;

        /// <summary>
        /// previous mouse state
        /// </summary>
        MouseState previousMouseState;

        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="mouse">the mouse pointer object</param>
        /// <param name="text">the text you want the button to display</param>
        public GameLauncher_RegisterButtonSprite(GameLauncher_MousePointerSprite mouse, string text)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/launcher/borderPixel"))
        {
            mousePointer = mouse;

            registerButtonSprite = new TPSprite(@"art/launcher/button-Up");

            buttonText = new TPString(text);

            buttonText.Scale = new Vector2(0.8f, 0.8f);
            buttonText.Position = new Vector2(667, 632);
            buttonText.RenderColor = Color.Black;


            previousMouseState = Mouse.GetState();

        }

        /// <summary>
        /// method to keep updating the game, contains logic for collision
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Recognize a single click or button is held of the left mouse button
            if ((Mouse.GetState().LeftButton == ButtonState.Released || previousMouseState.LeftButton == ButtonState.Pressed) && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                if (TPCollider.Test(mousePointer, registerButtonSprite))
                {
                    Status = ButtonStatus.Down;
                    Process proc = Process.Start("http://142.232.17.225/User");
                }
            }
            if (previousMouseState.LeftButton == ButtonState.Released && TPCollider.Test(mousePointer, registerButtonSprite))
            {
                Status = ButtonStatus.Up;
            }
            if (previousMouseState.LeftButton == ButtonState.Released && !TPCollider.Test(mousePointer, registerButtonSprite))
            {
                Status = ButtonStatus.Up;
            }

            previousMouseState = Mouse.GetState();

        }

        /// <summary>
        /// draws out all the sprites
        /// </summary>
        /// <param name="batch">used to draw</param>
        public override void Draw(SpriteBatch batch)
        {
            if (Status == ButtonStatus.Down)
            {
                registerButtonSprite = new TPSprite(@"art/launcher/button-Down");
                registerButtonSprite.Position = new Vector2(640, 625);
                registerButtonSprite.Draw(batch);
            }
            else
            {
                registerButtonSprite = new TPSprite(@"art/launcher/button-Up");
                registerButtonSprite.Position = new Vector2(640, 625);
                registerButtonSprite.Draw(batch);
            }

            buttonText.Draw(batch);
        }
    }
}
