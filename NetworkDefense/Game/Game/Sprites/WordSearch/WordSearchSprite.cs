using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Engine;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Engine.Collision;
using Game.States;

namespace Game.Sprites
{
    /// <summary>
    /// Struct Line that holds the start point of the line and end point of the line
    /// </summary>
    struct Line
    {
        public Vector2 startP;
        public Vector2 endP;
    }

    /// <summary>
    /// The player sprite.
    /// </summary>
    class WordSearchSprite : TPSprite
    {
        /// <summary>
        /// font
        /// </summary>
        SpriteFont font;

        /// <summary>
        /// Texture for a line
        /// </summary>
        Texture2D lineTexture;


        /// <summary>
        /// Sprite for the current character, Texture for a line
        /// </summary>
        TPSprite characterSprite,
                 lineSprite;

        /// <summary>
        /// The engine string that will be displayed on the screen
        /// </summary>
        TPString label,
                 scoreLabel,
                 timeLabel;

        /// <summary>
        /// a character grid
        /// </summary>
        char[,] grid;

        /// <summary>
        /// List of strings that will contain the words needed to be found
        /// </summary>
        List<string> wordsToFind;

        /// <summary>
        /// List that holds all the lines to be drawn
        /// </summary>
        List<Line> lineList;

        /// <summary>
        /// dictionary to hold all the images of the corresponding characters of the alphabet
        /// </summary>
        Dictionary<char, TPSprite> wordsearchSpriteGridDict;


        /// <summary>
        /// previous mouse state
        /// </summary>
        MouseState previousMouseState;

        /// <summary>
        /// stores the x and y of the first point
        /// </summary>
        Vector2 startPoint;

        /// <summary>
        /// flags to denote if we got the first point, have found the first word---used to start timer after we found first word
        /// and also to let startTime be set once
        /// </summary>
        bool firstpoint = true,
             firstWordFound = false;


        /// <summary>
        /// Line batch, contains methods to draw lines
        /// </summary>
        WordSearch_LineBatch lineBatch;

        /// <summary>
        /// list of all the word positions in the word search grid
        /// </summary>
        List<WordSearch_WordFind> wordGridPositionList;

        /// <summary>
        /// list of all the character sprites in the word search grid
        /// </summary>
        List<TPSprite> charactersInWordSearch;

        /// <summary>
        /// Mouse pointer
        /// </summary>
        WordSearch_MouseSprite mouse;

        /// <summary>
        /// the player score and the counter to keep trrack of how many words were found
        /// </summary>
        int playerScore = 0,
            playerScoreFinal = 0,
            wordFoundCounter;

        /// <summary>
        /// Class that used to create a countdown
        /// </summary>
        WordSearch_CountDownTimer timer;

