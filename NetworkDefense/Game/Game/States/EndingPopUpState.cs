using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Engine.Objects;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Microsoft.Xna.Framework;
using Game.Sprites;
using Microsoft.Xna.Framework.Input;

namespace Game.States
{
    /// <summary>
    /// A state contains a confirmation message, an ok button, and an optional cancel button.
    /// It is made by confirmationPopup.
    /// It is modified little form confirmationPopup.
    /// It is for displaying final score and job.
    /// </summary>
    class EndingPopUpState : TPState
    {
        /// <summary>
        /// The x position of the window
        /// </summary>
        private const int windowPosX = 340;
        /// <summary>
        /// The y posistion of the window
        /// </summary>
        private const int windowPosY = 210;
        /// <summary>
        /// The width of the window
        /// </summary>
        private const int windowSizeX = 600;
        /// <summary>
        /// The height of the window
        /// </summary>
        private const int windowSizeY = 300;
        /// <summary>
        /// The mouse cursor
        /// </summary>
        private MousePointerSprite mousePointer = new MousePointerSprite(new Vector2(32, 32));
        /// <summary>
        /// Layer : 2
        /// </summary>
        TPLayer[] layer;
        Int16 NumLayers = 2;
        /// <summary>
        /// Texture for result
        /// </summary>
        TPSprite resultTexture;
        /// <summary>
        /// TPString which implement from EndingState
        /// </summary>
        private TPString message;

        /// <summary>
        /// Constructor which get String from EndingState
        /// </summary>
        /// <param name="msg"></param>
        public EndingPopUpState(string msg)
        {
            message = new TPString(msg);
        }
        /// <summary>
        /// Load everything that a confirmation-popup-state needs.
        /// </summary>
        protected override void Load()
        {
            base.Load();
            resultTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/endingResult"));
            //Create layers
            layer = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                layer[x] = new TPLayer(this.layers);
            }
            layer[0].AddEntity(resultTexture);
            message.centerText = true;
            message.RenderColor = Color.Black;
            message.Position.X = windowPosX + windowSizeX / 2;
            message.Position.Y = windowPosY + windowSizeY / 4;
            layer[0].AddEntity(message);
            layer[NumLayers - 1].AddEntity(mousePointer);
        }
        /// <summary>
        /// Update HandleInput
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            HandleInput(gameTime);
        }
        /// <summary>
        /// Check finish button click
        /// </summary>
        /// <param name="gameTime"></param>
        private void HandleInput(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed
                && mousePointer.Position.X > (TPEngine.Get().ScreenSize.Width / 2) - 150
                && mousePointer.Position.X < (TPEngine.Get().ScreenSize.Width / 2) + 200
                && mousePointer.Position.Y > (TPEngine.Get().ScreenSize.Height) - 120
                && mousePointer.Position.Y < (TPEngine.Get().ScreenSize.Height) - 20)
            {
                TPEngine.Get().State.PushState(new MainMenu(), true);
            }
        }
    }
}
