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
using Engine.Collision;
using Microsoft.Xna.Framework.Input;

//Ting Fung (Kevin) Ng
//Wing Lim (William) Cheng

namespace Game.PipeGame
{
    /// <summary>
    /// The start screen for the pipe game. It has a title and a play button that brings the user
    /// to the game state.
    /// </summary>
    class PipeGame_StartState : TPState
    {
        /// <summary>
        /// The width of the play button
        /// </summary>
        private const int playButtonWidth = 300;
        /// <summary>
        /// The height of the play button
        /// </summary>
        private const int playButtonHeight = 50;
        /// <summary>
        /// The layer of the state
        /// </summary>
        private TPLayer[] layer;
        /// <summary>
        /// The mouse cursor
        /// </summary>
        private MousePointerSprite mousePointer = new MousePointerSprite(new Vector2(200, 200));

        /// <summary>
        /// Load everything the start state needed.
        /// </summary>
        protected override void Load()
        {
            base.Load();
            layer = new TPLayer[4];
            for (int x = 0; x < 4; x++)
            {
                layer[x] = new TPLayer(this.layers);
            }

            //Game title
            TPString title = new TPString("Connect to BCIT's Wifi");
            title.Position = new Vector2(TPEngine.Get().ScreenSize.Width / 2, TPEngine.Get().ScreenSize.Height / 2 - 100);
            title.RenderColor = Color.DarkBlue;
            title.centerText = true;
            title.Scale = new Vector2(2, 2);
            layer[1].AddEntity(title);

            //Play button
            ButtonSprite playButton = new ButtonSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectBlackBlueBurst"),
               TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectOrangeBlueBurst"),
               new Rectangle(TPEngine.Get().ScreenSize.Width / 2 - playButtonWidth / 2, TPEngine.Get().ScreenSize.Height /2 + 100 
                   , playButtonWidth, playButtonHeight), "Play", layer[2]);
            playButton.ButtonAction = delegate { TPEngine.Get().State.PopState(); };
            playButton.CenterText = true;
            playButton.DrawColor = Color.White;
            layer[1].AddEntity(playButton);

            //Background and mouse pointer
            layer[0].AddEntity(new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/pipe_background")));
            layer[3].AddEntity(mousePointer);
        }
    }
}
