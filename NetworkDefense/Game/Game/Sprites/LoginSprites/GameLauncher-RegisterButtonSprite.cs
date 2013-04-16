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
    class GameLauncher_RegisterButtonSprite: TPSprite
    {

        TPString buttonText;
        TPSprite registerButtonSprite;

        ButtonStatus Status = ButtonStatus.Up;

        GameLauncher_MousePointerSprite mousePointer;
        MouseState previousMouseState;

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