        /// <summary>
        /// The constructor sets the word search grid and sets the list of words to find.
        /// </summary>
        /// <param name="completedGrid">The completed word search grid</param>
        /// /// <param name="wordList">List of words to find</param>
        /// /// <param name="listOfPos">List of the words to find positions in the grid</param>
        public WordSearchSprite(WordSearch_MouseSprite pointer, char[,] completedGrid, List<string> wordList, List<WordSearch_WordFind> listOfPos)
            : base(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/1x1WhitePixel"))
        {
            // intialize the character grid
            grid = completedGrid;

            // intialize the list of positions for the words in the word search
            wordGridPositionList = listOfPos;

            //intialize my countdown timer
            timer = new WordSearch_CountDownTimer();

            // set the mouse pointer
            mouse = pointer;

            // Initialize the List of lines
            lineList = new List<Line>();

            // Initialize the LineBatch
            lineBatch = new WordSearch_LineBatch();

            // Initialize the Characters in Word Search
            charactersInWordSearch = new List<TPSprite>();

            // Initialize the label and set it up
            label = new TPString("Words to Find:");
            label.Position = new Vector2(600, 475);
            label.Scale = new Vector2(0.7f, 0.7f);
            label.RenderColor = Color.Black;

            // Initialize the label for scoring
            scoreLabel = new TPString("Score: " + playerScore);
            scoreLabel.Position = new Vector2(1150, 50);
            scoreLabel.Scale = new Vector2(0.6f, 0.6f);
            scoreLabel.RenderColor = Color.Black;


            // Initialize the label for scoring
            timeLabel = new TPString("Time: " + "1:00");
            timeLabel.Position = new Vector2(1150, 30);
            timeLabel.Scale = new Vector2(0.6f, 0.6f);
            timeLabel.RenderColor = Color.Black;

            // Initialize the list
            wordsToFind = wordList;

            // initialize the sprite
            lineSprite = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/1x1WhitePixel"));

            // grab the texure from the art folder
            lineTexture = TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/1x1WhitePixel");

            // load in the test font
            font = TPEngine.Get().FontManager.LoadFont(@"fonts\testfont");

            // initialize the previous mouse state
            previousMouseState = Mouse.GetState();

            // Initialize the character and sprite dictionaries
            initializeDictionarys();
        }

        /// <summary>
        /// Initializes the Dictionary of character with there proper textures
        /// </summary>
        public void initializeDictionarys()
        {
            wordsearchSpriteGridDict = new Dictionary<char, TPSprite>();

            wordsearchSpriteGridDict.Add('a', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/a")));
            wordsearchSpriteGridDict.Add('b', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/b")));
            wordsearchSpriteGridDict.Add('c', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/c")));
            wordsearchSpriteGridDict.Add('d', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/d")));
            wordsearchSpriteGridDict.Add('e', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/e")));
            wordsearchSpriteGridDict.Add('f', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/f")));
            wordsearchSpriteGridDict.Add('g', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/g")));
            wordsearchSpriteGridDict.Add('h', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/h")));
            wordsearchSpriteGridDict.Add('i', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/i")));
            wordsearchSpriteGridDict.Add('j', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/j")));
            wordsearchSpriteGridDict.Add('k', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/k")));
            wordsearchSpriteGridDict.Add('l', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/l")));
            wordsearchSpriteGridDict.Add('m', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/m")));
            wordsearchSpriteGridDict.Add('n', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/n")));
            wordsearchSpriteGridDict.Add('o', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/o")));
            wordsearchSpriteGridDict.Add('p', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/p")));
            wordsearchSpriteGridDict.Add('q', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/q")));
            wordsearchSpriteGridDict.Add('r', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/r")));
            wordsearchSpriteGridDict.Add('s', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/s")));
            wordsearchSpriteGridDict.Add('t', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/t")));
            wordsearchSpriteGridDict.Add('u', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/u")));
            wordsearchSpriteGridDict.Add('v', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/v")));
            wordsearchSpriteGridDict.Add('w', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/w")));
            wordsearchSpriteGridDict.Add('x', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/x")));
            wordsearchSpriteGridDict.Add('y', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/y")));
            wordsearchSpriteGridDict.Add('z', new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/WordSearch/z")));
        }

        /// <summary>
        /// Updates the game constantly
        /// </summary>
        /// <param name="gameTime">Game time used for scoring</param>
        public override void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();

            string word = " ";

            bool wordMatched = false;

            //checks to see if a mouse click has happened
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                if (firstpoint) // if first point save it
                {
                    //grab the start point
                    startPoint = new Vector2(currentMouseState.X, currentMouseState.Y);
                    firstpoint = false; //this is not the first click anymore of a new line
                }
                else // add it to the list 
                {
                    Line lines = new Line(); // create a new structure line
                    lines.startP = startPoint; //store the start point
                    lines.endP = new Vector2(currentMouseState.X, currentMouseState.Y); // store the end point

                    for (int i = 0; i < wordGridPositionList.Count; i++)
                    {
                        //checks to see if the line points match the words starting and end positions in the grid
                        if (wordGridPositionList[i].checkWord(new Point((int)lines.startP.X, (int)lines.startP.Y), new Point((int)lines.endP.X, (int)lines.endP.Y)))
                        {
                            //grabs the word from the according to the position of the start and end point in the grid
                            word = wordGridPositionList[i].getWord();

                            //checks to see if the word exists in the list
                            if (wordsToFind.Contains(word))
                            {
                                wordsToFind[wordsToFind.IndexOf(word)] = ""; // word has been found, make it a blank string
                                firstWordFound = true;
                                playerScore += 10; //increment the score by 10
                                wordFoundCounter++;

                                if (timer.isActive)
                                {
                                    timer.incrementTimer(10);
                                }

                                scoreLabel.Clear(); // clear the string
                                scoreLabel.Append("Score: " + playerScore.ToString()); // update the new score
                            }
                            //the word has been matched and found
                            wordMatched = true;
                        }
                    }

                    if (wordMatched)
                    {
                        lineList.Add(lines);
                    }

                    firstpoint = true;
                    startPoint = Vector2.Zero;
                }
            }

            previousMouseState = currentMouseState;


            if (timer.isActive == false)
            {
                timer.set(gameTime, 59);
            }

            if (timer.checkTimer(gameTime) || Keyboard.GetState().IsKeyDown(Keys.X))
            {
                if (wordFoundCounter >= 3)
                {
                    playerScoreFinal = 20 + playerScore;
                }
                else
                {
                    playerScoreFinal = playerScore;
                }

                if (playerScoreFinal >= 100)
                {
                    playerScoreFinal = 100;
                }

                TPEngine.Get().State.PushState(new WordSearch_EndGameState("Game Over", playerScoreFinal));
            }

            timeLabel.Clear();
            timeLabel.Append("Time: " + (timer.displayValue / 60) + ":" + (timer.displayValue % 60).ToString().PadLeft(2, '0'));



            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the sprite.
        /// </summary>
        /// <param name="spriteBatch">used to draw out the sprite.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {

            const int ROW = 14;
            const int COL = 25;

            if (!firstpoint)
            {
                lineBatch.DrawLine(spriteBatch, Color.Red, startPoint, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            }

            // Draw all lines in list
            for (int i = 0; i < lineList.Count; i++)
            {
                lineBatch.DrawLine(spriteBatch, Color.Red, lineList[i].startP, lineList[i].endP);
            }


            // Draws the word search grid
            for (int r = 0; r < COL; r++)
            {
                for (int c = 0; c < ROW; c++)
                {
                    if (wordsearchSpriteGridDict.ContainsKey(grid[r, c]))
                    {
                        characterSprite = wordsearchSpriteGridDict[grid[r, c]];
                        characterSprite.Position = new Vector2((r * 30) + 260, (c * 30) + 25);
                        characterSprite.Scale = new Vector2(0.8f, 0.8f);
                        charactersInWordSearch.Add(characterSprite);
                        characterSprite.Draw(spriteBatch);
                    }
                }
            }

            // Draws horizontal line
            spriteBatch.Draw(lineTexture, new Rectangle(0, 455, 1500, 2), Color.Black);

            // Draws left vertical line
            spriteBatch.Draw(lineTexture, new Rectangle(175, 0, 2, 1500), Color.Black);

            // Draws right vertical line
            spriteBatch.Draw(lineTexture, new Rectangle(1075, 0, 2, 1500), Color.Black);

            // Draws the label
            label.Draw(spriteBatch);

            // Draws the score
            scoreLabel.Draw(spriteBatch);

            //Draws the time elasped
            timeLabel.Draw(spriteBatch);

            // Draws the words to find list in 8 seperate columns of 5 rows each
            for (int i = 0; i < 5; i++)
            {
                spriteBatch.DrawString(font, wordsToFind[i], new Vector2(200, (i * 30) + 525), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            }

            for (int j = 5; j < 10; j++)
            {
                spriteBatch.DrawString(font, wordsToFind[j], new Vector2(320, (j * 30) + 375), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            }

            for (int k = 10; k < 15; k++)
            {
                spriteBatch.DrawString(font, wordsToFind[k], new Vector2(430, (k * 30) + 225), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            }

            for (int n = 15; n < 20; n++)
            {
                spriteBatch.DrawString(font, wordsToFind[n], new Vector2(540, (n * 30) + 75), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            }

            for (int m = 20; m < 25; m++)
            {
                spriteBatch.DrawString(font, wordsToFind[m], new Vector2(650, (m * 30) - 75), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            }

            for (int a = 25; a < 30; a++)
            {
                spriteBatch.DrawString(font, wordsToFind[a], new Vector2(760, (a * 30) - 225), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            }

            for (int b = 30; b < 35; b++)
            {
                spriteBatch.DrawString(font, wordsToFind[b], new Vector2(870, (b * 30) - 375), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            }

            for (int c = 35; c < 40; c++)
            {
                spriteBatch.DrawString(font, wordsToFind[c], new Vector2(990, (c * 30) - 525), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            }

        }
    }
}
