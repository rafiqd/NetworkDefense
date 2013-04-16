using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Engine.Objects;
using Game.Sprites;
using System.Runtime.InteropServices;

namespace Game
{
    /// <summary>
    /// Cyril
    /// Menu state for bird hunt game. Displays the menu screen that gives the user the option to choose to play the game or to skip.
    /// </summary>
    class BirdMenuState : TPState
    {
        /// <summary>
        /// an array type TPLayer that keeps track how many layers will be displayed on the screen
        /// </summary>
        TPLayer[] myLayers;

        /// <summary>
        /// number of layers to be displayed
        /// </summary>
        Int16 NumLayers = 3;

        /// <summary>
        /// crosshair sprite indicate where the user's aiming
        /// </summary>
        Bullseye bullseye;

        /// <summary>
        /// background image
        /// </summary>
        TPSprite backgroundTexture;

        /// <summary>
        /// start button sprite
        /// </summary>
        StartButton btn;

        /// <summary>
        /// skip button sprite
        /// </summary>
        SkipButton btn2;

        /// <summary>
        /// bullet hole sprite, indicate where the player shoots
        /// </summary>
        BulletHole bulletHole = new BulletHole();

        /// <summary>
        /// a flag indicates if the state is loaded for the first time during gameplay
        /// </summary>
        static bool FirstRunBirdMenu = true;
        
        /// <summary>
        /// load the necessary items that needed to display on screen
        /// </summary>
        protected override void Load()
        {
            base.Load();
            myLayers = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                myLayers[x] = new TPLayer(this.layers);
            }

            bullseye = new Bullseye(new Vector2(200, 200));
            myLayers[2].AddEntity(bullseye);

            backgroundTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/startmenu"));
            backgroundTexture.Position = new Vector2(backgroundTexture.Position.X, backgroundTexture.Position.Y);
            myLayers[0].AddEntity(backgroundTexture);
            btn = new StartButton();
            btn2 = new SkipButton();

            myLayers[1].AddEntity(btn);
            myLayers[1].AddEntity(btn2);
            myLayers[1].AddEntity(bulletHole);

            if (FirstRunBirdMenu)
            {
                TPEngine.Get().SpriteDictionary.Add("bulletHoleMenu", bulletHole);
                TPEngine.Get().SpriteDictionary.Add("MinigameBullseyeMenu", bullseye);
                FirstRunBirdMenu = false;
            }
        }

        /// <summary>
        /// Updates the state with each iteration.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
