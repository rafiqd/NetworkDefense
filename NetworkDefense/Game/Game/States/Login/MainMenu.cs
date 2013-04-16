using System.Text;
using Engine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Engine.Objects;
using System;
using Game.Sprites;

//Ting Fung (Kevin) Ng
//Wing Lim (William) Cheng

namespace Game.States
{
    /// <summary>
    /// Main menu of the game.
    /// Three navigations: Start game, Options, Exit
    /// </summary>
    class MainMenu : TPState
    {
        /// <summary>
        /// The width of the game window.
        /// </summary>
        private int screenWidth = TPEngine.Get().ScreenSize.Width;
        /// <summary>
        /// The height of the game window.
        /// </summary>
        private int screenHeight = TPEngine.Get().ScreenSize.Height;
        /// <summary>
        /// The layer that holds all the drawn objects.
        /// </summary>
        private TPLayer[] layer;
        /// <summary>
        /// The number of layers in this state
        /// </summary>
        private Int16 NumLayers = 5;
        /// <summary>
        /// The mouse pointer object
        /// </summary>
        private MousePointerSprite mousePointer = new MousePointerSprite(new Vector2(32, 32));
        /// <summary>
        /// The array of buttons in this state
        /// </summary>
        private ButtonSprite[] buttonArray;
        /// <summary>
        /// The number of buttons in this state
        /// </summary>
        private int buttonNum = 3;
        /// <summary>
        /// The array of strings on the buttons
        /// </summary>
        private string[] menuText = { "Start Game", "Options", "Exit" };
        /// <summary>
        /// The x position of the buttons
        /// </summary>
        private int buttonX = 50;
        /// <summary>
        /// The y position of the buttons
        /// </summary>
        private int buttonY = 50;
        /// <summary>
        /// The logo sprite
        /// </summary>
        private TPSprite logo;

        /// <summary>
        /// Load and create everything this state needs
        /// </summary>
        protected override void Load()
        {
            base.Load();

            logo = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/Logo"));
            logo.Position.X = screenWidth * 2 /3;
            logo.Position.Y = screenHeight * 3 / 5;
            logo.Scale = new Vector2(2, 2);

            
            layer = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                layer[x] = new TPLayer(this.layers);
            }

            buttonArray = new ButtonSprite[buttonNum];
            for (int i = 0; i < buttonNum; i++)
            {
                buttonArray[i] = new ButtonSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectBlackBlueBurst"),
                    TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectOrangeBlueBurst"),
                    new Rectangle(buttonX, buttonY + i*100, 300, 75), menuText[i], layer[3]);
                layer[2].AddEntity(buttonArray[i]);
            }
            //Set the menu button actions to push the new states
            buttonArray[0].ButtonAction = delegate { TPEngine.Get().State.PushState(new LoadCharacter(), true); };
            buttonArray[1].ButtonAction = delegate { TPEngine.Get().State.PushState(new Options(), true); };
            //Set the exit button action to close the game
            buttonArray[2].ButtonAction = delegate { TPEngine.Get().GameRef.Exit(); };

            layer[0].AddEntity(new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/MM_Background")));
            layer[1].AddEntity(logo);
            layer[4].AddEntity(mousePointer);
        }
    }
}