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
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Input;

namespace Game.States
{
    class WordSearch_EndGameState : TPState
    {
        /// <summary>
        /// layer to be added to the game
        /// </summary>
        TPLayer myLayer;

        /// <summary>
        /// Creates the backsplash of the wordsearch
        /// </summary>
        WordSearch_EndGameSprite endGameSprite;

        int playerScore = 0;
        string gameState = "";

        public WordSearch_EndGameState(string state, int score)
        {
            gameState = state;
            playerScore = score;
        }

        /// <summary>
        /// initialize all the components
        /// </summary>
        protected override void Load()
        {
            base.Load();

            myLayer = new TPLayer(this.layers);

            endGameSprite = new WordSearch_EndGameSprite(gameState, playerScore);
            myLayer.AddEntity(endGameSprite);
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="batch">used to draw out the sprite</param>
        public override void Draw(SpriteBatch batch)
        {
            batch.GraphicsDevice.Clear(Color.White);
            base.Draw(batch);
        }

    }
}
