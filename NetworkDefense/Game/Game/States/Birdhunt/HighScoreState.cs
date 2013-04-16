using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Engine.Objects;
using Game.Sprites;
using System.Runtime.InteropServices;
using DBCommService;

//using Game.DBComm;

namespace Game
{
    /// <summary>
    /// Highscore state. Responsible to display the list of the top 10 score in the list for a particular mini-game
    /// </summary>
    class HighScoreState : TPState
    {
        /// <summary>
        /// an array type TPLayer that keeps track how many layers will be displayed on the screen
        /// </summary>
        TPLayer[] myLayers;

        /// <summary>
        /// number of layers to be displayed
        /// </summary>
        Int16 NumLayers = 3;

        /// <summary>
        /// crosshair sprite indicate where the user's aiming
        /// </summary>
        Bullseye bullseye;

        /// <summary>
        /// background image
        /// </summary>
        TPSprite backgroundTexture;

        /// <summary>
        /// List of top 10 scores of the game
        /// </summary>
        List<MinigameScore> scores = new List<MinigameScore>();

        /// <summary>
        /// list of strings to be drawn on screen represent the top 10 list
        /// </summary>
        List<TPString> Items = new List<TPString>();

        /// <summary>
        /// highest scores counter. This variable keeps the list consistent to 10 items
        /// </summary>
        int highscoreCount;

        /// <summary>
        /// a note to the user of how to exit the game
        /// </summary>
        TPString note = new TPString("Press Esc to exit mini-game");

        /// <summary>
        /// bullet hole sprite, indicate where the player shoots
        /// </summary>
        BulletHole bulletHole = new BulletHole();

        /// <summary>
        /// a flag indicates if the state is loaded for the first time during gameplay
        /// </summary>
        public static bool FirstRunHighscore = true;

        /// <summary>
        /// label of the user total score
        /// </summary>
        TPString userScore;

        /// <summary>
        /// Cyril, Peter
        /// load the necessary items that needed to display on screen
        /// </summary>
        protected override void Load()
        {
            base.Load();
            using (DBCommServiceClient srv = new DBCommServiceClient())
            {
                scores = srv.GetMinigameHighScores(GameLauncher_LoginButtonSprite.getUser(), 1);
            }
            highscoreCount = 0;
            if (scores.Count > 0)
            {
                for (int i = 0; i < scores.Count; i++)
                {
                    if (highscoreCount <= 10)
                    {
                        Items.Add(new TPString(string.Format("{0,-10}{1,15}{2,35}", string.Format("{0}", i + 1), scores[i].Score.ToString(), scores[i].CharacterName)));
                    }
                    else
                    {
                        break;
                    }
                    highscoreCount++;
                }
            }
            userScore = new TPString("User Score: " + Bullseye.TotalScore);
            myLayers = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                myLayers[x] = new TPLayer(this.layers);
            }

            bullseye = new Bullseye(new Vector2(200, 200));
            myLayers[2].AddEntity(bullseye);

            backgroundTexture = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/minigame/highscore"));
            backgroundTexture.Position = new Vector2(backgroundTexture.Position.X, backgroundTexture.Position.Y);
            myLayers[0].AddEntity(backgroundTexture);
            myLayers[1].AddEntity(bulletHole);
            myLayers[1].AddEntity(note);
            myLayers[1].AddEntity(userScore);

            userScore.Position.X = 30;
            userScore.Position.Y = 55;
            userScore.RenderColor = Color.AliceBlue;

            note.Position.X = 820;
            note.Position.Y = 55;
            note.RenderColor = Color.AliceBlue;

            if (Items.Count > 0)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    myLayers[1].AddEntity(Items[i]);
                }

                Items[0].Position.X = 150;
                Items[0].Position.Y = 200;
                Items[0].RenderColor = Color.AliceBlue;
                Items[0].centerText = false;
                for (int i = 1; i < Items.Count; i++)
                {
                    Items[i].Position.X = 150;
                    Items[i].Position.Y = Items[i - 1].Position.Y + 50;
                    Items[i].RenderColor = Color.AliceBlue;
                }
            }
            if (FirstRunHighscore)
            {
                TPEngine.Get().SpriteDictionary.Add("bulletHoleHighscore", bulletHole);
                TPEngine.Get().SpriteDictionary.Add("MinigameBullseyeHighscore", bullseye);
                FirstRunHighscore = false;
                if (Items.Count > 0)
                {
                    for (int i = 0; i < Items.Count; i++)
                    {
                        TPEngine.Get().StringDictionary.Add(string.Format("Score{0}", i), (TPString)Items[i]);
                    }
                }
                TPEngine.Get().StringDictionary.Add("EndGameNote", note);
            }
        }

        /// <summary>
        /// Cyril
        /// Updates the state with each iteration.
        /// </summary>
        /// <param name="gameTime">The amount of time the game has been running for.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //if the Esc key is pressed, return to the main game
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                TPEngine.Get().State.PopState();
                TPEngine.Get().State.PopState();
            }
        }
    }
}
