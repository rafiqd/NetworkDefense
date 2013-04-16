using System.Text;
using Engine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Engine.Objects;
using System;
using Game.Sprites;
using System.Threading;

//Ting Fung (Kevin) Ng
//Wing Lim (William) Cheng

namespace Game.States
{
    class Loading : TPState
    {
        /// <summary>
        /// The layer that holds the drawn objects
        /// </summary>
        private TPLayer[] layer;
        /// <summary>
        /// The number of layers in this state
        /// </summary>
        private Int16 NumLayers = 2;
        /// <summary>
        /// The mouse pointer object
        /// </summary>
        private MousePointerSprite mousePointer = new MousePointerSprite(new Vector2(200, 200));
        /// <summary>
        /// The texture of the loading screen background
        /// </summary>
        private TPSprite loadingTexture;
        /// <summary>
        /// The string for the loading screen.
        /// </summary>
        private TPString loadingText;

        /// <summary>
        /// Loads everyting needed for this class
        /// </summary>
        protected override void Load()
        {
            base.Load();

            layer = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                layer[x] = new TPLayer(this.layers);
            }

            //Loading
            loadingTexture = new TPSprite(TPEngine.Get().TextureManager.CreateFilledRectangle(1280, 720, Color.Black));
            loadingTexture.Position = new Vector2(0, 0);
            loadingText = new TPString("Now Loading...");
            loadingText.centerText = true;
            loadingText.Position = new Vector2(640, 360);

            layer[0].AddEntity(loadingTexture);
            layer[1].AddEntity(loadingText);

        }
    }
}
