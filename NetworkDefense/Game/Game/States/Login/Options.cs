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
    /// <summary>
    /// The state for the options screen.
    /// </summary>
    class Options:TPState
    {
        /// <summary>
        /// The width of the game screen
        /// </summary>
        private int screenWidth = TPEngine.Get().ScreenSize.Width;
        /// <summary>
        /// The height of the game screen
        /// </summary>
        private int screenHeight = TPEngine.Get().ScreenSize.Height;
        /// <summary>
        /// The array of layers in the state
        /// </summary>
        private TPLayer[] layer;
        /// <summary>
        /// The number of layers in this state
        /// </summary>
        private Int16 NumLayers = 4;
        /// <summary>
        /// The mouse cursor object
        /// </summary>
        private MousePointerSprite mousePointer = new MousePointerSprite(new Vector2(32, 32));
        /// <summary>
        /// The accept button object
        /// </summary>
        private ButtonSprite acceptButton;
        /// <summary>
        /// The cancel button object
        /// </summary>
        private ButtonSprite cancelButton;

        /// <summary>
        /// Loads everything needed for this class
        /// </summary>
        protected override void Load()
        {
            base.Load();

            //Create the layer
            layer = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                layer[x] = new TPLayer(this.layers);
            }

            //Accept and Cancel button
            acceptButton = new ButtonSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectBlackBlueBurst"),
                TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectOrangeBlueBurst"),
                new Rectangle(screenWidth / 2 - 300, screenHeight * 9 / 10, 200, 40), "Accept", layer[2]);
            cancelButton = new ButtonSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectBlackBlueBurst"),
                TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectOrangeBlueBurst"),
                new Rectangle(screenWidth / 2 + 100, screenHeight * 9 / 10, 200, 40), "Cancel", layer[2]);
            acceptButton.CenterText = true;
            cancelButton.CenterText = true;
            //Set the cancel button action to pop the current state
            cancelButton.ButtonAction = delegate { TPEngine.Get().State.PopState(); };
            //Set the accept button to pop up a confirmation box
            acceptButton.ButtonAction = delegate { TPEngine.Get().State.PushState(new ConfirmationPopup("Not implemented", true, false));};

            layer[1].AddEntity(acceptButton);
            layer[1].AddEntity(cancelButton);

            layer[0].AddEntity(new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/NC_Background")));
            layer[3].AddEntity(mousePointer);
        }
    }
}
