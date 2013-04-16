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

//Ting Fung (Kevin) Ng
//Wing Lim (William) Cheng

namespace Game.States
{
    /// <summary>
    /// A state contains a confirmation message, an ok button, and an optional cancel button.
    /// </summary>
    class ConfirmationPopup : TPState
    {
        /// <summary>
        /// The array of layer of the state
        /// </summary>
        private TPLayer[] layer;
        /// <summary>
        /// The number of layers of the state
        /// </summary>
        private Int16 NumLayers = 4;
        /// <summary>
        /// The mouse cursor
        /// </summary>
        private MousePointerSprite mousePointer = new MousePointerSprite(new Vector2(32, 32));
        /// <summary>
        /// The message that is displayed on the state
        /// </summary>
        private TPString message;
        /// <summary>
        /// The texture of the background of the state
        /// </summary>
        private Texture2D backgroundTexture;
        /// <summary>
        /// Boolean indicates whether the state contains no cancel button
        /// </summary>
        private bool OKbuttonOnly = false;
        /// <summary>
        /// Boolean indicates whether the state pops two states(including the game state bebind this state
        /// when it quits
        /// </summary>
        private bool quitGame = false;
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
        /// The border thickness of the window
        /// </summary>
        private const int borderSize = 10;
        /// <summary>
        /// The width of the buttons
        /// </summary>
        private const int buttonWidth = 125;
        /// <summary>
        /// The height of the buttons
        /// </summary>
        private const int buttonHeight = 40;
        /// <summary>
        /// The y position of the buttons
        /// </summary>
        private const int buttonPosY = 250;
        /// <summary>
        /// The distance between the two buttons
        /// </summary>
        private const int buttonOffset = 100;
        /// <summary>
        /// The texture of the buttons
        /// </summary>
        private Texture2D buttonTexture;
        /// <summary>
        /// The colour of the buttons
        /// </summary>
        private Color buttonColour = Color.Blue;
        /// <summary>
        /// The confirmation result indicates which button the user has clicked. It returns the result.
        /// </summary>
        private ConfirmationResult result;

        /// <summary>
        /// Contructor that takes in a string only.
        /// </summary>
        /// <param name="msg">The string that is displayed on the state</param>
        public ConfirmationPopup(string msg)
        {
            message = new TPString(msg);
            backgroundTexture = TPEngine.Get().TextureManager.CreateFilledRectangle(1, 1, Color.White);
            buttonTexture = TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectBlackBlueBurst");
        }
    
        /// <summary>
        /// Constructor that takes in string and a result object.
        /// </summary>
        /// <param name="msg">The string that is displayed on the state</param>
        /// <param name="result">The result object of the confirmation</param>
        public ConfirmationPopup(string msg, ConfirmationResult result)
            : this(msg)
        {
            this.result = result;
        }

        /// <summary>
        /// Constructor for setting whether it contains only an ok button and it is a confirmation to quit a game.
        /// </summary>
        /// <param name="msg">The text appears on this confirmation box</param>
        /// <param name="okbuttononly">The Boolean for setting no cancel button appearring on this confirmatiob box</param>
        /// <param name="quitGame">The boolean for quiting a gamestate</param>
        public ConfirmationPopup(string msg, bool okbuttononly, bool quitGame)
            : this(msg)
        {
            OKbuttonOnly = okbuttononly;
            this.quitGame = quitGame;
        }

        /// <summary>
        /// Load everything that a confirmation-popup-state needs.
        /// </summary>
        protected override void Load()
        {
            base.Load();

            //Create layers
            layer = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                layer[x] = new TPLayer(this.layers);
            }
            layer[NumLayers - 1].AddEntity(mousePointer);

            //Set the text
            message.centerText = true;
            message.RenderColor = Color.Black;
            message.Position.X = windowPosX + windowSizeX / 2;
            message.Position.Y = windowPosY + windowSizeY / 4;
            layer[0].AddEntity(message);

            //Create buttons
            ButtonSprite OKButton = new ButtonSprite(buttonTexture,
                TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectOrangeBlueBurst"), 
                new Rectangle(windowPosX + buttonOffset, windowPosY + buttonPosY, buttonWidth, buttonHeight), "OK", layer[2]);
            OKButton.CenterText = true;
            OKButton.ButtonAction = delegate { if (result != null) result.result = true; TPEngine.Get().State.PopState(); if(quitGame) TPEngine.Get().State.PopState(); };
            layer[1].AddEntity(OKButton);

            if (!OKbuttonOnly)
            {
                ButtonSprite CancelButton = new ButtonSprite(buttonTexture,
                    TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectOrangeBlueBurst"),
                    new Rectangle(windowPosX + windowSizeX - buttonOffset - buttonWidth, windowPosY + buttonPosY, buttonWidth, buttonHeight), "Cancel", layer[2]);
                CancelButton.CenterText = true;
                CancelButton.ButtonAction = delegate { if (result != null) result.result = false; TPEngine.Get().State.PopState(); };
                layer[1].AddEntity(CancelButton);
            }
            else
                OKButton.Position = new Vector2(640 - buttonWidth / 2, windowPosY + buttonPosY);
        }

        /// <summary>
        /// Draw the background textures of the state
        /// </summary>
        /// <param name="batch">Sprite batch</param>
        public override void Draw(SpriteBatch batch)
        {
            
            batch.Begin();
            batch.Draw(backgroundTexture, new Rectangle(0, 0, 1280, 720), Color.Black * 0.75f);
            batch.Draw(backgroundTexture, new Rectangle(windowPosX - borderSize, windowPosY - borderSize, windowSizeX + borderSize * 2, windowSizeY + borderSize * 2), Color.Azure);
            batch.Draw(backgroundTexture, new Rectangle(windowPosX, windowPosY, windowSizeX, windowSizeY), Color.CadetBlue);       
            batch.End();
            base.Draw(batch);
        }
    }

    /// <summary>
    /// Holding a boolean result of comfirmation.
    /// </summary>
    public class ConfirmationResult
    {
        public bool result = false;
    }
}
