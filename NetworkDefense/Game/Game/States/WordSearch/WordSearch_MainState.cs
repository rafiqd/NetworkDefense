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
    class WordSearch_MainState : TPState
    {
        /// <summary>
        /// number of layers to be added to the game
        /// </summary>
        TPLayer[] myLayers;

        /// <summary>
        /// specfies the amount of layers to create
        /// </summary>
        Int16 NumLayers = 3;

        /// <summary>
        /// Creates the backsplash of the wordsearch
        /// </summary>
        WordSearchSprite mySprite;

        /// <summary>
        /// Custom mouse sprite
        /// </summary>
        WordSearch_MouseSprite mouseSprite;

        /// <summary>
        /// Generates the wordsearch
        /// </summary>
        WordSearch_WordGenerator wordSearchGenerator;

        /// <summary>
        /// contains all the characters of the word search
        /// </summary>
        char[,] wordSearchGrid;

        /// <summary>
        /// list of the words the user must find in the puzzle
        /// </summary>
        List<string> wordsToFind;

        /// <summary>
        /// list of positions of the words in the word search grid
        /// </summary>
        List<WordSearch_WordFind> positions;

        /// <summary>
        /// initialize all the components
        /// </summary>
        protected override void Load()
        {
            base.Load();

            wordSearchGenerator = new WordSearch_WordGenerator();

            wordsToFind = wordSearchGenerator.getWordsToFind();
            positions = wordSearchGenerator.getWordPositionList();

            myLayers = new TPLayer[NumLayers];
            wordSearchGrid = wordSearchGenerator.generatePuzzle();

            for (int x = 0; x < NumLayers; x++)
            {
                myLayers[x] = new TPLayer(this.layers);
            }


            mySprite = new WordSearchSprite(mouseSprite, wordSearchGrid, wordsToFind, positions);
            myLayers[0].AddEntity(mySprite);


            mouseSprite = new WordSearch_MouseSprite(new Vector2(200, 200));
            myLayers[1].AddEntity(mouseSprite);


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
