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
using Microsoft.Xna.Framework.Input;

namespace Game
{
    /// <summary>
    /// Cyril
    /// congratulation state used for displaying the congratulation message when the player completes a request
    /// </summary>
    class CongratzState : TPState
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
        /// background sprite
        /// </summary>
        TPSprite backgroundTexture;
        //int startTime;

        /// <summary>
        /// indicates if the state is loaded for the first time
        /// </summary>
        static bool FirstRunCongrat = true;

        /// <summary>
        /// initialize the star sprites
        /// </summary>
        CongratStar star = new CongratStar();

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
            
            backgroundTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/Quest/congratz"));
            backgroundTexture.Position = new Vector2(backgroundTexture.Position.X, backgroundTexture.Position.Y);
            myLayers[0].AddEntity(backgroundTexture);
            myLayers[1].AddEntity(star);

            if (FirstRunCongrat)
            {
                TPEngine.Get().SpriteDictionary.Add("starCongrat", star);
                FirstRunCongrat = false;
            }
        }

        /// <summary>
        /// Update the sprites with each iteration. When the time's up. add new score into the database
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (FirstRunCongrat)
            {
                FirstRunCongrat = false;
                //startTime = gameTime.TotalGameTime.Seconds;
            }
            star = (CongratStar)TPEngine.Get().SpriteDictionary["starCongrat"];
            star.SetStartPos(new Vector2(500, 500));
            if (gameTime.TotalGameTime.Seconds > 5)
            {
                TPEngine.Get().State.PopState();
            }
        }
    }
}
