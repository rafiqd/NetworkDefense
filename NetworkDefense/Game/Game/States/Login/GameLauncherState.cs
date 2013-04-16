using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Microsoft.Xna.Framework.Input;
using Engine;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Sprites;
using Engine.Collision;
using Engine.UI;

namespace Game.States
{
    class GameLauncherState : TPState
    {
        TPLayer[] myLayers;
        Int16 NumLayers = 3;

        TPSprite rectangleEmailSprite,
                 rectanglePasswordSprite;

        string emailString = "",
               passwordString = "";

        bool tabToggle = false;

        GameLauncherSprite launcherPageSprite;
        GameLauncher_EmailSprite emailSprite;
        GameLauncher_PasswordSprite passwordSprite;
        GameLauncher_MousePointerSprite mousePointerSprite;
        GameLauncher_LoginButtonSprite loginButtonSprite;
        GameLauncher_RegisterButtonSprite registerButtonSprite;

        KeyboardState previousKBState;

        protected override void Load()
        {
            base.Load();

            rectangleEmailSprite = new TPSprite(@"art/launcher/rectangleTexture");
            rectangleEmailSprite.Position = new Vector2(528, 483);

            rectanglePasswordSprite = new TPSprite(@"art/launcher/rectangleTexture");
            rectanglePasswordSprite.Position = new Vector2(528, 563);

            myLayers = new TPLayer[NumLayers];

            for (int x = 0; x < NumLayers; x++)
            {
                myLayers[x] = new TPLayer(this.layers);
            }

            launcherPageSprite = new GameLauncherSprite(new Vector2(450, 250));
            myLayers[0].AddEntity(launcherPageSprite);

            mousePointerSprite = new GameLauncher_MousePointerSprite(new Vector2(200, 200));
            myLayers[2].AddEntity(mousePointerSprite);

            emailSprite = new GameLauncher_EmailSprite(mousePointerSprite, rectanglePasswordSprite, emailString);
            emailSprite.selectMe();
            myLayers[1].AddEntity(emailSprite);

            passwordSprite = new GameLauncher_PasswordSprite(mousePointerSprite, rectangleEmailSprite, passwordString);
            myLayers[1].AddEntity(passwordSprite);

            loginButtonSprite = new GameLauncher_LoginButtonSprite(emailSprite, passwordSprite, mousePointerSprite, "Login");
            myLayers[1].AddEntity(loginButtonSprite);

            registerButtonSprite = new GameLauncher_RegisterButtonSprite(mousePointerSprite, "Register");
            myLayers[1].AddEntity(registerButtonSprite);

            previousKBState = Keyboard.GetState();

        }

        public override void Update(GameTime gameTime)
        {
            if (tabToggle == false)
            {
                if (Keyboard.GetState().IsKeyUp(Keys.Tab) && previousKBState.IsKeyDown(Keys.Tab))
                {
                    tabToggle = true;
                    emailSprite.unselectMe();
                    passwordSprite.selectMe();
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyUp(Keys.Tab) && previousKBState.IsKeyDown(Keys.Tab))
                {
                    tabToggle = false;
                    emailSprite.selectMe();
                    passwordSprite.unselectMe();
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && previousKBState.IsKeyDown(Keys.Enter))
            {
                loginButtonSprite.login();
            }

            previousKBState = Keyboard.GetState();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.GraphicsDevice.Clear(Color.CadetBlue);
            base.Draw(batch);
        }
    }
}
