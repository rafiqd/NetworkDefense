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
using Microsoft.Xna.Framework.Input;
namespace Game
{
    class IntroState : TPState
    {
        /// <summary>
        /// variable for layers
        /// </summary>
        TPLayer[] myLayers;
        Int16 NumLayers = 2;

        /// <summary>
        /// variable for texture
        /// </summary>
        TPSprite backgroundTexture;

        /// <summary>
        /// boolean variable to check mouse release
        /// </summary>
        bool isReady = false;

        protected override void Load()
        {
            base.Load();
            myLayers = new TPLayer[NumLayers];

            for (int x = 0; x < NumLayers; x++)
            {
                myLayers[x] = new TPLayer(this.layers);
            }
            backgroundTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/introduction"));
            backgroundTexture.Position.X = 0;
            backgroundTexture.Position.Y = 0;
            myLayers[0].AddEntity(backgroundTexture);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //check mouse release and press, and change state to WashRoomState
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                isReady = true; 
            }
            if (isReady && Mouse.GetState().LeftButton == ButtonState.Released)
            {
                TPEngine.Get().State.PopState();
                TPEngine.Get().State.PushState(new WashRoomState(), false);
            }
        }
    }
}
